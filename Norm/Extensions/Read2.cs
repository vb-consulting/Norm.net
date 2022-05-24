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
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return connection.GetNoOrmInstance().Read<T1, T2>(command, memberName, sourceFilePath, sourceLineNumber);
        }

        ///<summary>
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return connection.GetNoOrmInstance().Read<T1, T2>(command, readerCallback, memberName, sourceFilePath, sourceLineNumber);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public static IEnumerable<(T1, T2)> ReadFormat<T1, T2>(this DbConnection connection, FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return connection.GetNoOrmInstance().ReadFormat<T1, T2>(command, memberName, sourceFilePath, sourceLineNumber);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public static IEnumerable<(T1, T2)> ReadFormat<T1, T2>(this DbConnection connection, FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return connection.GetNoOrmInstance().ReadFormat<T1, T2>(command, readerCallback, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}