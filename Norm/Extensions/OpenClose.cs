using System.Data;
using System.Data.Common;
using System.Threading;
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

        public static async Task<DbConnection> EnsureIsOpenAsync(this DbConnection connection, CancellationToken? cancellationToken = null)
        {
            if (connection.State == ConnectionState.Open)
            {
                return connection;
            }
            cancellationToken?.ThrowIfCancellationRequested();
            if (cancellationToken.HasValue)
            {
                await connection.OpenAsync(cancellationToken.Value);
            }
            else
            {
                await connection.OpenAsync();
            }
            return connection;
        }
    }
}
