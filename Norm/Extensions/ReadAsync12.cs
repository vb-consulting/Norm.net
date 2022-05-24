﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command);
        }

        ///<summary>
        ///     Maps command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this DbConnection connection, 
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, readerCallback);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, FormattableString command)
        {
            return connection.GetNoOrmInstance().ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this DbConnection connection, 
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().ReadFormatAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, readerCallback);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, string command, object parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, parameters);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of twelve value tuples (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).</returns>
        public static IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this DbConnection connection, 
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            object parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(command, readerCallback, parameters);
        }
    }
}