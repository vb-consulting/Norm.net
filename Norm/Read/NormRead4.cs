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
        ///     Maps command results to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type),
                GetFieldValue<T3>(r, 2, t3.type),
                GetFieldValue<T4>(r, 3, t4.type)));
        }

        ///<summary>
        ///     Maps command results to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback).Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                GetFieldValue<T2>(r, 1, t2.type, readerCallback),
                GetFieldValue<T3>(r, 2, t3.type, readerCallback),
                GetFieldValue<T4>(r, 3, t4.type, readerCallback)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public IEnumerable<(T1, T2, T3, T4)> ReadFormat<T1, T2, T3, T4>(FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type),
                GetFieldValue<T3>(r, 2, t3.type),
                GetFieldValue<T4>(r, 3, t4.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public IEnumerable<(T1, T2, T3, T4)> ReadFormat<T1, T2, T3, T4>(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback).Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                GetFieldValue<T2>(r, 1, t2.type, readerCallback),
                GetFieldValue<T3>(r, 2, t3.type, readerCallback),
                GetFieldValue<T4>(r, 3, t4.type, readerCallback)));
        }
    }
}