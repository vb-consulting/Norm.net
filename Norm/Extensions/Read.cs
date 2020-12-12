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
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of name and value tuple arrays.
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of name and value tuple arrays.
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
        ///     IEnumerable enumerator of name and value tuple arrays.
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of name and value tuple arrays.
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
        ///     IEnumerable enumerator of name and value tuple arrays.
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of name and value tuple arrays.
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
        ///     IEnumerable enumerator of name and value tuple arrays.
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of name and value tuple arrays.
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
        ///     IEnumerable enumerator of name and value tuple arrays.
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of single values of type T.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of single values of type T.
        public static IEnumerable<T> Read<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of single values of type T.
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
        ///     IEnumerable enumerator of single values of type T.
        public static IEnumerable<T> Read<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of single values of type T.
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
        ///     IEnumerable enumerator of single values of type T.
        public static IEnumerable<T> Read<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of single values of type T.
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
        ///     IEnumerable enumerator of single values of type T.
        public static IEnumerable<T> Read<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of single values of type T.
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
        ///     IEnumerable enumerator of single values of type T.
        public static IEnumerable<T> Read<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of two value tuples (T1, T2).
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
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of two value tuples (T1, T2).
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
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of two value tuples (T1, T2).
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
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of two value tuples (T1, T2).
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
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        public static IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of three value tuples (T1, T2, T3).
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
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        public static IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of three value tuples (T1, T2, T3).
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
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        public static IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of three value tuples (T1, T2, T3).
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
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        public static IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of three value tuples (T1, T2, T3).
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
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        public static IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of four value tuples (T1, T2, T3, T4).
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
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of four value tuples (T1, T2, T3, T4).
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
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of four value tuples (T1, T2, T3, T4).
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
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of four value tuples (T1, T2, T3, T4).
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
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of five value tuples (T1, T2, T3, T4, T5).
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
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of five value tuples (T1, T2, T3, T4, T5).
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
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of five value tuples (T1, T2, T3, T4, T5).
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
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of five value tuples (T1, T2, T3, T4, T5).
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
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        public static IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
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
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
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
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
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
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, DbType)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
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
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, object)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
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
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
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
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
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
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
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
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
    }
}