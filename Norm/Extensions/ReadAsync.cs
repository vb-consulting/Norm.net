using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command) 
            => connection.GetNoOrmInstance().ReadAsync(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command, params object[] parameters) 
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///<summary>
        ///     Maps command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
    }
}