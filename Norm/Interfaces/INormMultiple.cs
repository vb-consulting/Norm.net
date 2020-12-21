using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm.Interfaces
{
    public interface INormMultiple
    {
        ///<summary>
        ///     Execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>Disposable INormMultipleReader instance.</returns>
        INormMultipleReader Multiple(string command);
        ///<summary>
        ///     Execute SQL command with positional parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Disposable INormMultipleReader instance.</returns>
        INormMultipleReader Multiple(string command, params object[] parameters);
        ///<summary>
        ///     Execute SQL command with named parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>Same DbConnection instance.</returns>
        INormMultipleReader Multiple(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Execute SQL command with named parameter values and DbType type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>Disposable INormMultipleReader instance.</returns>
        INormMultipleReader Multiple(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Execute SQL command with named parameter values and custom type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Disposable INormMultipleReader instance.</returns>
        INormMultipleReader Multiple(string command, params (string name, object value, object type)[] parameters);
        ///<summary>
        ///     Execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        ValueTask<INormMultipleReader> MultipleAsync(string command);
        ///<summary>
        ///     Execute SQL command asynchronously with positional parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        ValueTask<INormMultipleReader> MultipleAsync(string command, params object[] parameters);
        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and DbType type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and custom type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>A value task representing the asynchronous operation returning disposable INormMultipleReader instance.</returns>
        ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value, object type)[] parameters);
    }
}
