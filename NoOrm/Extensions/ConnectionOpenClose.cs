using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static DbConnection Open(this DbConnection connection)
        {
            connection.Open();
            return connection;
        }

        public static DbConnection Close(this DbConnection connection)
        {
            connection.Close();
            return connection;
        }

        public static async Task<DbConnection> OpenAsync(this DbConnection connection)
        {
            await connection.OpenAsync();
            return connection;
        }

        public static DbConnection EnsureIsOpen(this DbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }

        public static DbConnection EnsureIsClose(this DbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Close();
            }
            return connection;
        }

        public static async Task<DbConnection> EnsureIsOpenAsync(this DbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
            return connection;
        }
    }
}
