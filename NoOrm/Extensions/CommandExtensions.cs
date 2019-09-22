using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public static class CommandExtensions
    {
        private static readonly char[] NonCharacters =
            {' ', '\n', '\r', ',', ';', ':', '-', '!', '"', '#', '$', '%', '&', '/', '(', ')', '=', '?', '*', '\\', '.'};

        private const string ParamPrefix = "@";

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
            if (cmd.CommandType == CommandType.StoredProcedure)
            {
                throw new ArgumentException("Cannot use positional parameters with command type StoredProcedure. Use named parameters instead.");
            }
            var command = cmd.CommandText;
            var paramIndex = 0;
            for (var index = 0; ; index += ParamPrefix.Length)
            {
                index = command.IndexOf(ParamPrefix, index, StringComparison.Ordinal);
                if (index == -1)
                    break;
                index++;
                var endOf = command.IndexOfAny(NonCharacters, index);
                var name = endOf == -1 ? command.Substring(index) : command.Substring(index, endOf - index);
                cmd.AddParamWithValue(name, parameters[paramIndex++]);
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
                cmd.AddParamWithValue(name, value);
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
