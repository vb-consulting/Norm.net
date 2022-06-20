using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Norm.Mapper;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public virtual IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command,
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

            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternalAsync(command).MapValueTuple<T1, T2>(t1.type, t2.type);
            }

            if (this.readerCallback == null)
            {
                if (!t1.simple && !t2.simple)
                {
                    return ReadToArrayInternalAsync(command).Map<T1, T2>(t1.type, t2.type);
                }
                return ReadCallbackAsync(command, async r => (
                    await GetFieldValueAsync<T1>(r, 0, t1.type),
                    await GetFieldValueAsync<T2>(r, 1, t2.type)));
            }

            if (!t1.simple && !t2.simple)
            {
                return ReadToArrayWithSetInternalAsync(command).Map<T1, T2>(t1.type, t2.type);
            }
            return ReadCallbackAsync(command, async r => (
                await GetFieldValueWithReaderCallbackAsync<T1>(r, 0, t1.type),
                await GetFieldValueWithReaderCallbackAsync<T2>(r, 1, t2.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public virtual IAsyncEnumerable<(T1, T2)> ReadFormatAsync<T1, T2>(FormattableString command,
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

            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternalAsync(command).MapValueTuple<T1, T2>(t1.type, t2.type);
            }

            if (this.readerCallback == null)
            {
                if (!t1.simple && !t2.simple)
                {
                    return ReadToArrayInternalAsync(command).Map<T1, T2>(t1.type, t2.type);
                }
                return ReadCallbackAsync(command, async r => (
                    await GetFieldValueAsync<T1>(r, 0, t1.type),
                    await GetFieldValueAsync<T2>(r, 1, t2.type)));
            }

            if (!t1.simple && !t2.simple)
            {
                return ReadToArrayWithSetInternalAsync(command).Map<T1, T2>(t1.type, t2.type);
            }
            return ReadCallbackAsync(command, async r => (
                await GetFieldValueWithReaderCallbackAsync<T1>(r, 0, t1.type),
                await GetFieldValueWithReaderCallbackAsync<T2>(r, 1, t2.type)));
        }
    }
}