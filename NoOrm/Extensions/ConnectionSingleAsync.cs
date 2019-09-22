using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static async Task<IDictionary<string, object>> SingleAsync(this DbConnection connection, string command) =>
            await new NoOrmAccess(connection).SingleAsync(command);

        public static async Task<IDictionary<string, object>> SingleAsync(this DbConnection connection, string command, params object[] parameters) =>
            await new NoOrmAccess(connection).SingleAsync(command, parameters);

        public static async Task<IDictionary<string, object>> SingleAsync(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await new NoOrmAccess(connection).SingleAsync(command, parameters);
    }
}