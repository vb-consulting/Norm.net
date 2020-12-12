using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Norm.Interfaces
{
    public interface INormSingleAsync
    {
        //
        // Summary:
        //     Maps command results asynchronously to name and value tuple array.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning name and value tuple array.
        ValueTask<(string name, object value)[]> SingleAsync(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to name and value tuple array.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning name and value tuple array.
        ValueTask<(string name, object value)[]> SingleAsync(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to name and value tuple array.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning name and value tuple array.
        ValueTask<(string name, object value)[]> SingleAsync(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to name and value tuple array.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning name and value tuple array.
        ValueTask<(string name, object value)[]> SingleAsync(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to name and value tuple array.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning name and value tuple array.
        ValueTask<(string name, object value)[]> SingleAsync(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to single value of type T.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning single values of type T
        ValueTask<T> SingleAsync<T>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to single value of type T.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning single values of type T.
        ValueTask<T> SingleAsync<T>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to single value of type T.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning single values of type T.
        ValueTask<T> SingleAsync<T>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to single value of type T.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning single values of type T.
        ValueTask<T> SingleAsync<T>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to single value of type T.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning single values of type T.
        ValueTask<T> SingleAsync<T>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to two value tuple (T1, T2).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning two value tuple (T1, T2).
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to two value tuple (T1, T2).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning two value tuple (T1, T2).
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to two value tuple (T1, T2).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning two value tuple (T1, T2).
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to two value tuple (T1, T2).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning two value tuple (T1, T2).
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to two value tuple (T1, T2).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning two value tuple (T1, T2).
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to three value tuple (T1, T2, T3).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning three value tuple (T1, T2, T3).
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to three value tuple (T1, T2, T3).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning three value tuple (T1, T2, T3).
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to three value tuple (T1, T2, T3).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning three value tuple (T1, T2, T3).
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to three value tuple (T1, T2, T3).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning three value tuple (T1, T2, T3).
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to three value tuple (T1, T2, T3).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning three value tuple (T1, T2, T3).
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to four value tuple (T1, T2, T3, T4).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning four value tuple (T1, T2, T3, T4).
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to four value tuple (T1, T2, T3, T4).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning four value tuple (T1, T2, T3, T4).
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to four value tuple (T1, T2, T3, T4).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning four value tuple (T1, T2, T3, T4).
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to four value tuple (T1, T2, T3, T4).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning four value tuple (T1, T2, T3, T4).
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to four value tuple (T1, T2, T3, T4).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning four value tuple (T1, T2, T3, T4).
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to five value tuple (T1, T2, T3, T4, T5).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning five value tuple (T1, T2, T3, T4, T5).
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to five value tuple (T1, T2, T3, T4, T5).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning five value tuple (T1, T2, T3, T4, T5).
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to five value tuple (T1, T2, T3, T4, T5).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning five value tuple (T1, T2, T3, T4, T5).
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to five value tuple (T1, T2, T3, T4, T5).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning five value tuple (T1, T2, T3, T4, T5).
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to five value tuple (T1, T2, T3, T4, T5).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning five value tuple (T1, T2, T3, T4, T5).
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to six value tuple (T1, T2, T3, T4, T5, T6).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning six value tuple (T1, T2, T3, T4, T5, T6).
        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to six value tuple (T1, T2, T3, T4, T5, T6).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning six value tuple (T1, T2, T3, T4, T5, T6).
        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to six value tuple (T1, T2, T3, T4, T5, T6).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning six value tuple (T1, T2, T3, T4, T5, T6).
        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to six value tuple (T1, T2, T3, T4, T5, T6).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning six value tuple (T1, T2, T3, T4, T5, T6).
        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to six value tuple (T1, T2, T3, T4, T5, T6).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning six value tuple (T1, T2, T3, T4, T5, T6).
        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, object type)[] parameters);

        //
        // Summary:
        //     Maps command results asynchronously to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        //
        // Summary:
        //     Maps command results asynchronously with positional parameter values to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuple array - (string name, object value).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and DbType type for each parameter to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, DbType type).
        //
        // Returns:
        //     A value task representing the asynchronous operation returning twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results asynchronously with named parameter values and custom type for each parameter to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuple array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     A value task representing the asynchronous operation returning twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, object type)[] parameters);
    }
}