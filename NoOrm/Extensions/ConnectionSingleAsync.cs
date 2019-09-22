using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static async Task<IEnumerable<(string name, object value)>> SingleAsync(this DbConnection connection, string command) =>
            await connection.GetNoOrmInstance().SingleAsync(command);

        public static async Task<IEnumerable<(string name, object value)>> SingleAsync(this DbConnection connection, string command, params object[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync(command, parameters);

        public static async Task<IEnumerable<(string name, object value)>> SingleAsync(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await connection.GetNoOrmInstance().SingleAsync(command, parameters);
    }
}