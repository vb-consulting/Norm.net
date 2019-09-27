using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IEnumerable<(string name, object value)> Single(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single(command);

        public static IEnumerable<(string name, object value)> Single(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);

        public static IEnumerable<(string name, object value)> Single(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);

        public static T Single<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T>(command);

        public static T Single<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);

        public static T Single<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);

        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command);

        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);

        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);

        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command);

        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);

        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);

        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command);

        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);

        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);

        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command);

        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);

        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
    }
}