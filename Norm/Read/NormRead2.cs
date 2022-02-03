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
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type), 
                    GetFieldValue<T2>(r, 1, t2.type)));
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternal(command, readerCallback).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                    GetFieldValue<T2>(r, 1, t2.type, readerCallback)));
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> ReadFormat<T1, T2>(FormattableString command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadFormat(command).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadFormat(command).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type)));
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> ReadFormat<T1, T2>(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadFormat(command, readerCallback).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadFormat(command, readerCallback).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                    GetFieldValue<T2>(r, 1, t2.type, readerCallback)));
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternal(command, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternal(command, readerCallback, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                    GetFieldValue<T2>(r, 1, t2.type, readerCallback)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with named parameter values to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternal(command, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with named parameter values to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params (string name, object value)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternal(command, readerCallback, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                    GetFieldValue<T2>(r, 1, t2.type, readerCallback)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, object type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternalUnknowParamsType(command, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternalUnknowParamsType(command, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternalUnknowParamsType(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of two value tuples (T1, T2).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of two value tuples (T1, T2).</returns>
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params (string name, object value, object type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return ReadToArrayInternalUnknowParamsType(command, readerCallback, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return ReadToArrayInternalUnknowParamsType(command, readerCallback, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternalUnknowParamsType(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type, readerCallback),
                    GetFieldValue<T2>(r, 1, t2.type, readerCallback)), parameters);
            }
            throw new NormMultipleMappingsException();
        }
    }
}