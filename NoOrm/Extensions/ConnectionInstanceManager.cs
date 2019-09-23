using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        private static readonly ConditionalWeakTable<DbConnection, NoOrm> Table;
        static ConnectionExtensions()
        {
            Table = new ConditionalWeakTable<DbConnection, NoOrm>();
        }

        internal static NoOrm GetNoOrmInstance(this DbConnection connection)
        {
            if (Table.TryGetValue(connection, out var instance))
            {
                return instance;
            }
            instance = new NoOrm(connection);
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
