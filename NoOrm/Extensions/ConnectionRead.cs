using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IEnumerable<IEnumerable<(string name, object value)>> Read(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read(command);

        public static IEnumerable<IEnumerable<(string name, object value)>> Read(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read(command, parameters);

        public static IEnumerable<IEnumerable<(string name, object value)>> Read(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read(command, parameters);

        public static IEnumerable<T> Read<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T>(command);

        public static IEnumerable<T> Read<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T>(command, parameters);

        public static IEnumerable<T> Read<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T>(command, parameters);

        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read<T1, T2>(command);

        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2>(command, parameters);

        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read<T1, T2>(command, parameters);
    }
}