using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Norm
{
    public partial class Norm
    {
        protected IEnumerable<(string name, object value)[]> ReadToArrayInternal(string command)
        {
            using var cmd = CreateCommand(command);
            using var reader = this.ExecuteReader(cmd);

            if (this.readerCallback == null)
            {
                while (reader.Read())
                {
                    yield return ReadToArray(reader);
                }
            }
            else
            {
                while (reader.Read())
                {
                    yield return ReadToArray(reader, this.readerCallback);
                }
            }
        }
     
        protected IEnumerable<(string name, object value, bool set)[]> ReadToArrayWithWithSetInternal(string command)
        {
            using var cmd = CreateCommand(command);
            using var reader = this.ExecuteReader(cmd);
            while (reader.Read())
            {
                yield return ReadToArrayWithSet(reader);
            }
        }
        
        protected IEnumerable<(string name, object value)[]> ReadToArrayInternal(FormattableString command)
        {
            using var cmd = CreateCommand(command);
            using var reader = this.ExecuteReader(cmd);
            if (this.readerCallback == null)
            {
                while (reader.Read())
                {
                    yield return ReadToArray(reader);
                }
            }
            else
            {
                while (reader.Read())
                {
                    yield return ReadToArray(reader, readerCallback);
                }
            }
        }
        
        protected IEnumerable<(string name, object value, bool set)[]> ReadToArrayWithSetInternal(FormattableString command)
        {
            using var cmd = CreateCommand(command);
            using var reader = this.ExecuteReader(cmd);
            while (reader.Read())
            {
                yield return ReadToArrayWithSet(reader);
            }
        }

        protected async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (this.readerCallback == null)
            {
                if (cancellationToken.HasValue)
                {
                    await using var reader = await this.ExecuteReaderAsync(cmd);
                    while (await reader.ReadAsync(cancellationToken.Value))
                    {
                        yield return ReadToArray(reader);
                        cancellationToken?.ThrowIfCancellationRequested();
                    }
                }
                else
                {
                    await using var reader = await this.ExecuteReaderAsync(cmd);
                    while (await reader.ReadAsync())
                    {
                        yield return ReadToArray(reader);
                    }
                }
            }
            else
            {
                if (cancellationToken.HasValue)
                {
                    await using var reader = await this.ExecuteReaderAsync(cmd);
                    while (await reader.ReadAsync(cancellationToken.Value))
                    {
                        yield return ReadToArray(reader, this.readerCallback);
                        cancellationToken?.ThrowIfCancellationRequested();
                    }
                }
                else
                {
                    await using var reader = await this.ExecuteReaderAsync(cmd);
                    while (await reader.ReadAsync())
                    {
                        yield return ReadToArray(reader, this.readerCallback);
                    }
                }
            }
        }

        protected async IAsyncEnumerable<(string name, object value, bool set)[]> ReadToArrayWithSetInternalAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await this.ExecuteReaderAsync(cmd);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArrayWithSet(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await this.ExecuteReaderAsync(cmd);
                while (await reader.ReadAsync())
                {
                    yield return ReadToArrayWithSet(reader);
                }
            }
        }

        protected async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);

            if (this.readerCallback == null)
            {
                if (cancellationToken.HasValue)
                {
                    await using var reader = await this.ExecuteReaderAsync(cmd);
                    while (await reader.ReadAsync(cancellationToken.Value))
                    {
                        yield return ReadToArray(reader);
                        cancellationToken?.ThrowIfCancellationRequested();
                    }
                }
                else
                {
                    await using var reader = await this.ExecuteReaderAsync(cmd);
                    while (await reader.ReadAsync())
                    {
                        yield return ReadToArray(reader);
                    }
                }
            }
            else
            {
                if (cancellationToken.HasValue)
                {
                    await using var reader = await this.ExecuteReaderAsync(cmd);
                    while (await reader.ReadAsync(cancellationToken.Value))
                    {
                        yield return ReadToArray(reader, readerCallback);
                        cancellationToken?.ThrowIfCancellationRequested();
                    }
                }
                else
                {
                    await using var reader = await this.ExecuteReaderAsync(cmd);
                    while (await reader.ReadAsync())
                    {
                        yield return ReadToArray(reader, readerCallback);
                    }
                }
            }
        }

        protected async IAsyncEnumerable<(string name, object value, bool set)[]> ReadToArrayWithSetInternalAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await this.ExecuteReaderAsync(cmd);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArrayWithSet(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await this.ExecuteReaderAsync(cmd);
                while (await reader.ReadAsync())
                {
                    yield return ReadToArrayWithSet(reader);
                }
            }
        }

        protected (string name, object value, bool set)[] ReadToArrayWithSet(DbDataReader reader)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            (string name, object value, bool set)[] result = new (string name, object value, bool set)[count];
            for (var index = 0; index < count; index++)
            {
                n = reader.GetName(index);
                var callback = readerCallback((n, index, reader));
                if (callback != null)
                {
                    result[index] = (n, callback == DBNull.Value ? null : callback, true);
                    continue;
                }
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r, false);
            }
            return result;
        }
    }
}