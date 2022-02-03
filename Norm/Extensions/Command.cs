using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public static DbCommand AddParamWithValue(this DbCommand cmd, string name, object value, DbType? type = null)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            if (type.HasValue)
            {
                param.DbType = type.Value;
            }
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static DbCommand AddUnknownParamTypeWithValue(this DbCommand cmd, string name, object value, object type = null)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            if (type != null)
            {
                var paramType = type.GetType();
                if (!paramType.IsEnum && paramType != TypeExt.IntType)
                {
                    throw new NormWrongTypeParameterException(name);
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
                    var propMeta = prop.PropertyType.GetMetadata();
                    if (propMeta.simple)
                    {
                        cmd.AddParamWithValue(prop.Name, prop.GetValue(p));
                        names.Add(prop.Name);
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

        public static DbCommand AddParameters(this DbCommand cmd, params (string name, object value)[] parameters)
        {
            foreach (var (name, value) in parameters)
            {
                if (name != null)
                {
                    cmd.AddParamWithValue(name, value);
                }
            }

            return cmd;
        }

        public static DbCommand AddUnknownTypeParameters(this DbCommand cmd, params (string name, object value, object type)[] parameters)
        {
            foreach (var (name, value, type) in parameters)
            {
                if (name != null)
                {
                    cmd.AddUnknownParamTypeWithValue(name, value, type);
                }
            }

            return cmd;
        }

        public static DbCommand AddParameters(this DbCommand cmd, Action<DbParameterCollection> parameters)
        {
            parameters?.Invoke(cmd.Parameters);
            return cmd;
        }

        public static async Task<DbCommand> AddParametersAsync(this DbCommand cmd, Func<DbParameterCollection, Task> parameters)
        {
            if (parameters != null)
            {
                await parameters.Invoke(cmd.Parameters);
            }

            return cmd;
        }

        public static Task<int> ExecuteNonQueryWithOptionalTokenAsync(this DbCommand cmd, CancellationToken? cancellationToken)
        {
            return cancellationToken.HasValue ? cmd.ExecuteNonQueryAsync(cancellationToken.Value) : cmd.ExecuteNonQueryAsync();
        }

        public static DbCommand AddPgFormatParameters(this DbCommand cmd, params object[] parameters)
        {
            var command = new StringBuilder();
            var old = cmd.CommandText;
            var paramIndex = 0;
            int prevIndex = 0;
            foreach (var (_, index, len) in EnumerateParams(old))
            {
                command.Append(old.Substring(prevIndex, index - prevIndex - 1));
                command.Append(GetPgFormatExp(parameters[paramIndex++]));
                prevIndex = index + len;
            }
            command.Append(old.Substring(prevIndex, old.Length - prevIndex - 1));
            cmd.CommandText = command.ToString();
            return cmd;
        }

        public static DbCommand AddPgFormatParameters(this DbCommand cmd, params (string name, object value)[] parameters)
        {
            return cmd.AddPgFormatParametersFromHash(parameters.ToDictionary(p => p.name, p => p.value));
        }

        public static DbCommand AddPgFormatParameters(this DbCommand cmd, params (string name, object value, object type)[] parameters)
        {
            return cmd.AddPgFormatParametersFromHash(parameters.ToDictionary(p => p.name, p => p.value));
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

        private static string GetPgFormatExp(object value)
        {
            var code = Type.GetTypeCode(value.GetType());
            return string.Concat("format('%s', '", value, "')", code switch
            {
                TypeCode.Boolean => "::boolean",
                TypeCode.Byte => "::int",
                TypeCode.Char => "",
                TypeCode.DateTime => "::timestamp",
                TypeCode.Decimal => "::decimal",
                TypeCode.Double => "::float8",
                TypeCode.Int16 => "::smallint",
                TypeCode.Int32 => "::int",
                TypeCode.Int64 => "::bigint",
                TypeCode.SByte => "::smallint",
                TypeCode.Single => "::smallint",
                TypeCode.String => "",
                TypeCode.UInt16 => "::int",
                TypeCode.UInt32 => "::bigint",
                TypeCode.UInt64 => "::bigint",
                TypeCode.DBNull => "null",
                TypeCode.Empty => "null",
                TypeCode.Object => throw new ArgumentException("Parameter type object cannot be used this way."),
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        private static DbCommand AddPgFormatParametersFromHash(this DbCommand cmd, IDictionary<string, object> paramHash)
        {
            var command = new StringBuilder();
            var old = cmd.CommandText;
            int prevIndex = 0;
            foreach (var (name, index, len) in EnumerateParams(old))
            {
                command.Append(old.Substring(prevIndex, index - prevIndex - 1));
                command.Append(GetPgFormatExp(paramHash[name]));
                prevIndex = index + len;
            }
            command.Append(old.Substring(prevIndex, old.Length - prevIndex - 1));
            cmd.CommandText = command.ToString();
            return cmd;
        }
    }
}
