using System.Data;
using System.Threading.Tasks;

namespace Norm.Interfaces
{
    public interface INormExecuteAsync
    {
        ///
        /// Summary:
        ///     Execute SQL command asynchronously.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        ValueTask<INorm> ExecuteAsync(string command);
        ///
        /// Summary:
        ///      Execute SQL command asynchronously with positional parameter values.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters objects array.
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        ValueTask<INorm> ExecuteAsync(string command, params object[] parameters);
        ///
        /// Summary:
        ///     Execute SQL command asynchronously with named parameter values.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuples array - (string name, object value).
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        ValueTask<INorm> ExecuteAsync(string command, params (string name, object value)[] parameters);
        ///
        /// Summary:
        ///     Execute SQL command asynchronously with named parameter values DbType type for each parameter.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name and value tuples array - (string name, object value).
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same DbConnection instance.
        ValueTask<INorm> ExecuteAsync(string command, params (string name, object value, DbType type)[] parameters);
        ///
        /// Summary:
        ///     Execute SQL command asynchronously with named parameter values and custom type for each parameter.
        ///
        /// Parameters:
        ///   command:
        ///     SQL command text.
        ///
        ///   parameters:
        ///     Parameters name, value and type tuples array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///
        /// Returns:
        ///     A value task representing the asynchronous operation returning the same Norm instance.
        ValueTask<INorm> ExecuteAsync(string command, params (string name, object value, object type)[] parameters);
    }
}