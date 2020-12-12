using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        //
        // Summary:
        //     Execute SQL command.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        //   command:
        //     SQL command text.
        //
        // Returns:
        //     Same DbConnection instance.
        public static DbConnection Execute(this DbConnection connection, string command)
        {
            connection.GetNoOrmInstance().Execute(command);
            return connection;
        }
        //
        // Summary:
        //     Execute SQL command with positional parameter values.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters objects array.
        //
        // Returns:
        //     Same DbConnection instance.
        public static DbConnection Execute(this DbConnection connection, string command, params object[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
        //
        // Summary:
        //     Execute SQL command with named parameter values.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name and value tuples array - (string name, object value).
        //
        // Returns:
        //     Same DbConnection instance.
        public static DbConnection Execute(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
        //
        // Summary:
        //     Execute SQL command with named parameter values and DbType type for each parameter.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuples array - (string name, object value, DbType type).
        //
        // Returns:
        //     Same DbConnection instance.
        public static DbConnection Execute(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
        //
        // Summary:
        //     Execute SQL command with named parameter values and custom type for each parameter.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        //   command:
        //     SQL command text.
        //
        //   parameters:
        //     Parameters name, value and type tuples array - (string name, object value, object type).
        //     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        //
        // Returns:
        //     Same DbConnection instance.
        public static DbConnection Execute(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            connection.GetNoOrmInstance().Execute(command, parameters);
            return connection;
        }
    }
}
