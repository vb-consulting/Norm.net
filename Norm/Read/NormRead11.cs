﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return Read(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return Read(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
            {
                return ReadInternal(command, r => (
                   r.GetFieldValue<T1>(0, convertsDbNull),
                   r.GetFieldValue<T2>(1, convertsDbNull),
                   r.GetFieldValue<T3>(2, convertsDbNull),
                   r.GetFieldValue<T4>(3, convertsDbNull),
                   r.GetFieldValue<T5>(4, convertsDbNull),
                   r.GetFieldValue<T6>(5, convertsDbNull),
                   r.GetFieldValue<T7>(6, convertsDbNull),
                   r.GetFieldValue<T8>(7, convertsDbNull),
                   r.GetFieldValue<T9>(8, convertsDbNull),
                   r.GetFieldValue<T10>(9, convertsDbNull),
                   r.GetFieldValue<T11>(10, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            FormattableString command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return ReadFormat(command).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return ReadFormat(command).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
            {
                return ReadInternal(command, r => (
                   r.GetFieldValue<T1>(0, convertsDbNull),
                   r.GetFieldValue<T2>(1, convertsDbNull),
                   r.GetFieldValue<T3>(2, convertsDbNull),
                   r.GetFieldValue<T4>(3, convertsDbNull),
                   r.GetFieldValue<T5>(4, convertsDbNull),
                   r.GetFieldValue<T6>(5, convertsDbNull),
                   r.GetFieldValue<T7>(6, convertsDbNull),
                   r.GetFieldValue<T8>(7, convertsDbNull),
                   r.GetFieldValue<T9>(8, convertsDbNull),
                   r.GetFieldValue<T10>(9, convertsDbNull),
                   r.GetFieldValue<T11>(10, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params object[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return Read(command, parameters).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
            {
                return ReadInternal(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull),
                    r.GetFieldValue<T3>(2, convertsDbNull),
                    r.GetFieldValue<T4>(3, convertsDbNull),
                    r.GetFieldValue<T5>(4, convertsDbNull),
                    r.GetFieldValue<T6>(5, convertsDbNull),
                    r.GetFieldValue<T7>(6, convertsDbNull),
                    r.GetFieldValue<T8>(7, convertsDbNull),
                    r.GetFieldValue<T9>(8, convertsDbNull),
                    r.GetFieldValue<T10>(9, convertsDbNull),
                    r.GetFieldValue<T11>(10, convertsDbNull)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return Read(command, parameters).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
            {
                return ReadInternal(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull),
                    r.GetFieldValue<T3>(2, convertsDbNull),
                    r.GetFieldValue<T4>(3, convertsDbNull),
                    r.GetFieldValue<T5>(4, convertsDbNull),
                    r.GetFieldValue<T6>(5, convertsDbNull),
                    r.GetFieldValue<T7>(6, convertsDbNull),
                    r.GetFieldValue<T8>(7, convertsDbNull),
                    r.GetFieldValue<T9>(8, convertsDbNull),
                    r.GetFieldValue<T10>(9, convertsDbNull),
                    r.GetFieldValue<T11>(10, convertsDbNull)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value, DbType type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return Read(command, parameters).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
            {
                return ReadInternal(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull),
                    r.GetFieldValue<T3>(2, convertsDbNull),
                    r.GetFieldValue<T4>(3, convertsDbNull),
                    r.GetFieldValue<T5>(4, convertsDbNull),
                    r.GetFieldValue<T6>(5, convertsDbNull),
                    r.GetFieldValue<T7>(6, convertsDbNull),
                    r.GetFieldValue<T8>(7, convertsDbNull),
                    r.GetFieldValue<T9>(8, convertsDbNull),
                    r.GetFieldValue<T10>(9, convertsDbNull),
                    r.GetFieldValue<T11>(10, convertsDbNull)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value, object type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            var t3 = TypeCache<T3>.GetMetadata();
            var t4 = TypeCache<T4>.GetMetadata();
            var t5 = TypeCache<T5>.GetMetadata();
            var t6 = TypeCache<T6>.GetMetadata();
            var t7 = TypeCache<T7>.GetMetadata();
            var t8 = TypeCache<T8>.GetMetadata();
            var t9 = TypeCache<T9>.GetMetadata();
            var t10 = TypeCache<T10>.GetMetadata();
            var t11 = TypeCache<T11>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple && t3.valueTuple && t4.valueTuple && t5.valueTuple && t6.valueTuple && t7.valueTuple && t8.valueTuple && t9.valueTuple && t10.valueTuple && t11.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (!t1.simple && !t2.simple && !t3.simple && !t4.simple && !t5.simple && !t6.simple && !t7.simple && !t8.simple && !t9.simple && !t10.simple && !t11.simple)
            {
                return Read(command, parameters).Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1.type, t2.type, t3.type, t4.type, t5.type, t6.type, t7.type, t8.type, t9.type, t10.type, t11.type);
            }
            else if (t1.simple && t2.simple && t3.simple && t4.simple && t5.simple && t6.simple && t7.simple && t8.simple && t9.simple && t10.simple && t11.simple)
            {
                return ReadInternalUnknowParamsType(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull),
                    r.GetFieldValue<T3>(2, convertsDbNull),
                    r.GetFieldValue<T4>(3, convertsDbNull),
                    r.GetFieldValue<T5>(4, convertsDbNull),
                    r.GetFieldValue<T6>(5, convertsDbNull),
                    r.GetFieldValue<T7>(6, convertsDbNull),
                    r.GetFieldValue<T8>(7, convertsDbNull),
                    r.GetFieldValue<T9>(8, convertsDbNull),
                    r.GetFieldValue<T10>(9, convertsDbNull),
                    r.GetFieldValue<T11>(10, convertsDbNull)), parameters);
            }
            throw new NormMultipleMappingsException();
        }
    }
}