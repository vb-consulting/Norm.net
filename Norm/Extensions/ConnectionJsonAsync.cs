using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IAsyncEnumerable<T> JsonAsync<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().JsonAsync<T>(command);

        public static IAsyncEnumerable<T> JsonAsync<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().JsonAsync<T>(command, parameters);

        public static IAsyncEnumerable<T> JsonAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().JsonAsync<T>(command, parameters);

        public static IAsyncEnumerable<T> JsonAsync<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().JsonAsync<T>(command, parameters);
    }
}
