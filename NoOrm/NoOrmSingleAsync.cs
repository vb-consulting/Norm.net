using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using NoOrm.Extensions;

namespace NoOrm
{
    public partial class NoOrm
    {
        public IAsyncEnumerable<(string name, object value)> SingleAsync(string command) =>
            SingleInternalAsyncEnumerable(command);

        public IAsyncEnumerable<(string name, object value)> SingleAsync(string command, params object[] parameters) =>
            SingleInternalAsyncEnumerable(command, cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(string name, object value)> SingleAsync(string command, params (string name, object value)[] parameters) =>
            SingleInternalAsyncEnumerable(command, cmd => cmd.AddParameters(parameters));

        public async ValueTask<T> SingleAsync<T>(string command) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r,0)
                    : default);

        public async ValueTask<T> SingleAsync<T>(string command, params object[] parameters) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r,0)
                    : default,
                cmd => cmd.AddParameters(parameters));
        public async ValueTask<T> SingleAsync<T>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r,0)
                    : default,
                cmd => cmd.AddParameters(parameters));
        public async ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1))
                    : (default, default));
        public async ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1))
                    : (default, default),
                cmd => cmd.AddParameters(parameters));
        public async ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1))
                    : (default, default),
                cmd => cmd.AddParameters(parameters));

        public async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default));
        public async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default),
                cmd => cmd.AddParameters(parameters));
        public async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default),
                cmd => cmd.AddParameters(parameters));

        public async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default));
        public async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default),
                cmd => cmd.AddParameters(parameters));
        public async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default),
                cmd => cmd.AddParameters(parameters));

        public async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4))
                    : (default, default, default, default, default));
        public async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4))
                    : (default, default, default, default, default),
                cmd => cmd.AddParameters(parameters));
        public async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4))
                    : (default, default, default, default, default),
                cmd => cmd.AddParameters(parameters));




        private async ValueTask<T> SingleInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, Action<DbCommand> commandAction = null)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            commandAction?.Invoke(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            OnCommandExecuted(cmd);
            return await readerAction(reader);
        }

        private async IAsyncEnumerable<(string name, object value)> SingleInternalAsyncEnumerable(string command, Action<DbCommand> commandAction = null)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            commandAction?.Invoke(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            OnCommandExecuted(cmd);
            if (!await reader.ReadAsync()) yield break;
            for (var index = 0; index < reader.FieldCount; index++)
            {
                yield return (reader.GetName(index), await reader.GetFieldValueAsync<object>(index));
            }
        }
        
    }
}
