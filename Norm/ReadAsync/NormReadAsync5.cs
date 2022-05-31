﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///Maps command results to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public virtual IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();

            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple)
            {
                return ReadToArrayInternalAsync(command).MapValueTuple<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }

            if (this.readerCallback == null)
            {
                if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
                {
                    return ReadToArrayInternalAsync(command).Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
                }
                return ReadCallbackAsync(command, async r => (
                    await GetFieldValueAsync<T1>(r, 0, t1.type),
                    await GetFieldValueAsync<T2>(r, 1, t2.type),
                    await GetFieldValueAsync<T3>(r, 2, t3.type),
                    await GetFieldValueAsync<T4>(r, 3, t4.type),
                    await GetFieldValueAsync<T5>(r, 4, t5.type)));
            }

            if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
            {
                return ReadToArrayWithSetInternalAsync(command).Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            return ReadCallbackAsync(command, async r => (
                await GetFieldValueWithReaderCallbackAsync<T1>(r, 0, t1.type),
                await GetFieldValueWithReaderCallbackAsync<T2>(r, 1, t2.type),
                await GetFieldValueWithReaderCallbackAsync<T3>(r, 2, t3.type),
                await GetFieldValueWithReaderCallbackAsync<T4>(r, 3, t4.type),
                await GetFieldValueWithReaderCallbackAsync<T5>(r, 4, t5.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public virtual IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadFormatAsync<T1, T2, T3, T4, T5>(FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();

            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple)
            {
                return ReadToArrayInternalAsync(command).MapValueTuple<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }

            if (this.readerCallback == null)
            {
                if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
                {
                    return ReadToArrayInternalAsync(command).Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
                }
                return ReadCallbackAsync(command, async r => (
                    await GetFieldValueAsync<T1>(r, 0, t1.type),
                    await GetFieldValueAsync<T2>(r, 1, t2.type),
                    await GetFieldValueAsync<T3>(r, 2, t3.type),
                    await GetFieldValueAsync<T4>(r, 3, t4.type),
                    await GetFieldValueAsync<T5>(r, 4, t5.type)));
            }

            if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
            {
                return ReadToArrayWithSetInternalAsync(command).Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            return ReadCallbackAsync(command, async r => (
                await GetFieldValueWithReaderCallbackAsync<T1>(r, 0, t1.type),
                await GetFieldValueWithReaderCallbackAsync<T2>(r, 1, t2.type),
                await GetFieldValueWithReaderCallbackAsync<T3>(r, 2, t3.type),
                await GetFieldValueWithReaderCallbackAsync<T4>(r, 3, t4.type),
                await GetFieldValueWithReaderCallbackAsync<T5>(r, 4, t5.type)));
        }
    }
}