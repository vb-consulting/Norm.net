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
        ///Maps command results to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return connection.GetNormInstance().ReadAsync<T1, T2, T3, T4, T5, T6>(command, memberName, sourceFilePath, sourceLineNumber);
        }

        ///<summary>
        ///Parse interpolated (formattable) command as database parameters and map command results to async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadFormatAsync<T1, T2, T3, T4, T5, T6>(this DbConnection connection, FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return connection.GetNormInstance().ReadFormatAsync<T1, T2, T3, T4, T5, T6>(command, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}