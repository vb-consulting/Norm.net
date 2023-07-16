using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        internal IEnumerable<T> ReadCallback<T>(string command, Func<DbDataReader, T> readerAction)
        {
            using var cmd = CreateCommand(command);
            using var reader = this.ExecuteReader(cmd);
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        internal IEnumerable<T> ReadCallback<T>(FormattableString command, Func<DbDataReader, T> readerAction)
        {
            using var cmd = CreateCommand(command);
            using var reader = this.ExecuteReader(cmd);
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        internal async IAsyncEnumerable<T> ReadCallbackAsync<T>(string command,
            Func<DbDataReader, Task<T>> readerAction)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await this.ExecuteReaderAsync(cmd);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return await readerAction(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await this.ExecuteReaderAsync(cmd);
                while (await reader.ReadAsync())
                {
                    yield return await readerAction(reader);
                }
            }
        }

        internal async IAsyncEnumerable<T> ReadCallbackAsync<T>(FormattableString command,
            Func<DbDataReader, Task<T>> readerAction)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await this.ExecuteReaderAsync(cmd);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return await readerAction(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await this.ExecuteReaderAsync(cmd);
                while (await reader.ReadAsync())
                {
                    yield return await readerAction(reader);
                }
            }
        }

    }
}