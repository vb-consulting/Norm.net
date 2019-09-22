using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;


namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static async Task<DbConnection> ExecuteAsync(this DbConnection connection, string command)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command);
            return connection;
        }

        public static async Task<DbConnection> ExecuteAsync(this DbConnection connection, string command, params object[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }

        public static async Task<DbConnection> ExecuteAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }
    }
}
