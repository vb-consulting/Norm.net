using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command)
        {
            return ReadToArrayInternal(command);
        }

        ///<summary>
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command, 
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return ReadToArrayInternal(command, readerCallback);
        }

        ///<summary>
        ///      Parse interpolated (formattable) command as database parameters and map results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> ReadFormat(FormattableString command)
        {
            return ReadToArrayInternal(command);
        }

        ///<summary>
        ///      Parse interpolated (formattable) command as database parameters and map results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> ReadFormat(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return ReadToArrayInternal(command, readerCallback);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command, params object[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters)
        {
            return ReadToArrayInternal(command, readerCallback, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value)[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params (string name, object value)[] parameters)
        {
            return ReadToArrayInternal(command, readerCallback, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadToArrayInternal(command, readerCallback, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadToArrayInternalUnknowParamsType(command, parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params (string name, object value, object type)[] parameters)
        {
            return ReadToArrayInternalUnknowParamsType(command, readerCallback, parameters);
        }
    }
}