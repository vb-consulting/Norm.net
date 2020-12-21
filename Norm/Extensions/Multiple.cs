using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Norm.Interfaces;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Disposable INormMultipleReader instance.</returns>
        public static INormMultipleReader Multiple(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().Multiple(command);
        }
        ///<summary>
        ///     Execute SQL command with positional parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Disposable INormMultipleReader instance.</returns>
        public static INormMultipleReader Multiple(this DbConnection connection, string command, params object[] parameters)
        {
            return connection.GetNoOrmInstance().Multiple(command, parameters);
        }
        ///<summary>
        ///     Execute SQL command with named parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>Same DbConnection instance.</returns>
        public static INormMultipleReader Multiple(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            return connection.GetNoOrmInstance().Multiple(command, parameters);
        }
        ///<summary>
        ///     Execute SQL command with named parameter values and DbType type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>Disposable INormMultipleReader instance.</returns>
        public static INormMultipleReader Multiple(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            return connection.GetNoOrmInstance().Multiple(command, parameters);
        }
        ///<summary>
        ///     Execute SQL command with named parameter values and custom type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Disposable INormMultipleReader instance.</returns>
        public static INormMultipleReader Multiple(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            return connection.GetNoOrmInstance().Multiple(command, parameters);
        }
        ///<summary>
        ///     Execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        public static ValueTask<INormMultipleReader> MultipleAsync(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().MultipleAsync(command);
        }
        ///<summary>
        ///     Execute SQL command asynchronously with positional parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        public static ValueTask<INormMultipleReader> MultipleAsync(this DbConnection connection, string command, params object[] parameters)
        {
            return connection.GetNoOrmInstance().MultipleAsync(command, parameters);
        }
        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        public static ValueTask<INormMultipleReader> MultipleAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            return connection.GetNoOrmInstance().MultipleAsync(command, parameters);
        }
        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and DbType type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        public static ValueTask<INormMultipleReader> MultipleAsync(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            return connection.GetNoOrmInstance().MultipleAsync(command, parameters);
        }
        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and custom type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        public static ValueTask<INormMultipleReader> MultipleAsync(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            return connection.GetNoOrmInstance().MultipleAsync(command, parameters);
        }
    }
}
