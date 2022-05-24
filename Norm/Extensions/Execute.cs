using System;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Execute SQL command.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Same DbConnection instance.</returns>
        public static DbConnection Execute(this DbConnection connection, string command)
        {
            connection.GetNoOrmInstance().Execute(command);
            return connection;
        }
        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and execute resulting SQL.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>Same DbConnection instance.</returns>
        public static DbConnection ExecuteFormat(this DbConnection connection, FormattableString command)
        {
            connection.GetNoOrmInstance().ExecuteFormat(command);
            return connection;
        }
        ///<summary>
        ///     Execute SQL command with positional parameter values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>Same DbConnection instance.</returns>
        public static DbConnection Execute(this DbConnection connection, string command, object parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
    }
}
