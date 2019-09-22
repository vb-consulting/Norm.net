using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static DbConnection Read(this DbConnection connection, string command, RowCallback results)
        {
            connection.GetNoOrmInstance().Read(command, results);
            return connection;
        }

        public static DbConnection Read(this DbConnection connection, string command, RowCallback results, params object[] parameters)
        {
            connection.GetNoOrmInstance().Read(command, results, parameters);
            return connection;
        }

        public static DbConnection Read(this DbConnection connection, string command, RowCallback results, params (string name, object value)[] parameters)
        {
            connection.GetNoOrmInstance().Read(command, results, parameters);
            return connection;
        }
    }
}