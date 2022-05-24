using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return ReadToArrayInternalAsync(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayInternalAsync(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternalAsync(command, async r => (
                await GetFieldValueAsync<T1>(r, 0, t1.type),
                await GetFieldValueAsync<T2>(r, 1, t2.type),
                await GetFieldValueAsync<T3>(r, 2, t3.type),
                await GetFieldValueAsync<T4>(r, 3, t4.type),
                await GetFieldValueAsync<T5>(r, 4, t5.type),
                await GetFieldValueAsync<T6>(r, 5, t6.type),
                await GetFieldValueAsync<T7>(r, 6, t7.type),
                await GetFieldValueAsync<T8>(r, 7, t8.type),
                await GetFieldValueAsync<T9>(r, 8, t9.type),
                await GetFieldValueAsync<T10>(r, 9, t10.type),
                await GetFieldValueAsync<T11>(r, 10, t11.type)));
        }

        ///<summary>
        ///     Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternalAsync(command, async r => (
                await GetFieldValueAsync<T1>(r, 0, t1.type, readerCallback),
                await GetFieldValueAsync<T2>(r, 1, t2.type, readerCallback),
                await GetFieldValueAsync<T3>(r, 2, t3.type, readerCallback),
                await GetFieldValueAsync<T4>(r, 3, t4.type, readerCallback),
                await GetFieldValueAsync<T5>(r, 4, t5.type, readerCallback),
                await GetFieldValueAsync<T6>(r, 5, t6.type, readerCallback),
                await GetFieldValueAsync<T7>(r, 6, t7.type, readerCallback),
                await GetFieldValueAsync<T8>(r, 7, t8.type, readerCallback),
                await GetFieldValueAsync<T9>(r, 8, t9.type, readerCallback),
                await GetFieldValueAsync<T10>(r, 9, t10.type, readerCallback),
                await GetFieldValueAsync<T11>(r, 10, t11.type, readerCallback)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            FormattableString command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return ReadToArrayInternalAsync(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayInternalAsync(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternalAsync(command, async r => (
                await GetFieldValueAsync<T1>(r, 0, t1.type),
                await GetFieldValueAsync<T2>(r, 1, t2.type),
                await GetFieldValueAsync<T3>(r, 2, t3.type),
                await GetFieldValueAsync<T4>(r, 3, t4.type),
                await GetFieldValueAsync<T5>(r, 4, t5.type),
                await GetFieldValueAsync<T6>(r, 5, t6.type),
                await GetFieldValueAsync<T7>(r, 6, t7.type),
                await GetFieldValueAsync<T8>(r, 7, t8.type),
                await GetFieldValueAsync<T9>(r, 8, t9.type),
                await GetFieldValueAsync<T10>(r, 9, t10.type),
                await GetFieldValueAsync<T11>(r, 10, t11.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternalAsync(command, async r => (
                await GetFieldValueAsync<T1>(r, 0, t1.type, readerCallback),
                await GetFieldValueAsync<T2>(r, 1, t2.type, readerCallback),
                await GetFieldValueAsync<T3>(r, 2, t3.type, readerCallback),
                await GetFieldValueAsync<T4>(r, 3, t4.type, readerCallback),
                await GetFieldValueAsync<T5>(r, 4, t5.type, readerCallback),
                await GetFieldValueAsync<T6>(r, 5, t6.type, readerCallback),
                await GetFieldValueAsync<T7>(r, 6, t7.type, readerCallback),
                await GetFieldValueAsync<T8>(r, 7, t8.type, readerCallback),
                await GetFieldValueAsync<T9>(r, 8, t9.type, readerCallback),
                await GetFieldValueAsync<T10>(r, 9, t10.type, readerCallback),
                await GetFieldValueAsync<T11>(r, 10, t11.type, readerCallback)));
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command,
            object parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return ReadToArrayInternalAsync(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayInternalAsync(command, parameters).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternalAsync(command, async r => (
                await GetFieldValueAsync<T1>(r, 0, t1.type),
                await GetFieldValueAsync<T2>(r, 1, t2.type),
                await GetFieldValueAsync<T3>(r, 2, t3.type),
                await GetFieldValueAsync<T4>(r, 3, t4.type),
                await GetFieldValueAsync<T5>(r, 4, t5.type),
                await GetFieldValueAsync<T6>(r, 5, t6.type),
                await GetFieldValueAsync<T7>(r, 6, t7.type),
                await GetFieldValueAsync<T8>(r, 7, t8.type),
                await GetFieldValueAsync<T9>(r, 8, t9.type),
                await GetFieldValueAsync<T10>(r, 9, t10.type),
                await GetFieldValueAsync<T11>(r, 10, t11.type)), parameters);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            object parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback, parameters).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternalAsync(command, async r => (
                await GetFieldValueAsync<T1>(r, 0, t1.type, readerCallback),
                await GetFieldValueAsync<T2>(r, 1, t2.type, readerCallback),
                await GetFieldValueAsync<T3>(r, 2, t3.type, readerCallback),
                await GetFieldValueAsync<T4>(r, 3, t4.type, readerCallback),
                await GetFieldValueAsync<T5>(r, 4, t5.type, readerCallback),
                await GetFieldValueAsync<T6>(r, 5, t6.type, readerCallback),
                await GetFieldValueAsync<T7>(r, 6, t7.type, readerCallback),
                await GetFieldValueAsync<T8>(r, 7, t8.type, readerCallback),
                await GetFieldValueAsync<T9>(r, 8, t9.type, readerCallback),
                await GetFieldValueAsync<T10>(r, 9, t10.type, readerCallback),
                await GetFieldValueAsync<T11>(r, 10, t11.type, readerCallback)), parameters);
        }
    }
}