using System;
using System.Collections.Generic;
using System.Data.Common;
using Norm.Mapper;

namespace Norm
{
    public partial class NormMultipleBatch
    {
        ///<summary>
        /// Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read()
        {
            if (norm.ReaderCallback == null)
            {
                while (dbReader.Read())
                {
                    yield return Norm.ReadToArray(dbReader).ToArray();
                }
            }
            else
            {
                while (dbReader.Read())
                {
                    yield return Norm.ReadToArray(dbReader, norm.ReaderCallback).ToArray();
                }
            }
        }

        private IEnumerable<ReadOnlyMemory<(string name, object value)>> ReadReadOnlyMemory()
        {
            if (norm.ReaderCallback == null)
            {
                while (dbReader.Read())
                {
                    yield return Norm.ReadToArray(dbReader);
                }
            }
            else
            {
                while (dbReader.Read())
                {
                    yield return Norm.ReadToArray(dbReader, norm.ReaderCallback);
                }
            }
        }

        /// <summary>
        /// Maps command results to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> Read<T>()
        {
            var t1 = TypeCache<T>.GetMetadata();

            if (t1.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadReadOnlyMemory().Map<T>(t1.type);
            }

            return ReadInternal(r => Norm.GetFieldValue<T>(r, 0, t1.type));
        }

        /// <summary>
        /// Maps command results to enumerator of two value tuples (T1, T2).
        /// </summary>
        /// <returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2>(t1.type, t2.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of three value tuples (T1, T2, T3).
        /// </summary>
        /// <returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of four value tuples (T1, T2, T3, T4).
        /// </summary>
        /// <returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type),
                Norm.GetFieldValue<T4>(r, 3, t4.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of five value tuples (T1, T2, T3, T4, T5).
        /// </summary>
        /// <returns>IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type),
                Norm.GetFieldValue<T4>(r, 3, t4.type),
                Norm.GetFieldValue<T5>(r, 4, t5.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        /// </summary>
        /// <returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type),
                Norm.GetFieldValue<T4>(r, 3, t4.type),
                Norm.GetFieldValue<T5>(r, 4, t5.type),
                Norm.GetFieldValue<T6>(r, 5, t6.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        /// </summary>
        /// <returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type),
                Norm.GetFieldValue<T4>(r, 3, t4.type),
                Norm.GetFieldValue<T5>(r, 4, t5.type),
                Norm.GetFieldValue<T6>(r, 5, t6.type),
                Norm.GetFieldValue<T7>(r, 6, t7.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        /// </summary>
        /// <returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>()
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
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type),
                Norm.GetFieldValue<T4>(r, 3, t4.type),
                Norm.GetFieldValue<T5>(r, 4, t5.type),
                Norm.GetFieldValue<T6>(r, 5, t6.type),
                Norm.GetFieldValue<T7>(r, 6, t7.type),
                Norm.GetFieldValue<T8>(r, 7, t8.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        /// </summary>
        /// <returns>IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type),
                Norm.GetFieldValue<T4>(r, 3, t4.type),
                Norm.GetFieldValue<T5>(r, 4, t5.type),
                Norm.GetFieldValue<T6>(r, 5, t6.type),
                Norm.GetFieldValue<T7>(r, 6, t7.type),
                Norm.GetFieldValue<T8>(r, 7, t8.type),
                Norm.GetFieldValue<T9>(r, 8, t9.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        /// </summary>
        /// <returns>IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T99, T10).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type),
                Norm.GetFieldValue<T4>(r, 3, t4.type),
                Norm.GetFieldValue<T5>(r, 4, t5.type),
                Norm.GetFieldValue<T6>(r, 5, t6.type),
                Norm.GetFieldValue<T7>(r, 6, t7.type),
                Norm.GetFieldValue<T8>(r, 7, t8.type),
                Norm.GetFieldValue<T9>(r, 8, t9.type),
                Norm.GetFieldValue<T10>(r, 9, t10.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        /// </summary>
        /// <returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternal(r => (
               Norm.GetFieldValue<T1>(r, 0, t1.type),
               Norm.GetFieldValue<T2>(r, 1, t2.type),
               Norm.GetFieldValue<T3>(r, 2, t3.type),
               Norm.GetFieldValue<T4>(r, 3, t4.type),
               Norm.GetFieldValue<T5>(r, 4, t5.type),
               Norm.GetFieldValue<T6>(r, 5, t6.type),
               Norm.GetFieldValue<T7>(r, 6, t7.type),
               Norm.GetFieldValue<T8>(r, 7, t8.type),
               Norm.GetFieldValue<T9>(r, 8, t9.type),
               Norm.GetFieldValue<T10>(r, 9, t10.type),
               Norm.GetFieldValue<T11>(r, 10, t11.type)));
        }

        /// <summary>
        /// Maps command results to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        /// </summary>
        /// <returns>IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
            var t12 = TypeCache<T12>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple && t12.valueTuple)
            {
                return ReadReadOnlyMemory().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple && !t12.simple)
            {
                return ReadReadOnlyMemory().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            return ReadInternal(r => (
                Norm.GetFieldValue<T1>(r, 0, t1.type),
                Norm.GetFieldValue<T2>(r, 1, t2.type),
                Norm.GetFieldValue<T3>(r, 2, t3.type),
                Norm.GetFieldValue<T4>(r, 3, t4.type),
                Norm.GetFieldValue<T5>(r, 4, t5.type),
                Norm.GetFieldValue<T6>(r, 5, t6.type),
                Norm.GetFieldValue<T7>(r, 6, t7.type),
                Norm.GetFieldValue<T8>(r, 7, t8.type),
                Norm.GetFieldValue<T9>(r, 8, t9.type),
                Norm.GetFieldValue<T10>(r, 9, t10.type),
                Norm.GetFieldValue<T11>(r, 10, t11.type),
                Norm.GetFieldValue<T12>(r, 11, t12.type)));
        }

        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> Read<T>(T anonymousBlueprintInstance)
            where T : class
        {
            return ReadReadOnlyMemory().MapAnonymous<T>(anonymousBlueprintInstance.GetType());
        }

        private IEnumerable<T> ReadInternal<T>(Func<DbDataReader, T> readerAction)
        {
            while (dbReader.Read())
            {
                yield return readerAction(dbReader);
            }
        }
    }
}
