using System;
using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        public IAsyncEnumerable<T> ReadAsync<T>(string command)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return ReadAsync(command).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return ReadAsync(command).Map<T>(type);
            }

            return ReadInternalAsync(command, async r => await r.GetFieldValueAsync<T>(0, convertsDbNull));
        }

        public IAsyncEnumerable<T> ReadFormatAsync<T>(FormattableString command)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return ReadFormatAsync(command).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return ReadFormatAsync(command).Map<T>(type);
            }

            return ReadInternalAsync(command, async r => await r.GetFieldValueAsync<T>(0, convertsDbNull));
        }

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params object[] parameters)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return ReadAsync(command, parameters).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return ReadAsync(command, parameters).Map<T>(type);
            }

            return ReadInternalAsync(command, async r => await r.GetFieldValueAsync<T>(0, convertsDbNull), parameters);
        }

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value)[] parameters)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return ReadAsync(command, parameters).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return ReadAsync(command, parameters).Map<T>(type);
            }

            return ReadInternalAsync(command, async r => await r.GetFieldValueAsync<T>(0, convertsDbNull), parameters);
        }

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, DbType type)[] parameters)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return ReadAsync(command, parameters).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return ReadAsync(command, parameters).Map<T>(type);
            }

            return ReadInternalAsync(command, async r => await r.GetFieldValueAsync<T>(0, convertsDbNull), parameters);
        }

        public IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value, object type)[] parameters)
        {
            var (type, simple, valueTuple) = TypeCache<T>.GetMetadata();
            if (valueTuple)
            {
                return ReadAsync(command, parameters).MapValueTuple<T>(type);
            }
            if (!simple)
            {
                return ReadAsync(command, parameters).Map<T>(type);
            }

            return ReadInternalUnknownParamsTypeAsync(command, async r => await r.GetFieldValueAsync<T>(0, convertsDbNull), parameters);
        }
    }
}