using System;
using System.Collections.Generic;
using System.Data.Common;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<IList<(string name, object value)>> Read(string command) =>
            ReadInternal(command, r => r.ToList());

        public IEnumerable<IList<(string name, object value)>> Read(string command, params object[] parameters) =>
            ReadInternal(command, r => r.ToList(), cmd => cmd.AddParameters(parameters));

        public IEnumerable<IList<(string name, object value)>> Read(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command, r => r.ToList(), cmd => cmd.AddParameters(parameters));

        public IEnumerable<T> Read<T>(string command) =>
            ReadInternal(command, r => GetFieldValue<T>(r, 0));

        public IEnumerable<T> Read<T>(string command, params object[] parameters) =>
            ReadInternal(command, r => GetFieldValue<T>(r, 0), cmd => cmd.AddParameters(parameters));

        public IEnumerable<T> Read<T>(string command, params (string name, object value)[] parameters) =>
            ReadInternal(command, r => GetFieldValue<T>(r, 0), cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1)));

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1)),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1)),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2)));

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params object[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2)),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2)),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2),
                    GetFieldValue<T4>(r, 3)));

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params object[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2),
                    GetFieldValue<T4>(r, 3)),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2),
                    GetFieldValue<T4>(r, 3)),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4))
            );

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8)
                ),
                cmd => cmd.AddParameters(parameters));


        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10), GetFieldValue<T12>(r, 11)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10), GetFieldValue<T12>(r, 11)
                ),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10), GetFieldValue<T12>(r, 11)
                ),
                cmd => cmd.AddParameters(parameters));



        private IEnumerable<T> ReadInternal<T>(string command, Func<DbDataReader, T> readerAction,
            Action<DbCommand> commandAction = null)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            commandAction?.Invoke(cmd);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }
    }
}