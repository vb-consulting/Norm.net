using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Norm.Extensions
{
    public static class CommandExtensions
    {
        private static readonly char[] NonCharacters =
            {' ', '\n', '\r', ',', ';', ':', '-', '!', '"', '#', '$', '%', '&', '/', '(', ')', '=', '?', '*', '\\', '.'};

        private const string ParamPrefix = "@";

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

        public static DbCommand AddParamWithValue(this DbCommand cmd, string name, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
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
            for (var index = 0; ; index += ParamPrefix.Length)
            {
                index = command.IndexOf(ParamPrefix, index, StringComparison.Ordinal);
                if (index == -1)
                    break;
                index++;
                var endOf = command.IndexOfAny(NonCharacters, index);
                var name = endOf == -1 ? command.Substring(index) : command[index..endOf];
                if (names.Contains(name))
                {
                    continue;
                }
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    throw new ArgumentException("Cannot use positional parameters that are not DbParameter type with command type StoredProcedure. Use named parameters instead.");
                }
                cmd.AddParamWithValue(name, values[paramIndex++]);
                if (endOf == -1)
                    break;
                index = endOf;
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
    }
}
