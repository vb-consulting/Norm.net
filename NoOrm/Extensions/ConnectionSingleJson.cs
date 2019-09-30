using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static T SingleJson<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().SingleJson<T>(command);

        public static T SingleJson<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().SingleJson<T>(command, parameters);

        public static T SingleJson<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().SingleJson<T>(command, parameters);
    }
}