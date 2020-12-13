using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to async instance enumerator.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IAsyncEnumerable async enumerator of instances of type T.</returns>
        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().QueryAsync<T>(command);
        }
        ///<summary>
        ///     Maps command results with positional parameter values to async instance enumerator.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IAsyncEnumerable async enumerator of instances of type T.</returns> 
        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params object[] parameters)
        {
            return connection.GetNoOrmInstance().QueryAsync<T>(command, parameters);
        }
        ///<summary>
        ///     Maps command results with named parameter values to async instance enumerator.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IAsyncEnumerable async enumerator of instances of type T.</returns>
        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            return connection.GetNoOrmInstance().QueryAsync<T>(command, parameters);
        }
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async instance enumerator.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IAsyncEnumerable async enumerator of instances of type T.</returns>
        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            return connection.GetNoOrmInstance().QueryAsync<T>(command, parameters);
        }
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async instance enumerator.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IAsyncEnumerable async enumerator of instances of type T.</returns>
        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            return connection.GetNoOrmInstance().QueryAsync<T>(command, parameters);
        }
    }
}
