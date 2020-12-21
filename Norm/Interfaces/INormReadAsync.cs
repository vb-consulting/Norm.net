using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormReadAsync
    {
        ///<summary>
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        /// Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        IAsyncEnumerable<T> ReadAsync<T>(string command);
        ///<summary>
        /// Maps command results with positional parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        IAsyncEnumerable<T> ReadAsync<T>(string command, params object[] parameters);
        ///<summary>
        /// Maps command results with named parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value)[] parameters);
        ///<summary>
        /// Maps command results with named parameter values and DbType type for each parameter to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        /// Maps command results with named parameter values and custom type for each parameter to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3, T4).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params (string name, object value, object type)[] parameters);
        ///<summary>
        ///     Maps command results to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, object type)[] parameters);
    }
}