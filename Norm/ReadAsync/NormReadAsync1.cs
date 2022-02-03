using System;
using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        /// Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAsync<T>(string command)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadAsync(command).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadAsync(command).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadFormatAsync<T>(FormattableString command)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadFormatAsync(command).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadFormatAsync(command).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type));
        }

        ///<summary>
        /// Maps command results with positional parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public IAsyncEnumerable<T> ReadAsync<T>(string command, params object[] parameters)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadAsync(command, parameters).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadAsync(command, parameters).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type), parameters);
        }

        ///<summary>
        /// Maps command results with named parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value)[] parameters)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadAsync(command, parameters).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadAsync(command, parameters).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type), parameters);
        }

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
        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, object type)[] parameters)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadAsync(command, parameters).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadAsync(command, parameters).Map<T>(t1.type);
            }

            return ReadInternalUnknownParamsTypeAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type), parameters);
        }
    }
}