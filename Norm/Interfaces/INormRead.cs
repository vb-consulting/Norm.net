using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormRead
    {
        ///<summary>
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        IEnumerable<(string name, object value)[]> Read(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        IEnumerable<(string name, object value)[]> Read(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        /// Maps command results to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        IEnumerable<T> Read<T>(string command);
        ///<summary>
        /// Maps command results with positional parameter values to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        IEnumerable<T> Read<T>(string command, params object[] parameters);
        ///<summary>
        /// Maps command results with named parameter values to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        IEnumerable<T> Read<T>(string command, params (string name, object value)[] parameters);
        ///<summary>
        /// Maps command results with named parameter values and DbType type for each parameter to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        IEnumerable<T> Read<T>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        /// Maps command results with named parameter values and custom type for each parameter to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        IEnumerable<T> Read<T>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        IEnumerable<(T1, T2)> Read<T1, T2>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, object type)[] parameters);

        ///<summary>
        ///     Maps command results to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, object type)[] parameters);
    }
}