using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<(T1, T2)> Read<T1, T2>(string command)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return Read(command).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return Read(command).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull), 
                    r.GetFieldValue<T2>(1, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

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
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull)));
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return Read(command, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return Read(command, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, DbType type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return Read(command, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternal(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull)), parameters);
            }
            throw new NormMultipleMappingsException();
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, object type)[] parameters)
        {
            var t1 = TypeCache<T1>.GetMetadata();
            var t2 = TypeCache<T2>.GetMetadata();
            if (t1.valueTuple && t2.valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T1, T2>(t1.type, t2.type);
            }
            else if (!t1.simple && !t2.simple)
            {
                return Read(command, parameters).Map<T1, T2>(t1.type, t2.type);
            }
            else if (t1.simple && t2.simple)
            {
                return ReadInternalUnknowParamsType(command, r => (
                    r.GetFieldValue<T1>(0, convertsDbNull),
                    r.GetFieldValue<T2>(1, convertsDbNull)), parameters);
            }
            throw new NormMultipleMappingsException();
        }
    }
}