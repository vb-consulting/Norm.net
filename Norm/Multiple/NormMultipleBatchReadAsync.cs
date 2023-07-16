using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Norm.Mapper;

namespace Norm
{
    public partial class NormMultipleBatch
    {
        /// <summary>
        /// Maps command results to async enumerator of name and value tuple arrays.
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public async IAsyncEnumerable<(string name, object value)[]> ReadAsync()
        {
            norm.ResetNames();
            if (norm.ReaderCallback == null)
            {
                while (await dbReader.ReadAsync())
                {
                    yield return norm.ReadToArray(dbReader).ToArray();
                }
            }
            else
            {
                while (await dbReader.ReadAsync())
                {
                    yield return norm.ReadToArray(dbReader, norm.ReaderCallback).ToArray();
                }
            }
        }

        public async IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> ReadReadOnlyMemoryAsync()
        {
            norm.ResetNames();
            if (norm.ReaderCallback == null)
            {
                while (await dbReader.ReadAsync())
                {
                    yield return norm.ReadToArray(dbReader);
                }
            }
            else
            {
                while (await dbReader.ReadAsync())
                {
                    yield return norm.ReadToArray(dbReader, norm.ReaderCallback);
                }
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T>(t1.type);
            }

            return ReadInternalAsync(async r => await Norm.GetFieldValueAsync<T>(r, 0, t1.type));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2>(t1.type, t2.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                await Norm.GetFieldValueAsync<T3>(r, 2, t3.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                await Norm.GetFieldValueAsync<T4>(r, 3, t4.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                await Norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                await Norm.GetFieldValueAsync<T5>(r, 4, t5.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                await Norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                await Norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                await Norm.GetFieldValueAsync<T6>(r, 5, t6.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                await Norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                await Norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                await Norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                await Norm.GetFieldValueAsync<T7>(r, 6, t7.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            return ReadInternalAsync(async r => (
               await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
               await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
               await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
               await Norm.GetFieldValueAsync<T4>(r, 3, t4.type),
               await Norm.GetFieldValueAsync<T5>(r, 4, t5.type),
               await Norm.GetFieldValueAsync<T6>(r, 5, t6.type),
               await Norm.GetFieldValueAsync<T7>(r, 6, t7.type),
               await Norm.GetFieldValueAsync<T8>(r, 7, t8.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            return ReadInternalAsync(async r => (
               await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
               await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
               await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
               await Norm.GetFieldValueAsync<T4>(r, 3, t4.type),
               await Norm.GetFieldValueAsync<T5>(r, 4, t5.type),
               await Norm.GetFieldValueAsync<T6>(r, 5, t6.type),
               await Norm.GetFieldValueAsync<T7>(r, 6, t7.type),
               await Norm.GetFieldValueAsync<T8>(r, 7, t8.type),
               await Norm.GetFieldValueAsync<T9>(r, 8, t9.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                await Norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                await Norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                await Norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                await Norm.GetFieldValueAsync<T7>(r, 6, t7.type),
                await Norm.GetFieldValueAsync<T8>(r, 7, t8.type),
                await Norm.GetFieldValueAsync<T9>(r, 8, t9.type),
                await Norm.GetFieldValueAsync<T10>(r, 9, t10.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                await Norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                await Norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                await Norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                await Norm.GetFieldValueAsync<T7>(r, 6, t7.type),
                await Norm.GetFieldValueAsync<T8>(r, 7, t8.type),
                await Norm.GetFieldValueAsync<T9>(r, 8, t9.type),
                await Norm.GetFieldValueAsync<T10>(r, 9, t10.type),
                await Norm.GetFieldValueAsync<T11>(r, 10, t11.type)));
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
                return ReadReadOnlyMemoryAsync().MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple && !t12.simple)
            {
                return ReadReadOnlyMemoryAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            return ReadInternalAsync(async r => (
                await Norm.GetFieldValueAsync<T1>(r, 0, t1.type),
                await Norm.GetFieldValueAsync<T2>(r, 1, t2.type),
                await Norm.GetFieldValueAsync<T3>(r, 2, t3.type),
                await Norm.GetFieldValueAsync<T4>(r, 3, t4.type),
                await Norm.GetFieldValueAsync<T5>(r, 4, t5.type),
                await Norm.GetFieldValueAsync<T6>(r, 5, t6.type),
                await Norm.GetFieldValueAsync<T7>(r, 6, t7.type),
                await Norm.GetFieldValueAsync<T8>(r, 7, t8.type),
                await Norm.GetFieldValueAsync<T9>(r, 8, t9.type),
                await Norm.GetFieldValueAsync<T10>(r, 9, t10.type),
                await Norm.GetFieldValueAsync<T11>(r, 10, t11.type),
                await Norm.GetFieldValueAsync<T12>(r, 11, t12.type)));
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="bluePrintInstance">Instance used as blueprint to create new instances of same instance types</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAsync<T>(T bluePrintInstance)
            where T : class
        {
            return ReadReadOnlyMemoryAsync().MapInstance<T>(bluePrintInstance.GetType());
        }

        private async IAsyncEnumerable<T> ReadInternalAsync<T>(Func<DbDataReader, Task<T>> readerAction)
        {
            if (norm.CancellationToken.HasValue)
            {
                while (await dbReader.ReadAsync(norm.CancellationToken.Value))
                {
                    yield return await readerAction(dbReader);
                    norm.CancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                while (await dbReader.ReadAsync())
                {
                    yield return await readerAction(dbReader);
                }
            }
        }
    }
}
