using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static IEnumerable<(string name, object value)> Single(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single(command);

        public static IEnumerable<(string name, object value)> Single(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);

        public static IEnumerable<(string name, object value)> Single(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
    }
}