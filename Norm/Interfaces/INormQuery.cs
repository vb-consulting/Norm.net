using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormQuery
    {
        ///<summary>
        ///     Maps command results to instance enumerator.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IEnumerable enumerator of instances of type T.</returns>
        IEnumerable<T> Query<T>(string command);
        ///<summary>
        ///     Maps command results with positional parameter values to instance enumerator.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IEnumerable enumerator of instances of type T.</returns>
        IEnumerable<T> Query<T>(string command, params object[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values to instance enumerator.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IEnumerable enumerator of instances of type T.</returns>
        IEnumerable<T> Query<T>(string command, params (string name, object value)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to instance enumerator.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IEnumerable enumerator of instances of type T.</returns>
        IEnumerable<T> Query<T>(string command, params (string name, object value, DbType type)[] parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to instance enumerator.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<typeparam name="T">Type of instances that name and value tuples array will be mapped to.</typeparam>
        ///<returns>IEnumerable enumerator of instances of type T.</returns>
        IEnumerable<T> Query<T>(string command, params (string name, object value, object type)[] parameters);
    }
}