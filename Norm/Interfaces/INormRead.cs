using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormRead
    {
        ///
        /// Summary:
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of name and value tuple arrays.
        IEnumerable<(string name, object value)[]> Read(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of name and value tuple arrays.
        IEnumerable<(string name, object value)[]> Read(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of name and value tuple arrays.
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of name and value tuple arrays.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of name and value tuple arrays.
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of name and value tuple arrays.
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
        ///     IEnumerable enumerator of name and value tuple arrays.
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of single values of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of single values of type T.
        IEnumerable<T> Read<T>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of single values of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of single values of type T.
        IEnumerable<T> Read<T>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of single values of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of single values of type T.
        IEnumerable<T> Read<T>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of single values of type T.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of single values of type T.
        IEnumerable<T> Read<T>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of single values of type T.
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
        ///     IEnumerable enumerator of single values of type T.
        IEnumerable<T> Read<T>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        IEnumerable<(T1, T2)> Read<T1, T2>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of two value tuples (T1, T2).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of two value tuples (T1, T2).
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
        ///     IEnumerable enumerator of two value tuples (T1, T2).
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of three value tuples (T1, T2, T3).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of three value tuples (T1, T2, T3).
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
        ///     IEnumerable enumerator of three value tuples (T1, T2, T3).
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of four value tuples (T1, T2, T3, T4).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of four value tuples (T1, T2, T3, T4).
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
        ///     IEnumerable enumerator of four value tuples (T1, T2, T3, T4).
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of five value tuples (T1, T2, T3, T4, T5).
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
        ///     IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
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
        ///     IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
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
        ///     IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
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
        ///     IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
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
        ///     IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
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
        ///     IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
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
        ///     IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, object type)[] parameters);

        ///
        /// Summary:
        ///     Maps command results to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        ///
        /// Summary:
        ///     Maps command results with positional parameter values to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuple array - (string name, object value).
        ///
        /// Returns:
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuple array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
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
        ///     IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, object type)[] parameters);
    }
}