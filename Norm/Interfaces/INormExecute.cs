using System.Data;

namespace Norm.Interfaces
{
    public interface INormExecute
    {
        ///
        /// Summary:
        ///     Execute SQL command.
        ///
        ///
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     Norm instance.
        INorm Execute(string command);
        ///
        /// Summary:
        ///     Execute SQL command with positional parameter values.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     Norm instance.
        INorm Execute(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Execute SQL command with named parameter values.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuples array - (string name, object value).
        ///
        /// Returns:
        ///     Norm instance.
        INorm Execute(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Execute SQL command with named parameter values and DbType type for each parameter.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///
        /// Returns:
        ///     Norm instance.
        INorm Execute(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Execute SQL command with named parameter values and custom type for each parameter.
        ///
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuples array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     Norm instance.
        INorm Execute(string command, params (string name, object value, object type)[] parameters);
    }
}