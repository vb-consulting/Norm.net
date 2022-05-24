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
        public Norm Execute(string command, object parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            cmd.ExecuteNonQuery();
            return this;
        }
    }
}
