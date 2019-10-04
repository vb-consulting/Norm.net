using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static async ValueTask<IList<(string name, object value)>> SingleAsync(this DbConnection connection, string command) =>
            await connection.GetNoOrmInstance().SingleAsync(command);

        public static async ValueTask<IList<(string name, object value)>> SingleAsync(this DbConnection connection, string command, params object[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync(command, parameters);

        public static async ValueTask<IList<(string name, object value)>> SingleAsync(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync(command, parameters);

        public static async ValueTask<T> SingleAsync<T>(this DbConnection connection, string command) =>
            await connection.GetNoOrmInstance().SingleAsync<T>(command);

        public static async ValueTask<T> SingleAsync<T>(this DbConnection connection, string command, params object[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T>(command, parameters);

        public static async ValueTask<T> SingleAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T>(command, parameters);

        public static async ValueTask<(T1, T2)> SingleAsync<T1, T2>(this DbConnection connection, string command) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2>(command);

        public static async ValueTask<(T1, T2)> SingleAsync<T1, T2>(this DbConnection connection, string command, params object[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2>(command, parameters);

        public static async ValueTask<(T1, T2)> SingleAsync<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2>(command, parameters);

        public static async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(this DbConnection connection, string command) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3>(command);

        public static async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(this DbConnection connection, string command, params object[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3>(command, parameters);

        public static async ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3>(command, parameters);

        public static async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(this DbConnection connection, string command) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3, T4>(command);

        public static async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3, T4>(command, parameters);

        public static async ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3, T4>(command, parameters);

        public static async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3, T4, T5>(command);

        public static async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params object[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3, T4, T5>(command, parameters);

        public static async ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync<T1, T2, T3, T4, T5>(command, parameters);
    }
}