using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static T DeserializeSingleJson<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().DeserializeSingleJson<T>(command);

        public static T DeserializeSingleJson<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().DeserializeSingleJson<T>(command, parameters);

        public static T DeserializeSingleJson<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().DeserializeSingleJson<T>(command, parameters);
    }
}