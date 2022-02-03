using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple)
            {
                return Read(command).MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return Read(command).Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type),
                    GetFieldValue<T3>(r, 2, t3.type),
                    GetFieldValue<T4>(r, 3, t4.type),
                    GetFieldValue<T5>(r, 4, t5.type),
                    GetFieldValue<T6>(r, 5, t6.type)));
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6)> ReadFormat<T1, T2, T3, T4, T5, T6>(FormattableString command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple)
            {
                return ReadFormat(command).MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return ReadFormat(command).Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type),
                    GetFieldValue<T3>(r, 2, t3.type),
                    GetFieldValue<T4>(r, 3, t4.type),
                    GetFieldValue<T5>(r, 4, t5.type),
                    GetFieldValue<T6>(r, 5, t6.type)));
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with positional parameter values to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params object[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return Read(command, parameters).Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type),
                    GetFieldValue<T3>(r, 2, t3.type),
                    GetFieldValue<T4>(r, 3, t4.type),
                    GetFieldValue<T5>(r, 4, t5.type),
                    GetFieldValue<T6>(r, 5, t6.type)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with named parameter values to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuple array - (string name, object value).</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return Read(command, parameters).Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type),
                    GetFieldValue<T3>(r, 2, t3.type),
                    GetFieldValue<T4>(r, 3, t4.type),
                    GetFieldValue<T5>(r, 4, t5.type),
                    GetFieldValue<T6>(r, 5, t6.type)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with named parameter values and DbType type for each parameter to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuple array - (string name, object value, DbType type).</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return Read(command, parameters).Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternal(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type),
                    GetFieldValue<T3>(r, 2, t3.type),
                    GetFieldValue<T4>(r, 3, t4.type),
                    GetFieldValue<T5>(r, 4, t5.type),
                    GetFieldValue<T6>(r, 5, t6.type)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        ///<summary>
        ///     Maps command results with named parameter values and custom type for each parameter to enumerator of six value tuples (T1, T2, T3, T4, T5, T6).
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuple array - (string name, object value, object type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>IEnumerable enumerator of six value tuples (T1, T2, T3, T4, T5, T6).</returns>
        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, object type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple)
            {
                return Read(command, parameters).Map<T1, T2, T3, T4, T5, T6>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple)
            {
                return ReadInternalUnknowParamsType(command, r => (
                    GetFieldValue<T1>(r, 0, t1.type),
                    GetFieldValue<T2>(r, 1, t2.type),
                    GetFieldValue<T3>(r, 2, t3.type),
                    GetFieldValue<T4>(r, 3, t4.type),
                    GetFieldValue<T5>(r, 4, t5.type),
                    GetFieldValue<T6>(r, 5, t6.type)), parameters);
            }
            throw new NormMultipleMappingsException();
        }
    }
}