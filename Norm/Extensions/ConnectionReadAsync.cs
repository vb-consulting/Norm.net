using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Norm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IAsyncEnumerable<IList<(string name, object value)>> ReadAsync(this DbConnection connection, string command) 
            => connection.GetNoOrmInstance().ReadAsync(command);

        public static IAsyncEnumerable<IList<(string name, object value)>> ReadAsync(this DbConnection connection, string command, params object[] parameters) 
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);

        public static IAsyncEnumerable<IList<(string name, object value)>> ReadAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);

        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T>(command);

        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);

        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);

        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command);
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);

        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command);
        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command);
        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command);
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command);
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command);
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command);
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command);
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command);
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params object[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);

        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
    }
}