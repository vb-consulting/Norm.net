using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm
    {
        private async ValueTask<T> SingleInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await PrepareAsync(cmd);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                return await readerAction(reader);
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                return await readerAction(reader);
            }
        }

        private async ValueTask<T> SingleInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, params object[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                return await readerAction(reader);
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                return await readerAction(reader);
            }
        }

        private async ValueTask<T> SingleInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, params (string name, object value)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                return await readerAction(reader);
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                return await readerAction(reader);
            }
        }

        private async ValueTask<T> SingleInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, params (string name, object value, DbType type)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                return await readerAction(reader);
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                return await readerAction(reader);
            }
        }

        private async ValueTask<T> SingleInternalUnknowParamsTypeAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, params (string name, object value, object type)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersUnknownTypeAsync(cmd, parameters);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                return await readerAction(reader);
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                return await readerAction(reader);
            }
        }







        private async ValueTask<(string name, object value)[]> SingleToArrayInternalAsync(string command)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await PrepareAsync(cmd);
            if (cancellationToken.HasValue)
            {
                var ct = cancellationToken.Value;
                await using var reader = await cmd.ExecuteReaderAsync(ct);
                if (await reader.ReadAsync(ct))
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
        }

        private async ValueTask<(string name, object value)[]> SingleToArrayInternalAsync(string command, params object[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            if (cancellationToken.HasValue)
            {
                var ct = cancellationToken.Value;
                await using var reader = await cmd.ExecuteReaderAsync(ct);
                if (await reader.ReadAsync(ct))
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
        }

        private async ValueTask<(string name, object value)[]> SingleToArrayInternalAsync(string command, params (string name, object value)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            if (cancellationToken.HasValue)
            {
                var ct = cancellationToken.Value;
                await using var reader = await cmd.ExecuteReaderAsync(ct);
                if (await reader.ReadAsync(ct))
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
        }

        private async ValueTask<(string name, object value)[]> SingleToArrayInternalAsync(string command, params (string name, object value, DbType type)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            if (cancellationToken.HasValue)
            {
                var ct = cancellationToken.Value;
                await using var reader = await cmd.ExecuteReaderAsync(ct);
                if (await reader.ReadAsync(ct))
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
        }

        private async ValueTask<(string name, object value)[]> SingleToArrayInternalUnknowParamsTypeAsync(string command, params (string name, object value, object type)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersUnknownTypeAsync(cmd, parameters);
            if (cancellationToken.HasValue)
            {
                var ct = cancellationToken.Value;
                await using var reader = await cmd.ExecuteReaderAsync(ct);
                if (await reader.ReadAsync(ct))
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return reader.ToArray();
                }
                return Array.Empty<(string name, object value)>();
            }
        }

    }
}