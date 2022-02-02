using System;
using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command)
        {
            return ReadToArrayInternalAsync(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadFormatAsync(FormattableString command)
        {
            return ReadToArrayInternalAsync(command);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params object[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value)[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadToArrayInternalUnknownParamsTypeAsync(command, parameters);
        }
    }
}