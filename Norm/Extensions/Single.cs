using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to name and value tuple array.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Name and value tuple array.</returns>
        ///<returns>Name and value tuple array.</returns>
        public static (string name, object value)[] Single(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single(command);
        ///<summary>
        ///     Maps command results with positional parameter values to name and value tuple array.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Name and value tuple array.</returns>
        public static (string name, object value)[] Single(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to name and value tuple array.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Name and value tuple array.</returns>
        public static (string name, object value)[] Single(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to name and value tuple array.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Name and value tuple array.</returns>
        public static (string name, object value)[] Single(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to name and value tuple array.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Name and value tuple array.</returns>
        public static (string name, object value)[] Single(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single(command, parameters);
        ///<summary>
        ///     Maps command results to single value of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Single values of type T.</returns>
        public static T Single<T>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to single value of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Single values of type T.</returns>
        public static T Single<T>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to single value of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Single values of type T.</returns>
        public static T Single<T>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to single value of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Single values of type T.</returns>
        public static T Single<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to single value of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Single values of type T.</returns>
        public static T Single<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T>(command, parameters);
        ///<summary>
        ///     Maps command results to two value tuple (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Two value tuple (T1, T2).</returns>
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to two value tuple (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Two value tuple (T1, T2).</returns>
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to two value tuple (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Two value tuple (T1, T2).</returns>
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to two value tuple (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Two value tuple (T1, T2).</returns>
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to two value tuple (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Two value tuple (T1, T2).</returns>
        public static (T1, T2) Single<T1, T2>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2>(command, parameters);
        ///<summary>
        ///     Maps command results to three value tuple (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Three value tuple (T1, T2, T3).</returns>
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to three value tuple (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Three value tuple (T1, T2, T3).</returns>
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to three value tuple (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Three value tuple (T1, T2, T3).</returns>
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to three value tuple (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Three value tuple (T1, T2, T3).</returns>
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to three value tuple (T1, T2, T3).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Three value tuple (T1, T2, T3).</returns>
        public static (T1, T2, T3) Single<T1, T2, T3>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3>(command, parameters);
        ///<summary>
        ///     Maps command results to four value tuple (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Four value tuple (T1, T2, T3, T4).</returns>
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to four value tuple (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Four value tuple (T1, T2, T3, T4).</returns>
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to four value tuple (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Four value tuple (T1, T2, T3, T4).</returns>
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to four value tuple (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Four value tuple (T1, T2, T3, T4).</returns>
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to four value tuple (T1, T2, T3, T4).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Four value tuple (T1, T2, T3, T4).</returns>
        public static (T1, T2, T3, T4) Single<T1, T2, T3, T4>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4>(command, parameters);
        ///<summary>
        ///     Maps command results to five value tuple (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Five value tuple (T1, T2, T3, T4, T5).</returns>
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to five value tuple (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Five value tuple (T1, T2, T3, T4, T5).</returns>
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to five value tuple (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Five value tuple (T1, T2, T3, T4, T5).</returns>
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to five value tuple (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Five value tuple (T1, T2, T3, T4, T5).</returns>
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to five value tuple (T1, T2, T3, T4, T5).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Five value tuple (T1, T2, T3, T4, T5).</returns>
        public static (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5>(command, parameters);
        ///<summary>
        ///     Maps command results to six value tuple (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Six value tuple (T1, T2, T3, T4, T5, T6).</returns>
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to six value tuple (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Six value tuple (T1, T2, T3, T4, T5, T6).</returns>
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to six value tuple (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Six value tuple (T1, T2, T3, T4, T5, T6).</returns>
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to six value tuple (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Six value tuple (T1, T2, T3, T4, T5, T6).</returns>
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to six value tuple (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Six value tuple (T1, T2, T3, T4, T5, T6).</returns>
        public static (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6>(command, parameters);
        ///<summary>
        ///     Maps command results to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Seven value tuple (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Seven value tuple (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Seven value tuple (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Seven value tuple (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to seven value tuple (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Seven value tuple (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        ///<summary>
        ///     Maps command results to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Eight value tuple (T1, T2, T3, T4, T5, T6, T7, T8).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8>(command, parameters);
        ///<summary>
        ///     Maps command results to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Nine value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(command, parameters);
        ///<summary>
        ///     Maps command results to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Ten value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(command, parameters);
        ///<summary>
        ///     Maps command results to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Eleven value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(command, parameters);
        ///<summary>
        ///     Maps command results to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command);
        ///<summary>
        ///     Maps command results with positional parameter values to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params object[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>Twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>Twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Twelve value tuple (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters) =>
            connection.GetNoOrmInstance().Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
    }
}