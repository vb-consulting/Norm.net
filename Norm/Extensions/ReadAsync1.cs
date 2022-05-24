using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().ReadAsync<T>(command);
        }

        ///<summary>
        ///     Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, 
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().ReadAsync<T>(command, readerCallback);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadFormatAsync<T>(this DbConnection connection, 
            FormattableString command)
        {
            return connection.GetNoOrmInstance().ReadFormatAsync<T>(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public static IAsyncEnumerable<T> ReadFormatAsync<T>(this DbConnection connection, 
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().ReadFormatAsync<T>(command, readerCallback);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection, 
            string command, 
            object parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync<T>(command, parameters);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>.
        public static IAsyncEnumerable<T> ReadAsync<T>(this DbConnection connection,
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            object parameters)
        {
            return connection.GetNoOrmInstance().ReadAsync<T>(command, readerCallback, parameters);
        }
    }
}