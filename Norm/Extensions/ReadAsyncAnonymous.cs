using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, T anonymousBlueprintInstance, string command) where T : class
        {
            return connection.GetNoOrmInstance().ReadAsync(anonymousBlueprintInstance, command);
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, T anonymousBlueprintInstance, string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback) where T : class
        {
            return connection.GetNoOrmInstance().ReadAsync<T>(anonymousBlueprintInstance, command, readerCallback);
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadFormatAsync<T>(this DbConnection connection, T anonymousBlueprintInstance, FormattableString command) where T : class
        {
            return connection.GetNoOrmInstance().ReadFormatAsync<T>(anonymousBlueprintInstance, command);
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadFormatAsync<T>(this DbConnection connection, T anonymousBlueprintInstance,
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback) where T : class
        {
            return connection.GetNoOrmInstance().ReadFormatAsync<T>(anonymousBlueprintInstance, command, readerCallback);
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, T anonymousBlueprintInstance, string command, params object[] parameters) where T : class
        {
            return connection.GetNoOrmInstance().ReadAsync<T>(anonymousBlueprintInstance, command, parameters);
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, T anonymousBlueprintInstance, string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters) where T : class
        {
            return connection.GetNoOrmInstance().ReadAsync<T>(anonymousBlueprintInstance, command, readerCallback, parameters);
        }
    }
}