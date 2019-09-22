using System;
using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static IDictionary<string, object> Single(this DbConnection connection, string command) =>
            new NoOrmAccess(connection).Single(command);

        public static IDictionary<string, object> Single(this DbConnection connection, string command, params object[] parameters) =>
            new NoOrmAccess(connection).Single(command, parameters);

        public static IDictionary<string, object> Single(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            new NoOrmAccess(connection).Single(command, parameters);
    }
}