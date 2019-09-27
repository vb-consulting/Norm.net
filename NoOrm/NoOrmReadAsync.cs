using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using NoOrm.Extensions;

namespace NoOrm
{
    public partial class NoOrm
    {
        public IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command) =>
            ReadInternalAsync<IAsyncEnumerable<(string name, object value)>>(command,
                r => r.ToTuplesAsync());

        public IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command, params object[] parameters) =>
            ReadInternalAsync<IAsyncEnumerable<(string name, object value)>>(command,
                r => r.ToTuplesAsync(),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync<IAsyncEnumerable<(string name, object value)>>(command,
                r => r.ToTuplesAsync(),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<T> ReadAsync<T>(string command) =>
            ReadInternalAsync(command,
                async r => await GetFieldValueAsync<T>(r,0));

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => await GetFieldValueAsync<T>(r,0),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => await GetFieldValueAsync<T>(r,0),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1)));

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1)),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1)),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2)));

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2)),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2)),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3)));

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3)),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3)),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4)));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4)),
                cmd => cmd.AddParameters(parameters));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4)),
                cmd => cmd.AddParameters(parameters));



        private async IAsyncEnumerable<T> ReadInternalAsync<T>(string command, Func<DbDataReader, T> readerAction, Action<DbCommand> commandAction = null)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            commandAction?.Invoke(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return readerAction(reader);
            }
        }

        private async IAsyncEnumerable<T> ReadInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, Action<DbCommand> commandAction = null)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            commandAction?.Invoke(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return await readerAction(reader);
            }
        }
    }
}
