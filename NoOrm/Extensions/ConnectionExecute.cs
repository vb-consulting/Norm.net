using System.Data.Common;

namespace NoOrm.Extensions
{
    public static partial class ConnectionExtensions
    {
        public static DbConnection Execute(this DbConnection connection, string command)
        {
            connection.GetNoOrmInstance().Execute(command);
            return connection;
        }

        public static DbConnection Execute(this DbConnection connection, string command, params object[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }

        public static DbConnection Execute(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
    }
}
