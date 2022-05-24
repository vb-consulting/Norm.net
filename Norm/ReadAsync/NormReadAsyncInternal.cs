using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        private async IAsyncEnumerable<T> ReadInternalAsync<T>(string command, 
            Func<DbDataReader, Task<T>> readerAction)
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

        private async IAsyncEnumerable<T> ReadInternalAsync<T>(FormattableString command, 
            Func<DbDataReader, Task<T>> readerAction)
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

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await  cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArray(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await  cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return ReadToArray(reader);
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArray(reader, readerCallback);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return ReadToArray(reader, readerCallback);
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value, bool set)[]> ReadToArrayWithSetInternalAsync(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArrayWithSet(reader, readerCallback);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return ReadToArrayWithSet(reader, readerCallback);
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArray(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return ReadToArray(reader);
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArray(reader, readerCallback);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return ReadToArray(reader, readerCallback);
                }
            }
        }

        private async IAsyncEnumerable<(string name, object value, bool set)[]> ReadToArrayWithSetInternalAsync(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArrayWithSet(reader, readerCallback);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return ReadToArrayWithSet(reader, readerCallback);
                }
            }
        }

        internal async ValueTask<T> GetFieldValueAsync<T>(DbDataReader reader, int ordinal, Type type)
        {
            if (await reader.IsDBNullAsync(ordinal))
            {
                return default;
            }

            if (type.IsEnum || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum))
            {
                var fieldType = reader.GetFieldType(ordinal);
                if (fieldType == typeof(string))
                {
                    if (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum)
                    {
                        return (T)Enum.Parse(type.GenericTypeArguments[0], reader.GetString(ordinal));
                    }
                    return (T)Enum.Parse(type, reader.GetString(ordinal));
                }
                if (fieldType == typeof(int))
                {
                    if (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum)
                    {
                        return (T)Enum.ToObject(type.GenericTypeArguments[0], reader.GetInt32(ordinal));
                    }
                    return (T)Enum.ToObject(type, reader.GetInt32(ordinal));
                }
            }

            return await reader.GetFieldValueAsync<T>(ordinal);
        }

        internal async ValueTask<T> GetFieldValueAsync<T>(DbDataReader reader, int ordinal, Type type,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var name = reader.GetName(ordinal);
            var callback = readerCallback((name, ordinal, reader));
            if (callback == null)
            {
                return await GetFieldValueAsync<T>(reader, ordinal, type);
            }
            return (T)(callback == DBNull.Value ? null : callback);
        }
    }
}