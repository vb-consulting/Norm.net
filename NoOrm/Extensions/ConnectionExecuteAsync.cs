using System;
using System.Data.Common;
using System.Threading.Tasks;


namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static async Task<DbConnection> ExecuteAsync(this DbConnection connection, string command)
        {
            await new NoOrmAccess(connection).ExecuteAsync(command);
            return connection;
        }

        public static async Task<DbConnection> ExecuteAsync(this DbConnection connection, string command, params object[] parameters)
        {
            await new NoOrmAccess(connection).ExecuteAsync(command, parameters);
            return connection;
        }

        public static async Task<DbConnection> ExecuteAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            await new NoOrmAccess(connection).ExecuteAsync(command, parameters);
            return connection;
        }
    }
}
