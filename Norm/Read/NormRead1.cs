using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<T> Read<T>(string command)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return Read(command).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return Read(command).Map<T>(type);
            }

            return ReadInternal(command, r => r.GetFieldValue<T>(0, convertsDbNull));
        }

        public IEnumerable<T> ReadFormat<T>(FormattableString command)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return ReadFormat(command).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return ReadFormat(command).Map<T>(type);
            }

            return ReadInternal(command, r => r.GetFieldValue<T>(0, convertsDbNull));
        }

        public IEnumerable<T> Read<T>(string command, params object[] parameters)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return Read(command, parameters).Map<T>(type);
            }

            return ReadInternal(command, r => r.GetFieldValue<T>(0, convertsDbNull), parameters);
        }

        public IEnumerable<T> Read<T>(string command, params (string name, object value)[] parameters)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return Read(command, parameters).Map<T>(type);
            }

            return ReadInternal(command, r => r.GetFieldValue<T>(0, convertsDbNull), parameters);
        }

        public IEnumerable<T> Read<T>(string command, params (string name, object value, DbType type)[] parameters)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return Read(command, parameters).Map<T>(type);
            }

            return ReadInternal(command, r => r.GetFieldValue<T>(0, convertsDbNull), parameters);
        }

        public IEnumerable<T> Read<T>(string command, params (string name, object value, object type)[] parameters)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return Read(command, parameters).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return Read(command, parameters).Map<T>(type);
            }

            return ReadInternalUnknowParamsType(command, r => r.GetFieldValue<T>(0, convertsDbNull), parameters);
        }
    }
}