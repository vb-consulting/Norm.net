using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().ReadAsync<T1, T2>(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public static IAsyncEnumerable<(T1, T2)> ReadFormatAsync<T1, T2>(this DbConnection connection, FormattableString command)
        {
            return connection.GetNoOrmInstance().ReadFormatAsync<T1, T2>(command);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IAsyncEnumerable async enumerator of two value tuples (T1, T2).</returns>
        public static IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(this DbConnection connection, string command, params object[] parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync<T1, T2>(command, parameters);
        }
    }
}