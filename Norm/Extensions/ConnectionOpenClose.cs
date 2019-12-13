using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm.Extensions
{
    public static partial class ConnectionExtensions
    {
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
