using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Norm
{
    public class NormMultipleReader : IDisposable //INormMultipleReader
    {
        private readonly DbDataReader reader;
        private readonly CancellationToken? cancellationToken;
        private readonly Norm norm;
        private bool disposed = false;

        internal NormMultipleReader(
            DbDataReader reader, 
            CancellationToken? cancellationToken, 
            Norm norm)
        {
            this.reader = reader;
            this.cancellationToken = cancellationToken;
            this.norm = norm;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    reader.Dispose();
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>true if there are more result sets; otherwise, false.</returns>
        public bool Next() 
        { 
            return reader.NextResult();
        }

        /// <summary>
        /// Asynchronously advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>A value task representing the asynchronous operation returning true if there are more result sets; otherwise, false.</returns>
        public async ValueTask<bool> NextAsync()
        {
            if (this.cancellationToken.HasValue)
            {
                return await reader.NextResultAsync(this.cancellationToken.Value);
            }
            return await reader.NextResultAsync();
        }

        ///<summary>
        /// Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read()
        {
            while (reader.Read())
            {
                yield return norm.ReadToArray(reader);
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
                return Read().MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return Read().Map<T>(t1.type);
            }

            return ReadInternal(r => norm.GetFieldValue<T>(r, 0, t1.type));
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
                return Read().MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return Read().Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return Read().Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (t1.simple && t2.simple && t3.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return Read().Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type),
                    norm.GetFieldValue<T4>(r, 3, t4.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type),
                    norm.GetFieldValue<T4>(r, 3, t4.type),
                    norm.GetFieldValue<T5>(r, 4, t5.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type),
                    norm.GetFieldValue<T4>(r, 3, t4.type),
                    norm.GetFieldValue<T5>(r, 4, t5.type),
                    norm.GetFieldValue<T6>(r, 5, t6.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type),
                    norm.GetFieldValue<T4>(r, 3, t4.type),
                    norm.GetFieldValue<T5>(r, 4, t5.type),
                    norm.GetFieldValue<T6>(r, 5, t6.type),
                    norm.GetFieldValue<T7>(r, 6, t7.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type),
                    norm.GetFieldValue<T4>(r, 3, t4.type),
                    norm.GetFieldValue<T5>(r, 4, t5.type),
                    norm.GetFieldValue<T6>(r, 5, t6.type),
                    norm.GetFieldValue<T7>(r, 6, t7.type),
                    norm.GetFieldValue<T8>(r, 7, t8.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type),
                    norm.GetFieldValue<T4>(r, 3, t4.type),
                    norm.GetFieldValue<T5>(r, 4, t5.type),
                    norm.GetFieldValue<T6>(r, 5, t6.type),
                    norm.GetFieldValue<T7>(r, 6, t7.type),
                    norm.GetFieldValue<T8>(r, 7, t8.type),
                    norm.GetFieldValue<T9>(r, 8, t9.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type),
                    norm.GetFieldValue<T4>(r, 3, t4.type),
                    norm.GetFieldValue<T5>(r, 4, t5.type),
                    norm.GetFieldValue<T6>(r, 5, t6.type),
                    norm.GetFieldValue<T7>(r, 6, t7.type),
                    norm.GetFieldValue<T8>(r, 7, t8.type),
                    norm.GetFieldValue<T9>(r, 8, t9.type),
                    norm.GetFieldValue<T10>(r, 9, t10.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
            {
                return ReadInternal(r => (
                   norm.GetFieldValue<T1>(r, 0, t1.type),
                   norm.GetFieldValue<T2>(r, 1, t2.type),
                   norm.GetFieldValue<T3>(r, 2, t3.type),
                   norm.GetFieldValue<T4>(r, 3, t4.type),
                   norm.GetFieldValue<T5>(r, 4, t5.type),
                   norm.GetFieldValue<T6>(r, 5, t6.type),
                   norm.GetFieldValue<T7>(r, 6, t7.type),
                   norm.GetFieldValue<T8>(r, 7, t8.type),
                   norm.GetFieldValue<T9>(r, 8, t9.type),
                   norm.GetFieldValue<T10>(r, 9, t10.type),
                   norm.GetFieldValue<T11>(r, 10, t11.type)));
            }
            throw new NormMultipleMappingsException();
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
                return Read().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple && !t12.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple && t12.simple)
            {
                return ReadInternal(r => (
                    norm.GetFieldValue<T1>(r, 0, t1.type),
                    norm.GetFieldValue<T2>(r, 1, t2.type),
                    norm.GetFieldValue<T3>(r, 2, t3.type),
                    norm.GetFieldValue<T4>(r, 3, t4.type),
                    norm.GetFieldValue<T5>(r, 4, t5.type),
                    norm.GetFieldValue<T6>(r, 5, t6.type),
                    norm.GetFieldValue<T7>(r, 6, t7.type),
                    norm.GetFieldValue<T8>(r, 7, t8.type),
                    norm.GetFieldValue<T9>(r, 8, t9.type),
                    norm.GetFieldValue<T10>(r, 9, t10.type),
                    norm.GetFieldValue<T11>(r, 10, t11.type),
                    norm.GetFieldValue<T12>(r, 11, t12.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of name and value tuple arrays.
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public async IAsyncEnumerable<(string name, object value)[]> ReadAsync()
        {
            while (await reader.ReadAsync())
            {
                yield return norm.ReadToArray(reader);
            }
        }

        /// <summary>
        /// Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAsync<T>()
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadAsync().MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadAsync().Map<T>(t1.type);
            }

            return ReadInternalAsync(async r => await norm.GetFieldValueAsync<T>(r, 0, t1.type));
        }

        /// <summary>
        /// Maps command results to async enumerator of two value tuples (T1, T2).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadAsync().MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadAsync().Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of three value tuples (T1, T2, T3).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return ReadAsync().MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadAsync().Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (t1.simple && t2.simple && t3.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                    await norm.GetFieldValueAsync<T3>(r, 2, t3.type)));
            }
            throw new NormMultipleMappingsException(); ;
        }

        /// <summary>
        /// Maps command results to async enumerator of four value tuples (T1, T2, T3, T4).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple)
            {
                return ReadAsync().MapValueTuple<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                    await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                    await norm.GetFieldValueAsync<T4>(r, 3, t4.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple)
            {
                return ReadAsync().MapValueTuple<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                    await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                    await norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                    await norm.GetFieldValueAsync<T5>(r, 4, t5.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple)
            {
                return ReadAsync().MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                    await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                    await norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                    await norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                    await norm.GetFieldValueAsync<T6>(r, 5, t6.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>()
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
                return ReadAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                    await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                    await norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                    await norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                    await norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                    await norm.GetFieldValueAsync<T7>(r, 6, t7.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>()
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
                return ReadAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple)
            {
                return ReadInternalAsync(async r => (
                   await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                   await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                   await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                   await norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                   await norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                   await norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                   await norm.GetFieldValueAsync<T7>(r, 6, t7.type),
                   await norm.GetFieldValueAsync<T8>(r, 7, t8.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
                return ReadAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple)
            {
                return ReadInternalAsync(async r => (
                   await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                   await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                   await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                   await norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                   await norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                   await norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                   await norm.GetFieldValueAsync<T7>(r, 6, t7.type),
                   await norm.GetFieldValueAsync<T8>(r, 7, t8.type),
                   await norm.GetFieldValueAsync<T9>(r, 8, t9.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
                return ReadAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                    await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                    await norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                    await norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                    await norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                    await norm.GetFieldValueAsync<T7>(r, 6, t7.type),
                    await norm.GetFieldValueAsync<T8>(r, 7, t8.type),
                    await norm.GetFieldValueAsync<T9>(r, 8, t9.type),
                    await norm.GetFieldValueAsync<T10>(r, 9, t10.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
                return ReadAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                    await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                    await norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                    await norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                    await norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                    await norm.GetFieldValueAsync<T7>(r, 6, t7.type),
                    await norm.GetFieldValueAsync<T8>(r, 7, t8.type),
                    await norm.GetFieldValueAsync<T9>(r, 8, t9.type),
                    await norm.GetFieldValueAsync<T10>(r, 9, t10.type),
                    await norm.GetFieldValueAsync<T11>(r, 10, t11.type)));
            }
            throw new NormMultipleMappingsException();
        }

        /// <summary>
        /// Maps command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
                return ReadAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple && !t12.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple && t12.simple)
            {
                return ReadInternalAsync(async r => (
                    await norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                    await norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                    await norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                    await norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                    await norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                    await norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                    await norm.GetFieldValueAsync<T7>(r, 6, t7.type),
                    await norm.GetFieldValueAsync<T8>(r, 7, t8.type),
                    await norm.GetFieldValueAsync<T9>(r, 8, t9.type),
                    await norm.GetFieldValueAsync<T10>(r, 9, t10.type),
                    await norm.GetFieldValueAsync<T11>(r, 10, t11.type),
                    await norm.GetFieldValueAsync<T12>(r, 11, t12.type)));
            }
            throw new NormMultipleMappingsException();
        }

        private IEnumerable<T> ReadInternal<T>(Func<DbDataReader, T> readerAction)
        {
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        private async IAsyncEnumerable<T> ReadInternalAsync<T>(Func<DbDataReader, Task<T>> readerAction)
        {
            if (cancellationToken.HasValue)
            {
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return await readerAction(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                while (await reader.ReadAsync())
                {
                    yield return await readerAction(reader);
                }
            }
        }
    }
}
