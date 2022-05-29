using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        internal async IAsyncEnumerable<T> ReadInternalAsync<T>(string command, 
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

        internal async IAsyncEnumerable<T> ReadInternalAsync<T>(FormattableString command, 
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

        internal async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (this.readerCallback == null)
            {
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
            else
            {
                if (cancellationToken.HasValue)
                {
                    await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                    while (await reader.ReadAsync(cancellationToken.Value))
                    {
                        yield return ReadToArray(reader, this.readerCallback);
                        cancellationToken?.ThrowIfCancellationRequested();
                    }
                }
                else
                {
                    await using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        yield return ReadToArray(reader, this.readerCallback);
                    }
                }
            }
        }

        internal async IAsyncEnumerable<(string name, object value, bool set)[]> ReadToArrayWithCallbackInternalAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArrayWithSet(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return ReadToArrayWithSet(reader);
                }
            }
        }

        internal async IAsyncEnumerable<(string name, object value)[]> ReadToArrayInternalAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);

            if (this.readerCallback == null)
            {
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
            else
            {
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
        }


        internal async IAsyncEnumerable<(string name, object value, bool set)[]> ReadToArrayWithCallbackInternalAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken.Value);
                while (await reader.ReadAsync(cancellationToken.Value))
                {
                    yield return ReadToArrayWithSet(reader);
                    cancellationToken?.ThrowIfCancellationRequested();
                }
            }
            else
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    yield return ReadToArrayWithSet(reader);
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

        internal async ValueTask<T> GetFieldValueWithReaderCallbackAsync<T>(DbDataReader reader, int ordinal, Type type)
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