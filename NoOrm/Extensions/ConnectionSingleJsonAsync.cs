using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static async ValueTask<T> SingleJsonAsync<T>(this DbConnection connection, string command) =>
            await connection.GetNoOrmInstance().SingleJsonAsync<T>(command);

        public static async ValueTask<T> SingleJsonAsync<T>(this DbConnection connection, string command, params object[] parameters) =>
            await connection.GetNoOrmInstance().SingleJsonAsync<T>(command, parameters);

        public static async ValueTask<T> SingleJsonAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            await connection.GetNoOrmInstance().SingleJsonAsync<T>(command, parameters);
    }
}