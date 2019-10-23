using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Norm.Extensions
{
    public static partial class ConnectionExtensions
    {
        private static readonly ConditionalWeakTable<DbConnection, Norm> Table;
        static ConnectionExtensions()
        {
            Table = new ConditionalWeakTable<DbConnection, Norm>();
        }

        internal static Norm GetNoOrmInstance(this DbConnection connection)
        {
            if (Table.TryGetValue(connection, out var instance))
            {
                return instance;
            }
            instance = new Norm(connection);
            Table.Add(connection, instance);
            return instance;
        }

        public static DbConnection As(this DbConnection connection, CommandType type)
        {
            var instance = connection.GetNoOrmInstance();
            instance.As(type);
            return connection;
        }

        public static DbConnection AsProcedure(this DbConnection connection) => connection.As(CommandType.StoredProcedure);

        public static DbConnection AsText(this DbConnection connection) => connection.As(CommandType.Text);

        public static DbConnection Timeout(this DbConnection connection, int? timeout)
        {
            var instance = connection.GetNoOrmInstance();
            instance.Timeout(timeout);
            return connection;
        }

        public static DbConnection WithJsonOptions(this DbConnection connection, JsonSerializerOptions jsonOptions)
        {
            var instance = connection.GetNoOrmInstance();
            instance.WithJsonOptions(jsonOptions);
            return connection;
        }
    }
}
