using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormReadAsync
    {
        ///
        /// Summary:
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuples array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of name and value tuple arrays.
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of single values of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of single values of type T
        IAsyncEnumerable<T> ReadAsync<T>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of single values of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of single values of type T.
        IAsyncEnumerable<T> ReadAsync<T>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of single values of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of single values of type T.
        IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of single values of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of single values of type T.
        IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of single values of type T.
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
        ///     IAsyncEnumerable async enumerator of single values of type T.
        IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of two value tuples (T1, T2).
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
        ///     IAsyncEnumerable async enumerator of two value tuples (T1, T2).
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of three value tuples (T1, T2, T3).
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
        ///     IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3)
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of four value tuples (T1, T2, T3, T4).
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
        ///     IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params (string name, object value, object type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of five value tuples (T1, T2, T3, T4, T5).
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
        ///     IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
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
        ///     IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
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
        ///     IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7)
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, object type)[] parameters);
    }
}