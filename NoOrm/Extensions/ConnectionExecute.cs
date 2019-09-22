using System;
using System.Data.Common;


namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        public static DbConnection Execute(this DbConnection connection, string command)
        {
            new NoOrmAccess(connection).Execute(command);
            return connection;
        }

        public static DbConnection Execute(this DbConnection connection, string command, params object[] parameters)
        {
            new NoOrmAccess(connection).Execute(command, parameters);
            return connection;
        }

        public static DbConnection Execute(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            new NoOrmAccess(connection).Execute(command, parameters);
            return connection;
        }
    }
}
