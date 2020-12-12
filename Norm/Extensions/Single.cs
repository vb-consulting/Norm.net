using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///
        /// Summary:
        ///     Maps command results to name and value tuple array.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     name and value tuple array.
        public static (string name, object value)[] Single(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to name and value tuple array.
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
        ///     name and value tuple array.
        public static (string name, object value)[] Single(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to name and value tuple array.
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
        ///     name and value tuple array.
        public static (string name, object value)[] Single(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to name and value tuple array.
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
        ///     name and value tuple array.
        public static (string name, object value)[] Single(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to name and value tuple array.
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
        ///     name and value tuple array.
        public static (string name, object value)[] Single(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to single value of type T.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     single values of type T.
        public static T Single<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to single value of type T.
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
        ///     single values of type T.
        public static T Single<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to single value of type T.
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
        ///     single values of type T.
        public static T Single<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to single value of type T.
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
        ///     single values of type T.
        public static T Single<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to single value of type T.
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
        ///     single values of type T.
        public static T Single<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to two value tuple (T1, T2).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     two value tuple (T1, T2).
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to two value tuple (T1, T2).
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
        ///     two value tuple (T1, T2).
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to two value tuple (T1, T2).
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
        ///     two value tuple (T1, T2).
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to two value tuple (T1, T2).
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
        ///     two value tuple (T1, T2).
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to two value tuple (T1, T2).
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
        ///     two value tuple (T1, T2).
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to three value tuple (T1, T2, T3).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     three value tuple (T1, T2, T3).
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to three value tuple (T1, T2, T3).
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
        ///     three value tuple (T1, T2, T3).
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to three value tuple (T1, T2, T3).
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
        ///     three value tuple (T1, T2, T3).
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to three value tuple (T1, T2, T3).
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
        ///     three value tuple (T1, T2, T3).
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to three value tuple (T1, T2, T3).
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
        ///     three value tuple (T1, T2, T3).
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to four value tuple (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     four value tuple (T1, T2, T3, T4).
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to four value tuple (T1, T2, T3, T4).
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
        ///     four value tuple (T1, T2, T3, T4).
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to four value tuple (T1, T2, T3, T4).
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
        ///     four value tuple (T1, T2, T3, T4).
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to four value tuple (T1, T2, T3, T4).
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
        ///     four value tuple (T1, T2, T3, T4).
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to four value tuple (T1, T2, T3, T4).
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
        ///     four value tuple (T1, T2, T3, T4).
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to five value tuple (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     five value tuple (T1, T2, T3, T4, T5).
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to five value tuple (T1, T2, T3, T4, T5).
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
        ///     five value tuple (T1, T2, T3, T4, T5).
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to five value tuple (T1, T2, T3, T4, T5).
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
        ///     five value tuple (T1, T2, T3, T4, T5).
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to five value tuple (T1, T2, T3, T4, T5).
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
        ///     five value tuple (T1, T2, T3, T4, T5).
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to five value tuple (T1, T2, T3, T4, T5).
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
        ///     five value tuple (T1, T2, T3, T4, T5).
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to six value tuple (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to six value tuple (T1, T2, T3, T4, T5, T6).
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
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to six value tuple (T1, T2, T3, T4, T5, T6).
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
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to six value tuple (T1, T2, T3, T4, T5, T6).
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
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to six value tuple (T1, T2, T3, T4, T5, T6).
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
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
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
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
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
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
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
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
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
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
    }
}