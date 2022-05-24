using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

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
        public static IAsyncEnumerable<T> ReadAnonymousAsync<T>(this DbConnection connection, T anonymousBlueprintInstance, string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where T : class
        {
            return connection.GetNoOrmInstance().ReadAnonymousAsync(anonymousBlueprintInstance, command);
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAnonymousAsync<T>(this DbConnection connection, T anonymousBlueprintInstance, string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where T : class
        {
            return connection.GetNoOrmInstance().ReadAnonymousAsync<T>(anonymousBlueprintInstance, command, readerCallback);
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAnonymousFormatAsync<T>(this DbConnection connection, T anonymousBlueprintInstance, FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where T : class
        {
            return connection.GetNoOrmInstance().ReadAnonymousFormatAsync<T>(anonymousBlueprintInstance, command);
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAnonymousFormatAsync<T>(this DbConnection connection, T anonymousBlueprintInstance,
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where T : class
        {
            return connection.GetNoOrmInstance().ReadAnonymousFormatAsync<T>(anonymousBlueprintInstance, command, readerCallback);
        }
    }
}