using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormQueryAsync
    {
        //
        // Summary:
        //     Maps command results to async instance enumerator.
        //
        // Parameters:
        //   command:
        //     SQL Command text.
        //
        //   T:
        //    Type of instances that name and value tuples array will be mapped to. 
        //
        // Returns:
        //     IAsyncEnumerable async enumerator of instances of type T.
        IAsyncEnumerable<T> QueryAsync<T>(string command);
        //
        // Summary:
        //     Maps command results with positional parameter values to async instance enumerator.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        //   T:
        //    Type of instances that name and value tuples array will be mapped to. 
        //
        // Returns:
        //     IAsyncEnumerable async enumerator of instances of type T.
        IAsyncEnumerable<T> QueryAsync<T>(string command, params object[] parameters);
        //
        // Summary:
        //     Maps command results with named parameter values to async instance enumerator.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuples array - (string name, object value).
        //
        //   T:
        //    Type of instances that name and value tuples array will be mapped to. 
        //
        // Returns:
        //     IAsyncEnumerable async enumerator of instances of type T.
        IAsyncEnumerable<T> QueryAsync<T>(string command, params (string name, object value)[] parameters);
        //
        // Summary:
        //     Maps command results with named parameter values and DbType type for each parameter to async instance enumerator.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuples array - (string name, object value, DbType type).
        //
        //   T:
        //    Type of instances that name and value tuples array will be mapped to. 
        //
        // Returns:
        //     IAsyncEnumerable async enumerator of instances of type T.
        IAsyncEnumerable<T> QueryAsync<T>(string command, params (string name, object value, DbType type)[] parameters);
        //
        // Summary:
        //     Maps command results with named parameter values and custom type for each parameter to async instance enumerator.
        //
        // Parameters:
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuples array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        //   T:
        //    Type of instances that name and value tuples array will be mapped to. 
        //
        // Returns:
        //     IAsyncEnumerable async enumerator of instances of type T.
        IAsyncEnumerable<T> QueryAsync<T>(string command, params (string name, object value, object type)[] parameters);
    }
}