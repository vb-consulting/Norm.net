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
        public async Task<IEnumerable<(string name, object value)>> SingleAsync(string command) =>
            await SingleInternalAsync<IEnumerable<(string name, object value)>>(command, 
                async r => await r.ReadAsync()
                    ? r.ToTuples().ToList()
                    : new List<(string name, object value)>());

        public async Task<IEnumerable<(string name, object value)>> SingleAsync(string command, params object[] parameters) =>
            await SingleInternalAsync<IEnumerable<(string name, object value)>>(command,
                async r => await r.ReadAsync()
                    ? r.ToTuples().ToList()
                    : new List<(string name, object value)>(),
                cmd => cmd.AddParameters(parameters));

        public async Task<IEnumerable<(string name, object value)>> SingleAsync(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync<IEnumerable<(string name, object value)>>(command,
                async r => await r.ReadAsync()
                    ? r.ToTuples().ToList()
                    : new List<(string name, object value)>(),
                cmd => cmd.AddParameters(parameters));

        public async Task<T> SingleAsync<T>(string command) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r,0)
                    : default);

        public async Task<T> SingleAsync<T>(string command, params object[] parameters) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r,0)
                    : default,
                cmd => cmd.AddParameters(parameters));
        public async Task<T> SingleAsync<T>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r,0)
                    : default,
                cmd => cmd.AddParameters(parameters));
        public async Task<(T1, T2)> SingleAsync<T1, T2>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1))
                    : (default, default));
        public async Task<(T1, T2)> SingleAsync<T1, T2>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1))
                    : (default, default),
                cmd => cmd.AddParameters(parameters));
        public async Task<(T1, T2)> SingleAsync<T1, T2>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r,0), await GetFieldValueAsync<T2>(r,1))
                    : (default, default),
                cmd => cmd.AddParameters(parameters));

        public async Task<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default));
        public async Task<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default),
                cmd => cmd.AddParameters(parameters));
        public async Task<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default),
                cmd => cmd.AddParameters(parameters));

        public async Task<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default));
        public async Task<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default),
                cmd => cmd.AddParameters(parameters));
        public async Task<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default),
                cmd => cmd.AddParameters(parameters));

        public async Task<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4))
                    : (default, default, default, default, default));
        public async Task<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4))
                    : (default, default, default, default, default),
                cmd => cmd.AddParameters(parameters));
        public async Task<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1), await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4))
                    : (default, default, default, default, default),
                cmd => cmd.AddParameters(parameters));




        private async Task<T> SingleInternalAsync<T>(string command, Func<DbDataReader, Task<T>> readerAction, Action<DbCommand> commandAction = null)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            commandAction?.Invoke(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await readerAction(reader);
        }
    }
}
