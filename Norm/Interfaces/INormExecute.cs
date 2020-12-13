using System.Data;

namespace Norm.Interfaces
{
    public interface INormExecute
    {
        ///<summary>
        ///     Execute SQL command.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>Norm instance.</returns>
        INorm Execute(string command);
        ///<summary>
        ///     Execute SQL command with positional parameter values.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Norm instance.</returns>
        INorm Execute(string command, params object[] parameters);
        ///<summary>
        ///     Execute SQL command with named parameter values.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>Norm instance.</returns>
        INorm Execute(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Execute SQL command with named parameter values and DbType type for each parameter.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>Norm instance.</returns>
        INorm Execute(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Execute SQL command with named parameter values and custom type for each parameter.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Norm instance.</returns>
        INorm Execute(string command, params (string name, object value, object type)[] parameters);
    }
}