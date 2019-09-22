using System;
using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static IEnumerable<IDictionary<string, object>> Read(this DbConnection connection, string command) =>
            new NoOrmAccess(connection).Read(command);

        public static IEnumerable<IDictionary<string, object>> Read(this DbConnection connection, string command, params object[] parameters) =>
            new NoOrmAccess(connection).Read(command, parameters);

        public static IEnumerable<IDictionary<string, object>> Read(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            new NoOrmAccess(connection).Read(command, parameters);
    }
}