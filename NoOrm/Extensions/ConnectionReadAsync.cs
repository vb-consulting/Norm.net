using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(this DbConnection connection, string command) 
            => connection.GetNoOrmInstance().ReadAsync(command);

        public static IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(this DbConnection connection, string command, params object[] parameters) 
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);

        public static IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(this DbConnection connection, string command, params (string name, object value)[] parameters)
            => connection.GetNoOrmInstance().ReadAsync(command, parameters);
    }
}