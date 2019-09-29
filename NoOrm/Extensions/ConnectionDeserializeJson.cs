using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IEnumerable<T> DeserializeJson<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().DeserializeJson<T>(command);

        public static IEnumerable<T> DeserializeJson<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().DeserializeJson<T>(command, parameters);

        public static IEnumerable<T> DeserializeJson<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().DeserializeJson<T>(command, parameters);
    }
}

