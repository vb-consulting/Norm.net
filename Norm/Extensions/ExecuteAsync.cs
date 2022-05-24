using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Execute SQL command asynchronously.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning the same DbConnection instance.</returns>
        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command);
            return connection;
        }
        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and execute resulting SQL asynchronously.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>A value task representing the asynchronous operation returning the same DbConnection instance.</returns>
        public static async ValueTask<DbConnection> ExecuteFormatAsync(this DbConnection connection, FormattableString command)
        {
            await connection.GetNoOrmInstance().ExecuteFormatAsync(command);
            return connection;
        }
        ///<summary>
        ///      Execute SQL command asynchronously with positional parameter values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>A value task representing the asynchronous operation returning the same DbConnection instance.</returns>
        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, object parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }
    }
}
