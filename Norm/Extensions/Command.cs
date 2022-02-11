using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Norm
{
    internal static class CommandExtensions
    {
        public static DbCommand SetCommandParameters(this DbCommand cmd, string command, CommandType type, int? timeout)
        {
            cmd.CommandText = command;
            cmd.CommandType = type;
            if (timeout != null)
            {
                cmd.CommandTimeout = timeout.Value;
            }

            return cmd;
        }

        public static DbCommand AddParamWithValue(this DbCommand cmd, string name, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;

            cmd.Parameters.Add(param);
            return cmd;
        }

        public static DbCommand AddParamWithValue(this DbCommand cmd, string name, object value, object type)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            if (type != null)
            {
                var paramType = type.GetType();
                if (!paramType.IsEnum && paramType != TypeExt.IntType)
                {
                    throw new ArgumentException($"Wrong parameter type: {name}");
                }
                var paramTypeName = paramType.Name;
                var propertyInfo = param.GetType().GetProperty(paramTypeName);
                propertyInfo.SetValue(param, type);
            }
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static DbCommand AddParameters(this DbCommand cmd, params object[] parameters)
        {
            var command = cmd.CommandText;
            var valueList = new List<object>();
            var names = new HashSet<string>(parameters.Length);

            void AddPropValues(Type type, object p)
            {
                foreach (var prop in type.GetProperties())
                {
                    if (prop.PropertyType.BaseType == TypeExt.DbParameterType || prop.PropertyType == TypeExt.DbParameterType)
                    {
                        cmd.Parameters.Add(prop.GetValue(p) as DbParameter);
                    } 
                    else
                    {
                        var propMeta = prop.PropertyType.GetMetadata();
                        if (propMeta.valueTuple)
                        {
                            var tupleValue = prop.GetValue(p);
                            var tupleType = tupleValue.GetType();
                            var f1 = tupleType.GetField("Item1");
                            var f2 = tupleType.GetField("Item2");
                            if (f1 == null || f2 == null)
                            {
                                throw new ArgumentException(@$"Wrong parameter type: {prop.Name}.

Tuples in parameter values are only allowed to set specific database type like this for example:
new {{
    id = (1, NpgsqlDbType.Integer),
    name = (""some text"", DbType.String)
}}
");
                            }
                            cmd.AddParamWithValue(prop.Name, f1.GetValue(tupleValue), f2.GetValue(tupleValue));
                        }
                        else if (propMeta.simple)
                        {
                            cmd.AddParamWithValue(prop.Name, prop.GetValue(p));
                            names.Add(prop.Name);
                        }
                    }
                    
                }
            }
            foreach (var p in parameters)
            {
                if (p is DbParameter dbParameter)
                {
                    cmd.Parameters.Add(dbParameter);
                    names.Add(dbParameter.ParameterName);
                }
                else
                {
                    if (p == null)
                    {
                        valueList.Add(p);
                        continue;
                    }
                    var type = p.GetType();

                    if (type.IsAnonymousType())
                    {
                        AddPropValues(type, p);
                        continue;
                    }

                    var meta = type.GetMetadata();
                    if (meta.simple)
                    {
                        valueList.Add(p);
                        continue;
                    }

                    AddPropValues(type, p);
                }
            }
            var values = valueList.ToArray();
            if (values.Length == 0)
            {
                return cmd;
            }

            var paramIndex = 0;
            var used = new HashSet<string>(values.Length);
            foreach (var name in EnumerateParams(command, names).Select(t => t.name))
            {
                if (used.Contains(name))
                {
                    throw new NormParametersException(name);
                }
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    throw new NormPositionalParametersWithStoredProcedureException();
                }
                cmd.AddParamWithValue(name, values[paramIndex++]);
                used.Add(name);
            }
            return cmd;
        }

        public static Task<int> ExecuteNonQueryWithOptionalTokenAsync(this DbCommand cmd, CancellationToken? cancellationToken)
        {
            return cancellationToken.HasValue ? cmd.ExecuteNonQueryAsync(cancellationToken.Value) : cmd.ExecuteNonQueryAsync();
        }

        private static readonly char[] NonCharacters =
            {' ', '\n', '\r', ',', ';', ':', '-', '!', '"', '#', '$', '%', '&', '/', '(', ')', '=', '?', '*', '\\', '.'};

        private const string ParamPrefix = "@";

        private static IEnumerable<(string name, int index, int count)> EnumerateParams(string command, ICollection<string> skip = null)
        {
            for (var index = 0; ; index += ParamPrefix.Length)
            {
                index = command.IndexOf(ParamPrefix, index, StringComparison.Ordinal);
                if (index == -1)
                    break;
                if (index == command.Length-1)
                    break;
                // skip sqlservers @@ variables
                if (command[index + 1] == '@')
                {
                    index += 2;
                    continue;
                }
                
                index++;
                var endOf = command.IndexOfAny(NonCharacters, index);
                var name = endOf == -1 ? command[index..] : command[index..endOf];
                if (index - 9 > 0 && string.Equals(command[(index - 9)..(index - 2)], "declare", StringComparison.OrdinalIgnoreCase))
                {
                    if (skip == null)
                    {
                        skip = new HashSet<string>();
                    }
                    skip.Add(name);
                    continue;
                }
                if (skip != null && skip.Contains(name))
                {
                    continue;
                }
                yield return (name, index, name.Length);
                if (endOf == -1)
                    break;
                index = endOf;
            }
        }
    }
}
