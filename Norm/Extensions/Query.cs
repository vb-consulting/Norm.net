using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IEnumerable<T> Query<T>(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().Read(command).Map<T>();
        }

        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().ReadAsync(command).Map<T>();
        }

        public static IEnumerable<T> Query<T>(this DbConnection connection, string command, params object[] parameters)
        {
            return connection.GetNoOrmInstance().Read(command, parameters).Map<T>();
        }

        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params object[] parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync(command, parameters).Map<T>();
        }

        public static IEnumerable<T> Query<T>(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            return connection.GetNoOrmInstance().Read(command, parameters).Map<T>();
        }

        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync(command, parameters).Map<T>();
        }

        public static IEnumerable<T> Query<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            return connection.GetNoOrmInstance().Read(command, parameters).Map<T>();
        }

        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync(command, parameters).Map<T>();
        }

        public static IEnumerable<T> Query<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            return connection.GetNoOrmInstance().Read(command, parameters).Map<T>();
        }

        public static IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync(command, parameters).Map<T>();
        }
    }
}
