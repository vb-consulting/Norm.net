using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        protected static readonly char[] NonCharacters =
            {' ', '\n', '\r', ',', ';', ':', '-', '!', '"', '#', '$', '%', '&', '/', '(', ')', '=', '?', '*', '\\', '.'};

        protected const string ParamPrefix = "@";

        protected void AddParametersInternal(DbCommand cmd, params object[] parameters)
        {
            var command = cmd.CommandText;
            var valueList = new List<object>();
            var names = new HashSet<string>(parameters.Length);

            void AddPropValues(Type type, object p)
            {
                foreach (var prop in type.GetProperties())
                {
                    if (prop.PropertyType.BaseType == typeof(DbParameter) || prop.PropertyType == typeof(DbParameter))
                    {
                        AddDbParamInternal(cmd, prop.GetValue(p) as DbParameter);
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
                            AddParamWithValueInternal(cmd, prop.Name, f1.GetValue(tupleValue), f2.GetValue(tupleValue));
                        }
                        else if (propMeta.simple)
                        {
                            AddParamWithValueInternal(cmd, prop.Name, prop.GetValue(p));
                            names.Add(prop.Name);
                        }
                    }

                }
            }
            foreach (var p in parameters)
            {
                if (p is DbParameter dbParameter)
                {
                    AddDbParamInternal(cmd, dbParameter);
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
                return;
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
                AddParamWithValueInternal(cmd, name, values[paramIndex++]);
                used.Add(name);
            }
        }

        protected void AddParamWithValueInternal(DbCommand cmd, string name, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            AddDbParamInternal(cmd, param);
        }

        protected void AddParamWithValueInternal(DbCommand cmd, string name, object value, object type)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            if (type != null)
            {
                var paramType = type.GetType();
                if (!paramType.IsEnum && paramType != typeof(int))
                {
                    throw new ArgumentException($"Wrong parameter type: {name}");
                }
                var paramTypeName = paramType.Name;
                var propertyInfo = param.GetType().GetProperty(paramTypeName);
                propertyInfo.SetValue(param, type);
            }
            AddDbParamInternal(cmd, param);
        }

        protected void AddDbParamInternal(DbCommand cmd, DbParameter dbParameter)
        {
            if (this.dbType != DatabaseType.Npgsql)
            {
                if (dbParameter.Value is System.Collections.ICollection)
                {
                    var name = dbParameter.ParameterName;
                    var newNames = new List<string>(128);
                    uint i = 0;
                    var enumerator = (dbParameter.Value as System.Collections.IEnumerable).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        var value = enumerator.Current;
                        var param = cmd.CreateParameter();
                        param.ParameterName = $"__{name}{i++}";
                        param.Value = value ?? DBNull.Value;
                        cmd.Parameters.Add(param);
                        newNames.Add($"@{param.ParameterName}");
                    }
                    cmd.CommandText = cmd.CommandText.Replace($"@{name}", string.Join(", ", newNames));
                    return;
                }
                cmd.Parameters.Add(dbParameter);
                return;
            }
            cmd.Parameters.Add(dbParameter);
        }

        protected static IEnumerable<(string name, int index, int count)> EnumerateParams(string command, ICollection<string> skip = null)
        {
            for (var index = 0; ; index += ParamPrefix.Length)
            {
                index = command.IndexOf(ParamPrefix, index, StringComparison.Ordinal);
                if (index == -1)
                    break;
                if (index == command.Length - 1)
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

        protected (string commandString, object[] parameters) ParseFormattableCommand(FormattableString command)
        {
            var args = command.GetArguments();
            var parameters = new List<object>(args.Length);
            var commandString = string.Format(command.Format, args.Select((p, idx) =>
            {
                if (p is DbParameter dbParameter)
                {
                    dbParameter.ParameterName = $"p{idx}";
                    parameters.Add(p);
                    return $"@p{idx}";
                }
                if (p is string)
                {
                    if (command.Format.Contains($"{{{idx}:{NormOptions.Value.RawInterpolationParameterEscape}"))
                    {
                        return p;
                    }
                }
                parameters.Add(p);
                return $"@p{idx}";
            }).ToArray());

            return (commandString, parameters.ToArray());
        }
    }
}
