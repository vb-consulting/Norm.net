using System;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Execute SQL command.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>Norm instance.</returns>
        public Norm Execute(string command)
        {
            using var cmd = CreateCommand(command);
            cmd.ExecuteNonQuery();
            return this;
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and execute resulting SQL.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>Norm instance.</returns>
        public Norm ExecuteFormat(FormattableString command)
        {
            using var cmd = CreateCommand(command);
            cmd.ExecuteNonQuery();
            return this;
        }

        ///<summary>
        ///     Execute SQL command with positional parameter values.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>Norm instance.</returns>
        public Norm Execute(string command, params object[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            cmd.ExecuteNonQuery();
            return this;
        }

        ///<summary>
        ///     Execute SQL command with named parameter values.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>Norm instance.</returns>
        public Norm Execute(string command, params (string name, object value)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            cmd.ExecuteNonQuery();
            return this;
        }

        ///<summary>
        ///     Execute SQL command with named parameter values and custom type for each parameter.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Norm instance.</returns>
        public Norm Execute(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            cmd.ExecuteNonQuery();
            return this;
        }
    }
}
