using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public static IEnumerable<(T1, T2, T3, T4)> ReadFormat<T1, T2, T3, T4>(this DbConnection connection, FormattableString command)
        {
            return connection.GetNoOrmInstance().ReadFormat<T1, T2, T3, T4>(command);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of four value tuples (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of four value tuples (T1, T2, T3, T4).</returns>
        public static IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4>(command, parameters);
        }
    }
}