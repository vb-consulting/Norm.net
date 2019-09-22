using System;
using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static DbConnection Read(this DbConnection connection, string command, Func<IDictionary<string, object>, bool> results)
        {
            new NoOrmAccess(connection).Read(command, results);
            return connection;
        }

        public static DbConnection Read(this DbConnection connection, string command, Func<IDictionary<string, object>, bool> results, params object[] parameters)
        {
            new NoOrmAccess(connection).Read(command, results, parameters);
            return connection;
        }

        public static DbConnection Read(this DbConnection connection, string command, Func<IDictionary<string, object>, bool> results, params (string name, object value)[] parameters)
        {
            new NoOrmAccess(connection).Read(command, results, parameters);
            return connection;
        }
    }
}