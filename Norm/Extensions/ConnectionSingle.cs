using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Norm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IList<(string name, object value)> Single(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single(command);

        public static IList<(string name, object value)> Single(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);

        public static IList<(string name, object value)> Single(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
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

        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command);

        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command);

        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command);

        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
    }
}