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
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return Read().Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return Read().Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (t1.simple && t2.simple && t3.simple)
            {
                return ReadInternal(r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull),
                    r.GetFieldValue<T3>(2, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple)
            {
                return Read().Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return Read().Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple)
            {
                return ReadInternal(r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull),
                    r.GetFieldValue<T3>(2, convertsDbNull),
                    r.GetFieldValue<T4>(3, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple)
            {
                return Read().Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple)
            {
                return ReadInternal(r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull),
                    r.GetFieldValue<T3>(2, convertsDbNull),
                    r.GetFieldValue<T4>(3, convertsDbNull),
                    r.GetFieldValue<T5>(4, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

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
                return Read().Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternal(r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull),
                    r.GetFieldValue<T3>(2, convertsDbNull),
                    r.GetFieldValue<T4>(3, convertsDbNull),
                    r.GetFieldValue<T5>(4, convertsDbNull),
                    r.GetFieldValue<T6>(5, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

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
                return Read().Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple)
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
            throw new NormMultipleMappingsException();
        }

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
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple)
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
            throw new NormMultipleMappingsException();
        }

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
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple)
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
            throw new NormMultipleMappingsException();
        }

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
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple)
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
            throw new NormMultipleMappingsException();
        }

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
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
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
            throw new NormMultipleMappingsException();
        }

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
            var t12 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple && t12.valueTuple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple && !t12.simple)
            {
                return Read().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple && t12.simple)
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
            throw new NormMultipleMappingsException();
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
                    await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                    await r.GetFieldValueAsync<T2>(1, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return ReadAsync().Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadAsync().Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (t1.simple && t2.simple && t3.simple)
            {
                return ReadInternalAsync(async r => (
                    await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                    await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                    await r.GetFieldValueAsync<T3>(2, convertsDbNull)));
            }
            throw new NormMultipleMappingsException(); ;
        }

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple)
            {
                return ReadAsync().Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4>(t1.type, t2.type, t3.type, t4.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple)
            {
                return ReadInternalAsync(async r => (
                    await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                    await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                    await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                    await r.GetFieldValueAsync<T4>(3, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>()
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5>(t1.type, t2.type, t3.type, t4.type, t5.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple)
            {
                return ReadInternalAsync(async r => (
                    await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                    await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                    await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                    await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                    await r.GetFieldValueAsync<T5>(4, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

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
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternalAsync(async r => (
                    await r.GetFieldValueAsync<T1>(0, convertsDbNull),
                    await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                    await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                    await r.GetFieldValueAsync<T4>(3, convertsDbNull),
                    await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                    await r.GetFieldValueAsync<T6>(5, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

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
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple)
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
            throw new NormMultipleMappingsException();
        }

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
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple)
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
            throw new NormMultipleMappingsException();
        }

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
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple)
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
            throw new NormMultipleMappingsException();
        }

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
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple)
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
            throw new NormMultipleMappingsException();
        }

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
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
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
            throw new NormMultipleMappingsException();
        }

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
            var t12 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple && t12.valueTuple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple && !t12.simple)
            {
                return ReadAsync().Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type, t12.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple && t12.simple)
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
