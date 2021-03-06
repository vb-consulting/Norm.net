﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        private async IAsyncEnumerable<T> ReadInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return await readerAction(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return await readerAction(reader);
                }
            }
        }
        
        private async IAsyncEnumerable<T> ReadInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, params object[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return await readerAction(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return await readerAction(reader);
                }
            }
        }

        private async IAsyncEnumerable<T> ReadInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, params (string name, object value)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return await readerAction(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return await readerAction(reader);
                }
            }
        }

        private async IAsyncEnumerable<T> ReadInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return await readerAction(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return await readerAction(reader);
                }
            }
        }

        private async IAsyncEnumerable<T> ReadInternalUnknownParamsTypeAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, params (string name, object value, object type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return await readerAction(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return await readerAction(reader);
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await  cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return reader.ToArray();
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await  cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return reader.ToArray();
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(string command, params object[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return reader.ToArray();
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return reader.ToArray();
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(string command, params (string name, object value)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return reader.ToArray();
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return reader.ToArray();
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return reader.ToArray();
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return reader.ToArray();
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalUnknownParamsTypeAsync(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return reader.ToArray();
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return reader.ToArray();
                }
            }
        }
    }
}