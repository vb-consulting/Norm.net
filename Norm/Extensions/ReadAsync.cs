using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().ReadAsync(command);
        }

        ///<summary>
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, 
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().ReadAsync(command, readerCallback);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadFormatAsync(this DbConnection connection, 
            FormattableString command)
        {
            return connection.GetNoOrmInstance().ReadFormatAsync(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadFormatAsync(this DbConnection connection,
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().ReadFormatAsync(command, readerCallback);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection, 
            string command, 
            object parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync(command, parameters);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public static IAsyncEnumerable<(string name, object value)[]> ReadAsync(this DbConnection connection,
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            object parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync(command, readerCallback, parameters);
        }
    }
}