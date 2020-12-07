using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public static partial class NormExtensions
    {
        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command);
            return connection;
        }

        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, params object[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }

        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }

        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }

        public static async ValueTask<DbConnection> ExecuteAsync(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            await connection.GetNoOrmInstance().ExecuteAsync(command, parameters);
            return connection;
        }
    }
}
