using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm.Interfaces
{
    public interface INormMultipleReader : IDisposable
    {
        /// <summary>
        /// Advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>true if there are more result sets; otherwise, false.</returns>
        bool Next();
        /// <summary>
        /// Asynchronously advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>A value task representing the asynchronous operation returning true if there are more result sets; otherwise, false.</returns>
        ValueTask<bool> NextAsync();
        ///<summary>
        /// Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        IEnumerable<(string name, object value)[]> Read();
        /// <summary>
        /// Maps command results to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IEnumerable enumerator of single values of type T.</returns>
        IEnumerable<T> Read<T>();
        /// <summary>
        /// Maps command results to enumerator of two value tuples (T1, T2).
        /// </summary>
        /// <returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        IEnumerable<(T1, T2)> Read<T1, T2>();
        /// <summary>
        /// Maps command results to enumerator of three value tuples (T1, T2, T3).
        /// </summary>
        /// <returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>();
        /// <summary>
        /// Maps command results to enumerator of four value tuples (T1, T2, T3, T4).
        /// </summary>
        /// <returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>();
        /// <summary>
        /// Maps command results to enumerator of five value tuples (T1, T2, T3, T4, T5).
        /// </summary>
        /// <returns>IEnumerable enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>();
        /// <summary>
        /// Maps command results to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        /// </summary>
        /// <returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>();
        /// <summary>
        /// Maps command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        /// </summary>
        /// <returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>();
        /// <summary>
        /// Maps command results to enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        /// </summary>
        /// <returns>IEnumerable enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>();
        /// <summary>
        /// Maps command results to enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        /// </summary>
        /// <returns>IEnumerable enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
        /// <summary>
        /// Maps command results to enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        /// </summary>
        /// <returns>IEnumerable enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T99, T10).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
        /// <summary>
        /// Maps command results to enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        /// </summary>
        /// <returns>IEnumerable enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
        /// <summary>
        /// Maps command results to enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        /// </summary>
        /// <returns>IEnumerable enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
        /// <summary>
        /// Maps command results to async enumerator of name and value tuple arrays.
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        IAsyncEnumerable<(string name, object value)[]> ReadAsync();
        /// <summary>
        /// Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        IAsyncEnumerable<T> ReadAsync<T>();
        /// <summary>
        /// Maps command results to async enumerator of two value tuples (T1, T2).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>();
        /// <summary>
        /// Maps command results to async enumerator of three value tuples (T1, T2, T3).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of three value tuples (T1, T2, T3).</returns>
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>();
        /// <summary>
        /// Maps command results to async enumerator of four value tuples (T1, T2, T3, T4).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of four value tuples (T1, T2, T3, T4).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>();
        /// <summary>
        /// Maps command results to async enumerator of five value tuples (T1, T2, T3, T4, T5).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of five value tuples (T1, T2, T3, T4, T5).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>();
        /// <summary>
        /// Maps command results to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>();
        /// <summary>
        /// Maps command results to async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>();
        /// <summary>
        /// Maps command results to async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of eight value tuples (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>();
        /// <summary>
        /// Maps command results to async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of nine value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
        /// <summary>
        /// Maps command results to async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of ten value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
        /// <summary>
        /// Maps command results to async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of eleven value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
        /// <summary>
        /// Maps command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        /// </summary>
        /// <returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
    }
}
