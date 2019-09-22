using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static IEnumerable<IEnumerable<(string name, object value)>> Read(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Read(command);

        public static IEnumerable<IEnumerable<(string name, object value)>> Read(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Read(command, parameters);

        public static IEnumerable<IEnumerable<(string name, object value)>> Read(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Read(command, parameters);
    }
}