using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IEnumerable<T> Read<T>(this DbConnection connection, T anonymousBlueprintInstance, string command) where T : class
        {
            return connection.GetNoOrmInstance().Read(anonymousBlueprintInstance, command);
        }

        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public static IEnumerable<T> Read<T>(this DbConnection connection, T anonymousBlueprintInstance, string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback) where T : class
        {
            return connection.GetNoOrmInstance().Read<T>(anonymousBlueprintInstance, command, readerCallback);
        }

        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public static IEnumerable<T> ReadFormat<T>(this DbConnection connection, T anonymousBlueprintInstance, FormattableString command) where T : class
        {
            return connection.GetNoOrmInstance().ReadFormat<T>(anonymousBlueprintInstance, command);
        }

        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public static IEnumerable<T> ReadFormat<T>(this DbConnection connection, T anonymousBlueprintInstance,
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback) where T : class
        {
            return connection.GetNoOrmInstance().ReadFormat<T>(anonymousBlueprintInstance, command, readerCallback);
        }

        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public static IEnumerable<T> Read<T>(this DbConnection connection, T anonymousBlueprintInstance, string command, params object[] parameters) where T : class
        {
            return connection.GetNoOrmInstance().Read<T>(anonymousBlueprintInstance, command, parameters);
        }

        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public static IEnumerable<T> Read<T>(this DbConnection connection, T anonymousBlueprintInstance, string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters) where T : class
        {
            return connection.GetNoOrmInstance().Read<T>(anonymousBlueprintInstance, command, readerCallback, parameters);
        }
    }
}