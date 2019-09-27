using System;
using System.Collections.Generic;
using System.Data.Common;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm
    {
        public IEnumerable<IEnumerable<(string name, object value)>> Read(string command) => 
            ReadInternal(command, r => r.ToTuples());

        public IEnumerable<IEnumerable<(string name, object value)>> Read(string command, params object[] parameters) => 
            ReadInternal(command, r => r.ToTuples(), cmd => cmd.AddParameters(parameters));

        public IEnumerable<IEnumerable<(string name, object value)>> Read(string command, params (string name, object value)[] parameters) => 
            ReadInternal(command, r => r.ToTuples(), cmd => cmd.AddParameters(parameters));

        public IEnumerable<T> Read<T>(string command) => 
            ReadInternal(command, r => GetFieldValue<T>(r,0));

        public IEnumerable<T> Read<T>(string command, params object[] parameters) => 
            ReadInternal(command, r => GetFieldValue<T>(r,0), cmd => cmd.AddParameters(parameters));

        public IEnumerable<T> Read<T>(string command, params (string name, object value)[] parameters) => 
            ReadInternal(command, r => GetFieldValue<T>(r,0), cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command) =>
            ReadInternal(command, 
                r => (GetFieldValue<T1>(r,0), GetFieldValue<T2>(r,1)));

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters) =>
            ReadInternal(command, 
                r => (GetFieldValue<T1>(r,0), GetFieldValue<T2>(r,1)),
                cmd => cmd.AddParameters(parameters));

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters) =>
            ReadInternal(command, 
                r => (GetFieldValue<T1>(r,0), GetFieldValue<T2>(r,1)),
                cmd => cmd.AddParameters(parameters));

        private IEnumerable<T> ReadInternal<T>(string command, Func<DbDataReader, T> readerAction, Action<DbCommand> commandAction = null)
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
