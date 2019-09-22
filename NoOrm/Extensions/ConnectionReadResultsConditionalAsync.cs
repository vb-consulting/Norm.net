using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, Func<IDictionary<string, object>, bool> results)
        {
            await new NoOrmAccess(connection).ReadAsync(command, results);
            return connection;
        }

        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, Func<IDictionary<string, object>, bool> results, params object[] parameters)
        {
            await new NoOrmAccess(connection).ReadAsync(command, results, parameters);
            return connection;
        }

        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, Func<IDictionary<string, object>, bool> results, params (string name, object value)[] parameters)
        {
            await new NoOrmAccess(connection).ReadAsync(command, results, parameters);
            return connection;
        }
    }
}