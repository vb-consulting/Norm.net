using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///
        /// Summary:
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command) 
            => connection.GetNoOrmInstance().ReadAsync(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of name and value tuple arrays.
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
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command, params object[] parameters) 
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of name and value tuple arrays.
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
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of single values of type T.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of single values of type T
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of single values of type T.
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
        ///     IAsyncEnumerable async enumerator of single values of type T.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of single values of type T.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of single values of type T.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of single values of type T.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of single values of type T.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of single values of type T.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of single values of type T.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of two value tuples (T1, T2).
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
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of three value tuples (T1, T2, T3).
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
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of four value tuples (T1, T2, T3, T4).
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
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of five value tuples (T1, T2, T3, T4, T5).
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
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
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
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
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
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
    }
}