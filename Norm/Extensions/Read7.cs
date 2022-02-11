using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command);
        }

        ///<summary>
        ///     Maps command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, 
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command, readerCallback);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadFormat<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, 
            FormattableString command)
        {
            return connection.GetNoOrmInstance().ReadFormat<T1, T2, T3, T4, T5, T6, T7>(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadFormat<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection,
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().ReadFormat<T1, T2, T3, T4, T5, T6, T7>(command, readerCallback);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection, 
            string command, 
            params object[] parameters)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command, parameters);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of seven value tuples (T1, T2, T3, T4, T5, T6, T7).</returns>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(this DbConnection connection,
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters)
        {
            return connection.GetNoOrmInstance().Read<T1, T2, T3, T4, T5, T6, T7>(command, readerCallback, parameters);
        }
    }
}