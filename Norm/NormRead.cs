using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<(string name, object value)[]> Read(string command)
        {
            return ReadToArrayInternal(command);
        }

        public IEnumerable<(string name, object value)[]> Read(string command, params object[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value)[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadToArrayInternalUnknowParamsType(command, parameters);
        }

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

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command)
        {
            return ReadInternal(command, r => (r.GetFieldValue<T1>(0, convertsDbNull), r.GetFieldValue<T2>(1, convertsDbNull)));
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters)
        {
            return ReadInternal(command, r => (r.GetFieldValue<T1>(0, convertsDbNull), r.GetFieldValue<T2>(1, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters)
        {
            return ReadInternal(command, r => (r.GetFieldValue<T1>(0, convertsDbNull), r.GetFieldValue<T2>(1, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternal(command, r => (r.GetFieldValue<T1>(0, convertsDbNull), r.GetFieldValue<T2>(1, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknowParamsType(command, r => (r.GetFieldValue<T1>(0, convertsDbNull), r.GetFieldValue<T2>(1, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params object[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknowParamsType(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull)),
                parameters);
        }

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params object[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknowParamsType(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params object[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknowParamsType(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params object[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknowParamsType(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull),
                r.GetFieldValue<T2>(1, convertsDbNull),
                r.GetFieldValue<T3>(2, convertsDbNull),
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params object[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknowParamsType(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull), 
                r.GetFieldValue<T8>(7, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params object[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull), 
                r.GetFieldValue<T8>(7, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params (string name, object value)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull), 
                r.GetFieldValue<T8>(7, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadInternal(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull), 
                r.GetFieldValue<T8>(7, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadInternalUnknowParamsType(command, r => (
                r.GetFieldValue<T1>(0, convertsDbNull), 
                r.GetFieldValue<T2>(1, convertsDbNull), 
                r.GetFieldValue<T3>(2, convertsDbNull), 
                r.GetFieldValue<T4>(3, convertsDbNull),
                r.GetFieldValue<T5>(4, convertsDbNull),
                r.GetFieldValue<T6>(5, convertsDbNull), 
                r.GetFieldValue<T7>(6, convertsDbNull), 
                r.GetFieldValue<T8>(7, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command)
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
                r.GetFieldValue<T9>(8, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params object[] parameters)
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
                r.GetFieldValue<T9>(8, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value)[] parameters)
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
                r.GetFieldValue<T9>(8, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, DbType type)[] parameters)
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
                r.GetFieldValue<T9>(8, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, object type)[] parameters)
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
                r.GetFieldValue<T9>(8, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command)
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
                r.GetFieldValue<T10>(9, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params object[] parameters)
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
                r.GetFieldValue<T10>(9, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params (string name, object value)[] parameters)
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
                r.GetFieldValue<T10>(9, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params (string name, object value, DbType type)[] parameters)
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
                r.GetFieldValue<T10>(9, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params (string name, object value, object type)[] parameters)
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
                r.GetFieldValue<T10>(9, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command)
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

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params object[] parameters)
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

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value)[] parameters)
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

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value, DbType type)[] parameters)
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

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value, object type)[] parameters)
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

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command)
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
                r.GetFieldValue<T11>(10, convertsDbNull), 
                r.GetFieldValue<T12>(11, convertsDbNull)));
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params object[] parameters)
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
                r.GetFieldValue<T11>(10, convertsDbNull), 
                r.GetFieldValue<T12>(11, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params (string name, object value)[] parameters)
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
                r.GetFieldValue<T11>(10, convertsDbNull),
                r.GetFieldValue<T12>(11, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params (string name, object value, DbType type)[] parameters)
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
                r.GetFieldValue<T11>(10, convertsDbNull),
                r.GetFieldValue<T12>(11, convertsDbNull)), parameters);
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params (string name, object value, object type)[] parameters)
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
                r.GetFieldValue<T11>(10, convertsDbNull),
                r.GetFieldValue<T12>(11, convertsDbNull)), parameters);
        }
    }
}