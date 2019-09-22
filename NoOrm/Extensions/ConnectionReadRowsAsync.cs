using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, RowCallback results)
        {
            await connection.GetNoOrmInstance().ReadAsync(command, results);
            return connection;
        }

        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, RowCallback results, params object[] parameters)
        {
            await connection.GetNoOrmInstance().ReadAsync(command, results, parameters);
            return connection;
        }

        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, RowCallback results, params (string name, object value)[] parameters)
        {
            await connection.GetNoOrmInstance().ReadAsync(command, results, parameters);
            return connection;
        }

        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, RowCallbackAsync results)
        {
            await connection.GetNoOrmInstance().ReadAsync(command, results);
            return connection;
        }

        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, RowCallbackAsync results, params object[] parameters)
        {
            await connection.GetNoOrmInstance().ReadAsync(command, results, parameters);
            return connection;
        }

        public static async Task<DbConnection> ReadAsync(this DbConnection connection, string command, RowCallbackAsync results, params (string name, object value)[] parameters)
        {
            await connection.GetNoOrmInstance().ReadAsync(command, results, parameters);
            return connection;
        }
    }
}