using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().Read(command);
        }

        ///<summary>
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, 
            string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().Read(command, readerCallback);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IEnumerable<(string name, object value)[]> ReadFormat(this DbConnection connection, FormattableString command)
        {
            return connection.GetNoOrmInstance().ReadFormat(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IEnumerable<(string name, object value)[]> ReadFormat(this DbConnection connection, 
            FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.GetNoOrmInstance().ReadFormat(command, readerCallback);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, string command, 
            params object[] parameters)
        {
            return connection.GetNoOrmInstance().Read(command, parameters);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters)
        {
            return connection.GetNoOrmInstance().Read(command, readerCallback, parameters);
        }
    }
}