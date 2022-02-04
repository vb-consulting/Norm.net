using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type),
                GetFieldValue<T3>(r, 2, t3.type)));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        public IEnumerable<(T1, T2, T3)> ReadFormat<T1, T2, T3>(FormattableString command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadToArrayInternal(command).Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type),
                GetFieldValue<T3>(r, 2, t3.type)));
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params object[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return ReadToArrayInternal(command, parameters).MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadToArrayInternal(command, parameters).Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type),
                GetFieldValue<T3>(r, 2, t3.type)), parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return ReadToArrayInternal(command, parameters).MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadToArrayInternal(command, parameters).Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            return ReadInternal(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type),
                GetFieldValue<T3>(r, 2, t3.type)), parameters);
        }

        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of three value tuples (T1, T2, T3).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of three value tuples (T1, T2, T3).</returns>
        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value, object type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple)
            {
                return ReadToArrayInternalUnknowParamsType(command, parameters).MapValueTuple<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple)
            {
                return ReadToArrayInternalUnknowParamsType(command, parameters).Map<T1, T2, T3>(t1.type, t2.type, t3.type);
            }
            return ReadInternalUnknowParamsType(command, r => (
                GetFieldValue<T1>(r, 0, t1.type),
                GetFieldValue<T2>(r, 1, t2.type),
                GetFieldValue<T3>(r, 2, t3.type)), parameters);
        }
    }
}