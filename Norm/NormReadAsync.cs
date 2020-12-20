using System;
using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command) =>
            ReadToArrayInternalAsync(command);

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params object[] parameters) =>
            ReadToArrayInternalAsync(command, parameters);

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value)[] parameters) =>
            ReadToArrayInternalAsync(command, parameters);

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadToArrayInternalAsync(command, parameters);

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadToArrayInternalUnknownParamsTypeAsync(command, parameters);

        public IAsyncEnumerable<T> ReadAsync<T>(string command)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0));
            }
            return ReadAsync(command).Map<T>(type);
        }

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params object[] parameters)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0), parameters);
            }
            return ReadAsync(command, parameters).Map<T>(type);
        }

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value)[] parameters)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0), parameters);
            }
            return ReadAsync(command, parameters).Map<T>(type);
        }

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, DbType type)[] parameters)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0), parameters);
            }
            return ReadAsync(command, parameters).Map<T>(type);
        }

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, object type)[] parameters)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternalUnknownParamsTypeAsync(command, async r => await GetFieldValueAsync<T>(r, 0), parameters);
            }
            return ReadAsync(command, parameters).Map<T>(type);
        }

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1)));

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1)),
                parameters);

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1)),
                parameters);

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1)),
                parameters);

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1)),
                parameters);


        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2)));

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2)),
                parameters);


        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3)));

        public IAsyncEnumerable<(T1, T2, T3, T4)>
            ReadAsync<T1, T2, T3, T4>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command,
          params (string name, object value, object type)[] parameters) =>
          ReadInternalUnknownParamsTypeAsync(command,
              async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                  await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3)),
              parameters);


        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3),
                    await GetFieldValueAsync<T5>(r, 4)));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
            params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3),
                    await GetFieldValueAsync<T5>(r, 4)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3),
                    await GetFieldValueAsync<T5>(r, 4)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3),
                    await GetFieldValueAsync<T5>(r, 4)),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2), await GetFieldValueAsync<T4>(r, 3),
                    await GetFieldValueAsync<T5>(r, 4)),
                parameters);


        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5)
                ));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command,
            params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6)
                ));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6)
                ),
                parameters);


        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7)
                ));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7)
                ),
                parameters);


        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8)
                ));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8)
                ),
                parameters);


        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9)
                ));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9)
                ),
                parameters);


        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command) =>
            ReadInternalAsync(command,
        async r => (
                await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                await GetFieldValueAsync<T3>(r, 2),
                await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                await GetFieldValueAsync<T6>(r, 5),
                await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                await GetFieldValueAsync<T9>(r, 8),
                await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10)
            ));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10)
                ),
                parameters);


        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command) =>
            ReadInternalAsync(command,
        async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10), await GetFieldValueAsync<T12>(r, 11)
                ));

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(string command, params object[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10), await GetFieldValueAsync<T12>(r, 11)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10), await GetFieldValueAsync<T12>(r, 11)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternalAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10), await GetFieldValueAsync<T12>(r, 11)
                ),
                parameters);

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknownParamsTypeAsync(command,
                async r => (
                    await GetFieldValueAsync<T1>(r, 0), await GetFieldValueAsync<T2>(r, 1),
                    await GetFieldValueAsync<T3>(r, 2),
                    await GetFieldValueAsync<T4>(r, 3), await GetFieldValueAsync<T5>(r, 4),
                    await GetFieldValueAsync<T6>(r, 5),
                    await GetFieldValueAsync<T7>(r, 6), await GetFieldValueAsync<T8>(r, 7),
                    await GetFieldValueAsync<T9>(r, 8),
                    await GetFieldValueAsync<T10>(r, 9), await GetFieldValueAsync<T11>(r, 10), await GetFieldValueAsync<T12>(r, 11)
                ),
                parameters);
    }
}