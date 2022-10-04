using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Norm.Mapper;

namespace Norm
{
    public static class NormParameterParser
    {
        private static readonly char[] NonCharacters =
            {' ', '\n', '\r', ',', ';', ':', '-', '!', '"', '#', '$', '%', '&', '/', '(', ')', '=', '?', '*', '\\', '.', '\''};

        private const string ParamPrefix = "@";

        public static void AddParameters(Norm instance, IDbCommand cmd, params object[] parameters)
        {
            var command = cmd.CommandText;
            var values = new List<object>(parameters.Length);
            var names = new HashSet<string>(parameters.Length);

            void AddPropValues(Type type, object p)
            {
                foreach (var prop in type.GetProperties())
                {
                    if (prop.PropertyType.BaseType == typeof(DbParameter) || prop.PropertyType == typeof(DbParameter))
                    {
                        AddDbParamInternal(instance, cmd, prop.GetValue(p) as DbParameter);
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
                            AddParamWithValueInternal(instance, cmd, prop.Name, f1.GetValue(tupleValue), f2.GetValue(tupleValue));
                        }
                        else //if (propMeta.simple)
                        {
                            AddParamWithValueInternal(instance, cmd, prop.Name, prop.GetValue(p));
                            names.Add(prop.Name);
                        }
                    }

                }
            }
            foreach (var p in parameters)
            {
                if (p is DbParameter dbParameter)
                {
                    AddDbParamInternal(instance, cmd, dbParameter);
                    names.Add(dbParameter.ParameterName);
                }
                else
                {
                    if (p == null)
                    {
                        values.Add(p);
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
                        values.Add(p);
                        continue;
                    }

                    AddPropValues(type, p);
                }
            }
            
            if (values.Count == 0)
            {
                return;
            }

            var paramIndex = 0;
            var used = new HashSet<string>(values.Count);
            var usedIndexes = new HashSet<int>(values.Count);
            foreach (var name in EnumerateParams(command, names))
            {
                if (used.Contains(name))
                {
                    throw new NormParametersException(name);
                }
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    throw new NormPositionalParametersWithStoredProcedureException();
                }
                usedIndexes.Add(paramIndex);
                AddParamWithValueInternal(instance, cmd, name, values[paramIndex++]);
                used.Add(name);
            }
            paramIndex = 0;
            foreach (var value in values)
            {
                if (!usedIndexes.Contains(paramIndex))
                {
                    if (value != null)
                    {
                        var type = value.GetType();
                        var meta = type.GetMetadata();
                        if (meta.valueTuple)
                        {
                            var f1 = type.GetField("Item1");
                            var f2 = type.GetField("Item2");
                            if (f1 == null || f2 == null)
                            {
                                throw new ArgumentException(@$"Wrong parameter at index: {paramIndex}. Tuples in parameter values are only allowed to set specific database type like this for example: (1, NpgsqlDbType.Integer), (""some text"", DbType.String), etc");
                            }
                            AddParamWithValueInternal(instance, cmd, null, f1.GetValue(value), f2.GetValue(value));
                        }
                        else if (meta.simple)
                        {
                            AddParamWithValueInternal(instance, cmd, null, value);
                        }
                    }
                    else
                    {
                        AddParamWithValueInternal(instance, cmd, null, value);
                    }
                }
                paramIndex++;
            }
        }

        public static (string commandString, object[] parameters) ParseFormattableCommand(FormattableString command)
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

        private static void AddParamWithValueInternal(Norm instance, IDbCommand cmd, string name, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            AddDbParamInternal(instance, cmd, param);
        }

        private static void AddParamWithValueInternal(Norm instance, IDbCommand cmd, string name, object value, object type)
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
            AddDbParamInternal(instance, cmd, param);
        }

        private static void AddDbParamInternal(Norm instance, IDbCommand cmd, IDbDataParameter dbParameter)
        {
            if (instance.DbType != DatabaseType.Npgsql)
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

        private static IEnumerable<string> EnumerateParams(string command, ICollection<string> skip = null)
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
                yield return name;
                if (endOf == -1)
                    break;
                index = endOf;
            }
        }
    }
}
