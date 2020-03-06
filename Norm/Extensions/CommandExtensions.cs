using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Norm.Extensions
{
    public static class CommandExtensions
    {
        public static DbCommand SetCommandParameters(this DbCommand cmd, string command, CommandType type, int? timeout)
        {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            cmd.CommandText = command;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
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

        public static DbCommand AddParameters(this DbCommand cmd, params object[] parameters)
        {
            var command = cmd.CommandText;
            var valueList = new List<object>();
            var nameList = new List<string>();
            foreach (var p in parameters)
            {
                if (p is DbParameter dbParameter)
                {
                    cmd.Parameters.Add(dbParameter);
                    nameList.Add(dbParameter.ParameterName);
                }
                else
                {
                    valueList.Add(p);
                }
            }
            var values = valueList.ToArray();
            if (values.Length == 0)
            {
                return cmd;
            }
            var names = nameList.ToArray();
            var paramIndex = 0;
            foreach (var (name, _, _) in EnumerateParams(command, names))
            {
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    throw new ArgumentException("Cannot use positional parameters that are not DbParameter type with command type StoredProcedure. Use named parameters instead.");
                }
                cmd.AddParamWithValue(name, values[paramIndex++]);
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

        public static DbCommand AddParameters(this DbCommand cmd, params (string name, object value, DbType type)[] parameters)
        {
            foreach (var (name, value, type) in parameters)
            {
                if (name != null)
                {
                    cmd.AddParamWithValue(name, value, type);
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
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            cmd.CommandText = command.ToString();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            return cmd;
        }

        public static DbCommand AddPgFormatParameters(this DbCommand cmd, params (string name, object value)[] parameters)
        {
            return cmd.AddPgFormatParametersFromHash(parameters.ToDictionary(p => p.name, p => p.value));
        }

        public static DbCommand AddPgFormatParameters(this DbCommand cmd, params (string name, object value, DbType type)[] parameters)
        {
            return cmd.AddPgFormatParametersFromHash(parameters.ToDictionary(p => p.name, p => p.value));
        }

        private static readonly char[] NonCharacters =
            {' ', '\n', '\r', ',', ';', ':', '-', '!', '"', '#', '$', '%', '&', '/', '(', ')', '=', '?', '*', '\\', '.'};

        private const string ParamPrefix = "@";

        private static IEnumerable<(string name, int index, int count)> EnumerateParams(string command, string[] skip = null)
        {
            for (var index = 0; ; index += ParamPrefix.Length)
            {
                index = command.IndexOf(ParamPrefix, index, StringComparison.Ordinal);
                if (index == -1)
                    break;
                index++;
                var endOf = command.IndexOfAny(NonCharacters, index);
                var name = endOf == -1 ? command.Substring(index) : command[index..endOf];
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
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            cmd.CommandText = command.ToString();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            return cmd;
        }
    }
}
