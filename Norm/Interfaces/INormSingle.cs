using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormSingle
    {
        ///
        /// Summary:
        ///     Maps command results to name and value tuple array.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     name and value tuple array.
        (string name, object value)[] Single(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to name and value tuple array.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     name and value tuple array.
        (string name, object value)[] Single(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to name and value tuple array.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     name and value tuple array.
        (string name, object value)[] Single(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to name and value tuple array.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     name and value tuple array.
        (string name, object value)[] Single(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to name and value tuple array.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     name and value tuple array.
        (string name, object value)[] Single(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to single value of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     single values of type T.
        T Single<T>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to single value of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     single values of type T.
        T Single<T>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to single value of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     single values of type T.
        T Single<T>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to single value of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     single values of type T.
        T Single<T>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to single value of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     single values of type T.
        T Single<T>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to two value tuple (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     two value tuple (T1, T2).
        (T1, T2) Single<T1, T2>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to two value tuple (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     two value tuple (T1, T2).
        (T1, T2) Single<T1, T2>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to two value tuple (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     two value tuple (T1, T2).
        (T1, T2) Single<T1, T2>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to two value tuple (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     two value tuple (T1, T2).
        (T1, T2) Single<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to two value tuple (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     two value tuple (T1, T2).
        (T1, T2) Single<T1, T2>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to three value tuple (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     three value tuple (T1, T2, T3).
        (T1, T2, T3) Single<T1, T2, T3>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to three value tuple (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     three value tuple (T1, T2, T3).
        (T1, T2, T3) Single<T1, T2, T3>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to three value tuple (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     three value tuple (T1, T2, T3).
        (T1, T2, T3) Single<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to three value tuple (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     three value tuple (T1, T2, T3).
        (T1, T2, T3) Single<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to three value tuple (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     three value tuple (T1, T2, T3).
        (T1, T2, T3) Single<T1, T2, T3>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to four value tuple (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     four value tuple (T1, T2, T3, T4).
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to four value tuple (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     four value tuple (T1, T2, T3, T4).
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to four value tuple (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     four value tuple (T1, T2, T3, T4).
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to four value tuple (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     four value tuple (T1, T2, T3, T4).
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to four value tuple (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     four value tuple (T1, T2, T3, T4).
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to five value tuple (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     five value tuple (T1, T2, T3, T4, T5).
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to five value tuple (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     five value tuple (T1, T2, T3, T4, T5).
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to five value tuple (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     five value tuple (T1, T2, T3, T4, T5).
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to five value tuple (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     five value tuple (T1, T2, T3, T4, T5).
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to five value tuple (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     five value tuple (T1, T2, T3, T4, T5).
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to six value tuple (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to six value tuple (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to six value tuple (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to six value tuple (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to six value tuple (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     six value tuple (T1, T2, T3, T4, T5, T6).
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, object type)[] parameters);
    }
}