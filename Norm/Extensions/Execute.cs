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
        ///     Execute SQL command with positional parameter values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Same DbConnection instance.</returns>
        public static DbConnection Execute(this DbConnection connection, string command, params object[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
        ///<summary>
        ///     Execute SQL command with named parameter values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>Same DbConnection instance.</returns>
        public static DbConnection Execute(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
        ///<summary>
        ///     Execute SQL command with named parameter values and DbType type for each parameter.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>Same DbConnection instance.</returns>
        public static DbConnection Execute(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
        ///<summary>
        ///     Execute SQL command with named parameter values and custom type for each parameter.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Same DbConnection instance.</returns>
        public static DbConnection Execute(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
    }
}
