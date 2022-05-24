using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple)
            {
                return ReadToArrayInternalAsync(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return ReadToArrayInternalAsync(command).Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            return ReadInternalAsync(command, async r => (
               await GetFieldValueAsync<T1>(r, 0, t1.type),
               await GetFieldValueAsync<T2>(r, 1, t2.type),
               await GetFieldValueAsync<T3>(r, 2, t3.type),
               await GetFieldValueAsync<T4>(r, 3, t4.type),
               await GetFieldValueAsync<T5>(r, 4, t5.type),
               await GetFieldValueAsync<T6>(r, 5, t6.type),
               await GetFieldValueAsync<T7>(r, 6, t7.type),
               await GetFieldValueAsync<T8>(r, 7, t8.type)));
        }

        ///<summary>
        ///     Maps command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
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
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback).Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            return ReadInternalAsync(command, async r => (
               await GetFieldValueAsync<T1>(r, 0, t1.type, readerCallback),
               await GetFieldValueAsync<T2>(r, 1, t2.type, readerCallback),
               await GetFieldValueAsync<T3>(r, 2, t3.type, readerCallback),
               await GetFieldValueAsync<T4>(r, 3, t4.type, readerCallback),
               await GetFieldValueAsync<T5>(r, 4, t5.type, readerCallback),
               await GetFieldValueAsync<T6>(r, 5, t6.type, readerCallback),
               await GetFieldValueAsync<T7>(r, 6, t7.type, readerCallback),
               await GetFieldValueAsync<T8>(r, 7, t8.type, readerCallback)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
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
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple)
            {
                return ReadToArrayInternalAsync(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return ReadToArrayInternalAsync(command).Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            return ReadInternalAsync(command, async r => (
               await GetFieldValueAsync<T1>(r, 0, t1.type),
               await GetFieldValueAsync<T2>(r, 1, t2.type),
               await GetFieldValueAsync<T3>(r, 2, t3.type),
               await GetFieldValueAsync<T4>(r, 3, t4.type),
               await GetFieldValueAsync<T5>(r, 4, t5.type),
               await GetFieldValueAsync<T6>(r, 5, t6.type),
               await GetFieldValueAsync<T7>(r, 6, t7.type),
               await GetFieldValueAsync<T8>(r, 7, t8.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
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
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback).Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            return ReadInternalAsync(command, async r => (
               await GetFieldValueAsync<T1>(r, 0, t1.type, readerCallback),
               await GetFieldValueAsync<T2>(r, 1, t2.type, readerCallback),
               await GetFieldValueAsync<T3>(r, 2, t3.type, readerCallback),
               await GetFieldValueAsync<T4>(r, 3, t4.type, readerCallback),
               await GetFieldValueAsync<T5>(r, 4, t5.type, readerCallback),
               await GetFieldValueAsync<T6>(r, 5, t6.type, readerCallback),
               await GetFieldValueAsync<T7>(r, 6, t7.type, readerCallback),
               await GetFieldValueAsync<T8>(r, 7, t8.type, readerCallback)));
        }
    }
}