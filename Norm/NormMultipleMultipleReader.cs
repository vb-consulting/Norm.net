using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Norm.Interfaces;

namespace Norm
{
    public partial class Norm
    {
        public INormMultipleReader Multiple(string command)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader Multiple(string command, params object[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command, params object[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader Multiple(string command, params (string name, object value)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader Multiple(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader Multiple(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }
    }

    public class NormMultipleMultipleReader : INormMultipleReader
    {
        private readonly DbDataReader reader;
        private readonly CancellationToken? cancellationToken;
        private readonly bool convertsDbNull;
        private bool disposed = false;

        internal NormMultipleMultipleReader(DbDataReader reader, CancellationToken? cancellationToken, bool convertsDbNull)
        {
            this.reader = reader;
            this.cancellationToken = cancellationToken;
            this.convertsDbNull = convertsDbNull;
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

        public bool Next() 
        { 
            return reader.NextResult();
        }

        public async ValueTask<bool> NextAsync()
        {
            if (this.cancellationToken.HasValue)
            {
                return await reader.NextResultAsync(this.cancellationToken.Value);
            }
            return await reader.NextResultAsync();
        }

        public IEnumerable<(string name, object value)[]> Read()
        {
            while (reader.Read())
            {
                yield return reader.ToArray();
            }
        }

        public IEnumerable<T> Read<T>()
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return Read().MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return Read().Map<T>(type);
            }

            return ReadInternal(r => r.GetFieldValue<T>(0, convertsDbNull));
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull),
                r.GetFieldValue<T7>(6, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull),
                r.GetFieldValue<T7>(6, convertsDbNull),
                r.GetFieldValue<T8>(7, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull),
                r.GetFieldValue<T7>(6, convertsDbNull),
                r.GetFieldValue<T8>(7, convertsDbNull),
                r.GetFieldValue<T9>(8, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        {
            return ReadInternal(r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull),
                r.GetFieldValue<T7>(6, convertsDbNull),
                r.GetFieldValue<T8>(7, convertsDbNull),
                r.GetFieldValue<T9>(8, convertsDbNull),
                r.GetFieldValue<T10>(9, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
        {
            return ReadInternal(r => (
                 r.GetFieldValue<T1>(0, convertsDbNull),
                 r.GetFieldValue<T2>(1, convertsDbNull),
                 r.GetFieldValue<T3>(2, convertsDbNull),
                 r.GetFieldValue<T4>(3, convertsDbNull),
                 r.GetFieldValue<T5>(4, convertsDbNull),
                 r.GetFieldValue<T6>(5, convertsDbNull),
                 r.GetFieldValue<T7>(6, convertsDbNull),
                 r.GetFieldValue<T8>(7, convertsDbNull),
                 r.GetFieldValue<T9>(8, convertsDbNull),
                 r.GetFieldValue<T10>(9, convertsDbNull),
                 r.GetFieldValue<T11>(10, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
        {
            return ReadInternal(r => (
                 r.GetFieldValue<T1>(0, convertsDbNull),
                 r.GetFieldValue<T2>(1, convertsDbNull),
                 r.GetFieldValue<T3>(2, convertsDbNull),
                 r.GetFieldValue<T4>(3, convertsDbNull),
                 r.GetFieldValue<T5>(4, convertsDbNull),
                 r.GetFieldValue<T6>(5, convertsDbNull),
                 r.GetFieldValue<T7>(6, convertsDbNull),
                 r.GetFieldValue<T8>(7, convertsDbNull),
                 r.GetFieldValue<T9>(8, convertsDbNull),
                 r.GetFieldValue<T10>(9, convertsDbNull),
                 r.GetFieldValue<T11>(10, convertsDbNull),
                 r.GetFieldValue<T12>(11, convertsDbNull)));
        }

        public async IAsyncEnumerable<(string name, object value)[]> ReadAsync()
        {
            while (await reader.ReadAsync())
            {
                yield return reader.ToArray();
            }
        }

        public IAsyncEnumerable<T> ReadAsync<T>()
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return ReadAsync().MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return ReadAsync().Map<T>(type);
            }

            return ReadInternalAsync(async r => await r.GetFieldValueAsync<T>(0, convertsDbNull));
        }

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                await r.GetFieldValueAsync<T5>(4, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull),
                await r.GetFieldValueAsync<T8>(7, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull),
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull),
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull),
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull),
                await r.GetFieldValueAsync<T11>(10, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
        {
            return ReadInternalAsync(async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull),
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull),
                await r.GetFieldValueAsync<T11>(10, convertsDbNull),
                await r.GetFieldValueAsync<T12>(11, convertsDbNull)));
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
