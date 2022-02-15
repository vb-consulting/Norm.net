using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

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
                return ReadToArrayInternalAsync(command).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayInternalAsync(command).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type));
        }

        ///<summary>
        /// Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAsync<T>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type, readerCallback));
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
                return ReadToArrayInternalAsync(command).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayInternalAsync(command).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadFormatAsync<T>(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type, readerCallback));
        }

        ///<summary>
        /// Maps command results with positional parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public IAsyncEnumerable<T> ReadAsync<T>(string command, params object[] parameters)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternalAsync(command, parameters).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayInternalAsync(command, parameters).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type), parameters);
        }

        ///<summary>
        /// Maps command results with positional parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public IAsyncEnumerable<T> ReadAsync<T>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback, parameters).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback, parameters).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type, readerCallback), parameters);
        }
    }
}