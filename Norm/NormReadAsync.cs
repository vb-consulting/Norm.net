using System;
using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command)
        {
            return ReadToArrayInternalAsync(command);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params object[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value)[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadToArrayInternalUnknownParamsTypeAsync(command, parameters);
        }

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

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command)
        {
            return ReadInternalAsync(command, async r => (await r.GetFieldValueAsync<T1>(0, convertsDbNull), await r.GetFieldValueAsync<T2>(1, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (await r.GetFieldValueAsync<T1>(0, convertsDbNull), await r.GetFieldValueAsync<T2>(1, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (await r.GetFieldValueAsync<T1>(0, convertsDbNull), await r.GetFieldValueAsync<T2>(1, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (await r.GetFieldValueAsync<T1>(0, convertsDbNull), await r.GetFieldValueAsync<T2>(1, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (await r.GetFieldValueAsync<T1>(0, convertsDbNull), await r.GetFieldValueAsync<T2>(1, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4)>
            ReadAsync<T1, T2, T3, T4>(string command, params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command,
          params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
            params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull), 
                await r.GetFieldValueAsync<T3>(2, convertsDbNull), 
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command,
            params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command)
        {
            return ReadInternalAsync(command, async r => (
               await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
               await r.GetFieldValueAsync<T2>(1, convertsDbNull),
               await r.GetFieldValueAsync<T3>(2, convertsDbNull),
               await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
               await r.GetFieldValueAsync<T5>(4, convertsDbNull),
               await r.GetFieldValueAsync<T6>(5, convertsDbNull),
               await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
               await r.GetFieldValueAsync<T8>(7, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            string command, params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command)
        {
            return ReadInternalAsync(command, async r => (
               await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
               await r.GetFieldValueAsync<T2>(1, convertsDbNull),
               await r.GetFieldValueAsync<T3>(2, convertsDbNull),
               await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
               await r.GetFieldValueAsync<T5>(4, convertsDbNull),
               await r.GetFieldValueAsync<T6>(5, convertsDbNull),
               await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
               await r.GetFieldValueAsync<T8>(7, convertsDbNull),
               await r.GetFieldValueAsync<T9>(8, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command, params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command, params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command, params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(string command, params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(string command, params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(string command, params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(string command, params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(string command, params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull), 
                await r.GetFieldValueAsync<T12>(11, convertsDbNull)));
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(string command, params object[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull), 
                await r.GetFieldValueAsync<T12>(11, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(string command, params (string name, object value)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull), 
                await r.GetFieldValueAsync<T12>(11, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternalAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull), 
                await r.GetFieldValueAsync<T12>(11, convertsDbNull)), parameters);
        }

        public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(string command, params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknownParamsTypeAsync(command, async r => (
                await r.GetFieldValueAsync<T1>(0, convertsDbNull), 
                await r.GetFieldValueAsync<T2>(1, convertsDbNull),
                await r.GetFieldValueAsync<T3>(2, convertsDbNull),
                await r.GetFieldValueAsync<T4>(3, convertsDbNull), 
                await r.GetFieldValueAsync<T5>(4, convertsDbNull),
                await r.GetFieldValueAsync<T6>(5, convertsDbNull),
                await r.GetFieldValueAsync<T7>(6, convertsDbNull), 
                await r.GetFieldValueAsync<T8>(7, convertsDbNull),
                await r.GetFieldValueAsync<T9>(8, convertsDbNull),
                await r.GetFieldValueAsync<T10>(9, convertsDbNull), 
                await r.GetFieldValueAsync<T11>(10, convertsDbNull), 
                await r.GetFieldValueAsync<T12>(11, convertsDbNull)), parameters);
        }
    }
}