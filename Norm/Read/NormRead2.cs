using System;
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
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2>(t1.type, t2.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type)));
        }

        ///<summary>
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback).Map<T1, T2>(t1.type, t2.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                GetFieldValue<T2>(r, 1, t2.type, readerCallback)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> ReadFormat<T1, T2>(FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2>(t1.type, t2.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> ReadFormat<T1, T2>(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback).Map<T1, T2>(t1.type, t2.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                GetFieldValue<T2>(r, 1, t2.type, readerCallback)));
        }
    }
}