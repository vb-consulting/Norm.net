using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///
        /// Summary:
        ///     Execute SQL command asynchronously.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command);
            return connection;
        }
        ///
        /// Summary:
        ///      Execute SQL command asynchronously with positional parameter values.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, params object[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }
        ///
        /// Summary:
        ///     Execute SQL command asynchronously with named parameter values.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuples array - (string name, object value).
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }
        ///
        /// Summary:
        ///     Execute SQL command asynchronously with named parameter values DbType type for each parameter.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuples array - (string name, object value).
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }
        ///
        /// Summary:
        ///     Execute SQL command asynchronously with named parameter values and custom type for each parameter.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuples array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }
    }
}
