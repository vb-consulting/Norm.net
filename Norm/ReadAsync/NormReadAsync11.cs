using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Norm.Mapper;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public virtual IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command,
            object parameters = null,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            if (parameters != null)
            {
                this.WithParameters(parameters);
            }
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
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

            if (this.readerCallback == null)
            {
                if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
                {
                    return ReadToArrayInternalAsync(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
                }
                return ReadCallbackAsync(command, async r => (
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

            if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayWithSetInternalAsync(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadCallbackAsync(command, async r => (
                await GetFieldValueWithReaderCallbackAsync<T1>(r, 0, t1.type),
                await GetFieldValueWithReaderCallbackAsync<T2>(r, 1, t2.type),
                await GetFieldValueWithReaderCallbackAsync<T3>(r, 2, t3.type),
                await GetFieldValueWithReaderCallbackAsync<T4>(r, 3, t4.type),
                await GetFieldValueWithReaderCallbackAsync<T5>(r, 4, t5.type),
                await GetFieldValueWithReaderCallbackAsync<T6>(r, 5, t6.type),
                await GetFieldValueWithReaderCallbackAsync<T7>(r, 6, t7.type),
                await GetFieldValueWithReaderCallbackAsync<T8>(r, 7, t8.type),
                await GetFieldValueWithReaderCallbackAsync<T9>(r, 8, t9.type),
                await GetFieldValueWithReaderCallbackAsync<T10>(r, 9, t10.type),
                await GetFieldValueWithReaderCallbackAsync<T11>(r, 10, t11.type)));
        }

        ///<summary>
        ///Parse interpolated (formattable) command as database parameters and map command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public virtual IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            FormattableString command,
            object parameters = null,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            if (parameters != null)
            {
                this.WithParameters(parameters);
            }
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
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

            if (this.readerCallback == null)
            {
                if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
                {
                    return ReadToArrayInternalAsync(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
                }
                return ReadCallbackAsync(command, async r => (
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

            if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayWithSetInternalAsync(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadCallbackAsync(command, async r => (
                await GetFieldValueWithReaderCallbackAsync<T1>(r, 0, t1.type),
                await GetFieldValueWithReaderCallbackAsync<T2>(r, 1, t2.type),
                await GetFieldValueWithReaderCallbackAsync<T3>(r, 2, t3.type),
                await GetFieldValueWithReaderCallbackAsync<T4>(r, 3, t4.type),
                await GetFieldValueWithReaderCallbackAsync<T5>(r, 4, t5.type),
                await GetFieldValueWithReaderCallbackAsync<T6>(r, 5, t6.type),
                await GetFieldValueWithReaderCallbackAsync<T7>(r, 6, t7.type),
                await GetFieldValueWithReaderCallbackAsync<T8>(r, 7, t8.type),
                await GetFieldValueWithReaderCallbackAsync<T9>(r, 8, t9.type),
                await GetFieldValueWithReaderCallbackAsync<T10>(r, 9, t10.type),
                await GetFieldValueWithReaderCallbackAsync<T11>(r, 10, t11.type)));
        }
    }
}