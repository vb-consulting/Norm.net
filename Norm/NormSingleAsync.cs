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
        public async ValueTask<IList<(string name, object value)>> SingleAsync(string command) =>
            await SingleInternalAsync(command, async r => await r.ReadAsync() ? r.ToList() : default);

        public async ValueTask<IList<(string name, object value)>> SingleAsync(string command,
            params object[] parameters) =>
            await SingleInternalAsync(command, async r => await r.ReadAsync() ? r.ToList() : default,
                parameters);

        public async ValueTask<IList<(string name, object value)>> SingleAsync(string command,
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command, async r => await r.ReadAsync() ? r.ToList() : default,
                parameters);

        public async ValueTask<IList<(string name, object value)>> SingleAsync(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command, async r => await r.ReadAsync() ? r.ToList() : default,
                parameters);

        public async ValueTask<T> SingleAsync<T>(string command) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r, 0)
                    : default);

        public async ValueTask<T> SingleAsync<T>(string command, params object[] parameters) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r, 0)
                    : default,
                parameters);

        public async ValueTask<T> SingleAsync<T>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r, 0)
                    : default,
                parameters);

        public async ValueTask<T> SingleAsync<T>(string command, params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync<T>(command,
                async r => await r.ReadAsync()
                    ? await GetFieldValueAsync<T>(r, 0)
                    : default,
                parameters);

        public async ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1))
                    : (default, default));

        public async ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1))
                    : (default, default),
                parameters);

        public async ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command,
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1))
                    : (default, default),
                parameters);

        public async ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1))
                    : (default, default),
                parameters);

        public async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default));

        public async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command,
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2))
                    : (default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4)>
            SingleAsync<T1, T2, T3, T4>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command,
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3))
                    : (default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4)
                    )
                    : (default, default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command,
            params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4)
                    )
                    : (default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command,
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4)
                    )
                    : (default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4)
                    )
                    : (default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5)
                    )
                    : (default, default, default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command,
            params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5)
                    )
                    : (default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5)
                    )
                    : (default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5)
                    )
                    : (default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6)
                    )
                    : (default, default, default, default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6)
                    )
                    : (default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6)
                    )
                    : (default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6)
                    )
                    : (default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7)
                    )
                    : (default, default, default, default, default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7)
                    )
                    : (default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7)
                    )
                    : (default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7)
                    )
                    : (default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8)
                    )
                    : (default, default, default, default, default, default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8)
                    )
                    : (default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8)
                    )
                    : (default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8)
                    )
                    : (default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10>(string command) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9)
                    )
                    : (default, default, default, default, default, default, default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9)
                    )
                    : (default, default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10>(string command, params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9)
                    )
                    : (default, default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10>(string command, params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9)
                    )
                    : (default, default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command) =>
            await SingleInternalAsync(command,
        async r => await r.ReadAsync()
            ? (
                await GetFieldValueAsync<T1>(r, 0),
                await GetFieldValueAsync<T2>(r, 1),
                await GetFieldValueAsync<T3>(r, 2),
                await GetFieldValueAsync<T4>(r, 3),
                await GetFieldValueAsync<T5>(r, 4),
                await GetFieldValueAsync<T6>(r, 5),
                await GetFieldValueAsync<T7>(r, 6),
                await GetFieldValueAsync<T8>(r, 7),
                await GetFieldValueAsync<T9>(r, 8),
                await GetFieldValueAsync<T10>(r, 9),
                await GetFieldValueAsync<T11>(r, 10)
            )
            : (default, default, default, default, default, default, default, default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9),
                        await GetFieldValueAsync<T11>(r, 10)
                    )
                    : (default, default, default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, 
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10)
                    )
                    : (default, default, default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10)
                    )
                    : (default, default, default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command) =>
            await SingleInternalAsync(command,
        async r => await r.ReadAsync()
            ? (
                await GetFieldValueAsync<T1>(r, 0),
                await GetFieldValueAsync<T2>(r, 1),
                await GetFieldValueAsync<T3>(r, 2),
                await GetFieldValueAsync<T4>(r, 3),
                await GetFieldValueAsync<T5>(r, 4),
                await GetFieldValueAsync<T6>(r, 5),
                await GetFieldValueAsync<T7>(r, 6),
                await GetFieldValueAsync<T8>(r, 7),
                await GetFieldValueAsync<T9>(r, 8),
                await GetFieldValueAsync<T10>(r, 9),
                await GetFieldValueAsync<T11>(r, 10),
                await GetFieldValueAsync<T12>(r, 11)
            )
            : (default, default, default, default, default, default, default, default, default, default, default, default));

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0),
                        await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3),
                        await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6),
                        await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9),
                        await GetFieldValueAsync<T11>(r, 10),
                        await GetFieldValueAsync<T12>(r, 11)
                    )
                    : (default, default, default, default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, 
            params (string name, object value)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10), await GetFieldValueAsync<T12>(r, 11)
                    )
                    : (default, default, default, default, default, default, default, default, default, default, default, default),
                parameters);

        public async ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            await SingleInternalAsync(command,
                async r => await r.ReadAsync()
                    ? (
                        await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                        await GetFieldValueAsync<T3>(r, 2),
                        await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                        await GetFieldValueAsync<T6>(r, 5),
                        await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                        await GetFieldValueAsync<T9>(r, 8),
                        await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10), await GetFieldValueAsync<T12>(r, 11)
                    )
                    : (default, default, default, default, default, default, default, default, default, default, default, default),
                parameters);


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
    }
}