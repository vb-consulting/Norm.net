﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
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
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();

            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }

            if (this.readerCallback == null)
            {
                if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
                {
                    return ReadToArrayInternal(command).Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
                }
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type),
                    GetFieldValue<T3>(r, 2, t3.type),
                    GetFieldValue<T4>(r, 3, t4.type),
                    GetFieldValue<T5>(r, 4, t5.type),
                    GetFieldValue<T6>(r, 5, t6.type),
                    GetFieldValue<T7>(r, 6, t7.type)));
            }

            if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
            {
                return ReadToArrayWithCallbackInternal(command).Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            return ReadInternal(command, r => (
                GetFieldValueWithCallback<T1>(r, 0, t1.type),
                GetFieldValueWithCallback<T2>(r, 1, t2.type),
                GetFieldValueWithCallback<T3>(r, 2, t3.type),
                GetFieldValueWithCallback<T4>(r, 3, t4.type),
                GetFieldValueWithCallback<T5>(r, 4, t5.type),
                GetFieldValueWithCallback<T6>(r, 5, t6.type),
                GetFieldValueWithCallback<T7>(r, 6, t7.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadFormat<T1, T2, T3, T4, T5, T6, T7>(FormattableString command,
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
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();

            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }

            if (this.readerCallback == null)
            {
                if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
                {
                    return ReadToArrayInternal(command).Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
                }
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type),
                    GetFieldValue<T3>(r, 2, t3.type),
                    GetFieldValue<T4>(r, 3, t4.type),
                    GetFieldValue<T5>(r, 4, t5.type),
                    GetFieldValue<T6>(r, 5, t6.type),
                    GetFieldValue<T7>(r, 6, t7.type)));
            }

            if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
            {
                return ReadToArrayWithCallbackInternal(command).Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            return ReadInternal(command, r => (
                GetFieldValueWithCallback<T1>(r, 0, t1.type),
                GetFieldValueWithCallback<T2>(r, 1, t2.type),
                GetFieldValueWithCallback<T3>(r, 2, t3.type),
                GetFieldValueWithCallback<T4>(r, 3, t4.type),
                GetFieldValueWithCallback<T5>(r, 4, t5.type),
                GetFieldValueWithCallback<T6>(r, 5, t6.type),
                GetFieldValueWithCallback<T7>(r, 6, t7.type)));
        }
    }
}