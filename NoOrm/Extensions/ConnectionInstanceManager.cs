using System;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;


namespace NoOrm
{
    public static partial class ConnectionExtensions
    {
        private static readonly ConditionalWeakTable<DbConnection, NoOrmAccess> Table;
        static ConnectionExtensions()
        {
            Table = new ConditionalWeakTable<DbConnection, NoOrmAccess>();
        }

        internal static NoOrmAccess GetNoOrmInstance(this DbConnection connection)
        {
            if (Table.TryGetValue(connection, out var instance))
            {
                return instance;
            }
            instance = new NoOrmAccess(connection);
            Table.Add(connection, instance);
            return instance;
        }

        public static DbConnection As(this DbConnection connection, CommandType type)
        {
            var instance = connection.GetNoOrmInstance();
            instance.As(type);
            return connection;
        }

        public static DbConnection Timeout(this DbConnection connection, int? timeout)
        {
            var instance = connection.GetNoOrmInstance();
            instance.Timeout(timeout);
            return connection;
        }
    }
}
