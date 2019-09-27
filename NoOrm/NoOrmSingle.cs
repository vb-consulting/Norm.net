using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm
    {
        public IEnumerable<(string name, object value)> Single(string command) =>
            SingleInternal<IEnumerable<(string name, object value)>>(command,
                r => r.Read()
                    ? r.ToTuples().ToList()
                    : new List<(string name, object value)>());

        public IEnumerable<(string name, object value)> Single(string command, params object[] parameters) =>
            SingleInternal<IEnumerable<(string name, object value)>>(command,
                r => r.Read()
                    ? r.ToTuples().ToList()
                    : new List<(string name, object value)>(),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(string name, object value)> Single(string command, params (string name, object value)[] parameters) =>
            SingleInternal<IEnumerable<(string name, object value)>>(command,
                r => r.Read()
                    ? r.ToTuples().ToList()
                    : new List<(string name, object value)>(),
                cmd => cmd.AddParameters(parameters));

        public T Single<T>(string command) =>
            SingleInternal(command,
                r => r.Read()
                    ? GetFieldValue<T>(r,0)
                    : default);

        public T Single<T>(string command, params object[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? GetFieldValue<T>(r,0)
                    : default,
                cmd => cmd.AddParameters(parameters));

        public T Single<T>(string command, params (string name, object value)[] parameters) =>
            SingleInternal<T>(command,
                r => r.Read()
                    ? GetFieldValue<T>(r,0)
                    : default,
                cmd => cmd.AddParameters(parameters));

        public (T1, T2) Single<T1, T2>(string command) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r,0), GetFieldValue<T2>(r,1))
                    : (default, default));

        public (T1, T2) Single<T1, T2>(string command, params object[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r,0), GetFieldValue<T2>(r,1))
                    : (default, default),
                cmd => cmd.AddParameters(parameters));

        public (T1, T2) Single<T1, T2>(string command, params (string name, object value)[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r,0), GetFieldValue<T2>(r,1))
                    : (default, default),
                cmd => cmd.AddParameters(parameters));

        public (T1, T2, T3) Single<T1, T2, T3>(string command) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2))
                    : (default, default, default));

        public (T1, T2, T3) Single<T1, T2, T3>(string command, params object[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2))
                    : (default, default, default),
                cmd => cmd.AddParameters(parameters));

        public (T1, T2, T3) Single<T1, T2, T3>(string command, params (string name, object value)[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2))
                    : (default, default, default),
                cmd => cmd.AddParameters(parameters));

        public (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3))
                    : (default, default, default, default));

        public (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params object[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3))
                    : (default, default, default, default),
                cmd => cmd.AddParameters(parameters));

        public (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3))
                    : (default, default, default, default),
                cmd => cmd.AddParameters(parameters));

        public (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3), GetFieldValue<T5>(r, 4))
                    : (default, default, default, default, default));

        public (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params object[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3), GetFieldValue<T5>(r, 4))
                    : (default, default, default, default, default),
                cmd => cmd.AddParameters(parameters));

        public (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters) =>
            SingleInternal(command,
                r => r.Read()
                    ? (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3), GetFieldValue<T5>(r, 4))
                    : (default, default, default, default, default),
                cmd => cmd.AddParameters(parameters));




        private T SingleInternal<T>(string command, Func<DbDataReader, T> readerAction, Action<DbCommand> commandAction = null)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            commandAction?.Invoke(cmd);
            using var reader = cmd.ExecuteReader();
            return readerAction(reader);
        }
    }
}
