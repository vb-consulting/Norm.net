using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternal(command, r => (
               GetFieldValue<T1>(r, 0, t1.type),
               GetFieldValue<T2>(r, 1, t2.type),
               GetFieldValue<T3>(r, 2, t3.type),
               GetFieldValue<T4>(r, 3, t4.type),
               GetFieldValue<T5>(r, 4, t5.type),
               GetFieldValue<T6>(r, 5, t6.type),
               GetFieldValue<T7>(r, 6, t7.type),
               GetFieldValue<T8>(r, 7, t8.type),
               GetFieldValue<T9>(r, 8, t9.type),
               GetFieldValue<T10>(r, 9, t10.type),
               GetFieldValue<T11>(r, 10, t11.type)));
        }

        ///<summary>
        ///     Maps command results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternal(command, r => (
               GetFieldValue<T1>(r, 0, t1.type, readerCallback),
               GetFieldValue<T2>(r, 1, t2.type, readerCallback),
               GetFieldValue<T3>(r, 2, t3.type, readerCallback),
               GetFieldValue<T4>(r, 3, t4.type, readerCallback),
               GetFieldValue<T5>(r, 4, t5.type, readerCallback),
               GetFieldValue<T6>(r, 5, t6.type, readerCallback),
               GetFieldValue<T7>(r, 6, t7.type, readerCallback),
               GetFieldValue<T8>(r, 7, t8.type, readerCallback),
               GetFieldValue<T9>(r, 8, t9.type, readerCallback),
               GetFieldValue<T10>(r, 9, t10.type, readerCallback),
               GetFieldValue<T11>(r, 10, t11.type, readerCallback)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of ten eleven tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
                return ReadToArrayInternal(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternal(command, r => (
               GetFieldValue<T1>(r, 0, t1.type),
               GetFieldValue<T2>(r, 1, t2.type),
               GetFieldValue<T3>(r, 2, t3.type),
               GetFieldValue<T4>(r, 3, t4.type),
               GetFieldValue<T5>(r, 4, t5.type),
               GetFieldValue<T6>(r, 5, t6.type),
               GetFieldValue<T7>(r, 6, t7.type),
               GetFieldValue<T8>(r, 7, t8.type),
               GetFieldValue<T9>(r, 8, t9.type),
               GetFieldValue<T10>(r, 9, t10.type),
               GetFieldValue<T11>(r, 10, t11.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of ten eleven tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternal(command, r => (
               GetFieldValue<T1>(r, 0, t1.type, readerCallback),
               GetFieldValue<T2>(r, 1, t2.type, readerCallback),
               GetFieldValue<T3>(r, 2, t3.type, readerCallback),
               GetFieldValue<T4>(r, 3, t4.type, readerCallback),
               GetFieldValue<T5>(r, 4, t5.type, readerCallback),
               GetFieldValue<T6>(r, 5, t6.type, readerCallback),
               GetFieldValue<T7>(r, 6, t7.type, readerCallback),
               GetFieldValue<T8>(r, 7, t8.type, readerCallback),
               GetFieldValue<T9>(r, 8, t9.type, readerCallback),
               GetFieldValue<T10>(r, 9, t10.type, readerCallback),
               GetFieldValue<T11>(r, 10, t11.type, readerCallback)));
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params object[] parameters)
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
                return ReadToArrayInternal(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayInternal(command, parameters).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type),
                GetFieldValue<T3>(r, 2, t3.type),
                GetFieldValue<T4>(r, 3, t4.type),
                GetFieldValue<T5>(r, 4, t5.type),
                GetFieldValue<T6>(r, 5, t6.type),
                GetFieldValue<T7>(r, 6, t7.type),
                GetFieldValue<T8>(r, 7, t8.type),
                GetFieldValue<T9>(r, 8, t9.type),
                GetFieldValue<T10>(r, 9, t10.type),
                GetFieldValue<T11>(r, 10, t11.type)), parameters);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters)
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
                return ReadToArrayInternal(command, readerCallback, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback, parameters).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                GetFieldValue<T2>(r, 1, t2.type, readerCallback),
                GetFieldValue<T3>(r, 2, t3.type, readerCallback),
                GetFieldValue<T4>(r, 3, t4.type, readerCallback),
                GetFieldValue<T5>(r, 4, t5.type, readerCallback),
                GetFieldValue<T6>(r, 5, t6.type, readerCallback),
                GetFieldValue<T7>(r, 6, t7.type, readerCallback),
                GetFieldValue<T8>(r, 7, t8.type, readerCallback),
                GetFieldValue<T9>(r, 8, t9.type, readerCallback),
                GetFieldValue<T10>(r, 9, t10.type, readerCallback),
                GetFieldValue<T11>(r, 10, t11.type, readerCallback)), parameters);
        }
    }
}