using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Norm.Extensions
{
    public static partial class NormExtensions
    {
        private static readonly ConcurrentDictionary<int, IDictionary<int, Delegate>> TypeCache = new ConcurrentDictionary<int, IDictionary<int, Delegate>>();
        private static readonly ConcurrentDictionary<int, HashSet<int>> NullableCache = new ConcurrentDictionary<int, HashSet<int>>();

        private static void SetNullableValue<TInst, TProp>(
            TInst instance,
            string propertyName,
            TProp value, 
            IReflect instanceType, 
            IDictionary<int, Delegate> typeDict,
            HashSet<int> nullableHash,
            int index)
            where TProp : struct
        {
            if (typeDict.TryGetValue(index, out var cachedSetter))
            {
                if (!nullableHash.Contains(index))
                {
                    ((Action<TInst, TProp>)cachedSetter).Invoke(instance, value);
                }
                else
                {
                    ((Action<TInst, TProp?>)cachedSetter).Invoke(instance, value);
                }
                return;
            }
            var propertyInfo = instanceType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (propertyInfo == null)
            {
                return;
            }
            var reflectionSetter = propertyInfo.GetSetMethod(true);
            var nullable = Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;
            if (!nullable)
            {
                var setter = Delegate.CreateDelegate(typeof(Action<TInst, TProp>), reflectionSetter);
                typeDict.Add(index, setter);
                ((Action<TInst, TProp>)setter).Invoke(instance, value);
            }
            else
            {
                var setter = Delegate.CreateDelegate(typeof(Action<TInst, TProp?>), reflectionSetter);
                typeDict.Add(index, setter);
                nullableHash.Add(index);
                ((Action<TInst, TProp?>)setter).Invoke(instance, value);
            }
        }

        private static void SetValue<TInst, TProp>(
            TInst instance,
            string propertyName,
            TProp value,
            IReflect instanceType,
            IDictionary<int, Delegate> dict,
            int index)
        {
            if (dict.TryGetValue(index, out var cachedSetter))
            {
                ((Action<TInst, TProp>)cachedSetter).Invoke(instance, value);
                return;
            }
            var propertyInfo = instanceType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (propertyInfo == null)
            {
                return;
            }
            var reflectionSetter = propertyInfo.GetSetMethod(true);
            var setter = Delegate.CreateDelegate(typeof(Action<TInst, TProp>), reflectionSetter);
            dict.Add(index, setter);
            ((Action<TInst, TProp>) setter).Invoke(instance, value);
        }

        private static T SelectInternal<T>(
            this IList<(string name, object value)> tuples,
            T instance,
            int instanceHashCode,
            Type instanceType, 
            IDictionary<int, Delegate> typeDict,
            HashSet<int> nullableHash,
            bool isNewEntry)
        {
            foreach (var ((name, value), index) in tuples.Select((item, index) => (item, index)))
            {
                var code = Type.GetTypeCode(value.GetType());
                switch (code)
                {
                    case TypeCode.Boolean:
                        SetNullableValue(instance, name, (bool)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Byte:
                        SetNullableValue(instance, name, (byte)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Char:
                        SetNullableValue(instance, name, (char)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.DateTime:
                        SetNullableValue(instance, name, (DateTime)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Decimal:
                        SetNullableValue(instance, name, (decimal)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Double:
                        SetNullableValue(instance, name, (double)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Int16:
                        SetNullableValue(instance, name, (short)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Int32:
                        SetNullableValue(instance, name, (int)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Int64:
                        SetNullableValue(instance, name, (long)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Object:
                        SetValue(instance, name, value, instanceType, typeDict, index);
                        continue;
                    case TypeCode.SByte:
                        SetNullableValue(instance, name, (sbyte)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.Single:
                        SetNullableValue(instance, name, (float)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.String:
                        SetValue(instance, name, (string)value, instanceType, typeDict, index);
                        continue;
                    case TypeCode.UInt16:
                        SetNullableValue(instance, name, (ushort)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.UInt32:
                        SetNullableValue(instance, name, (uint)value, instanceType, typeDict, nullableHash, index);
                        continue;
                    case TypeCode.UInt64:
                        SetNullableValue(instance, name, (ulong)value, instanceType, typeDict, nullableHash, index);
                        continue;
                }
            }

            if (!isNewEntry) return instance;
            TypeCache.TryAdd(instanceHashCode, typeDict);
            NullableCache.TryAdd(instanceHashCode, nullableHash);
            return instance;
        }


        public static T Select<T>(this IList<(string name, object value)> tuples) where T : new()
        {
            var instanceType = typeof(T);
            var instanceHashCode = instanceType.GetHashCode();
            var instance = new T();

            if (TypeCache.TryGetValue(instanceHashCode, out var dict))
            {
                return tuples.SelectInternal(instance, instanceHashCode, instanceType, dict, NullableCache[instanceHashCode], false);
            }

            dict = new Dictionary<int, Delegate>();
            var nullableHash = new HashSet<int>();
            return tuples.SelectInternal(instance, instanceHashCode, instanceType, dict, nullableHash, true);
        }
        
        public static IEnumerable<T> Select<T>(this IEnumerable<IList<(string name, object value)>> tuples) where T : new()
        {
            var instanceType = typeof(T);
            var instanceHashCode = instanceType.GetHashCode();

            return tuples.Select(t =>
            {
                var instance = new T();

                if (TypeCache.TryGetValue(instanceHashCode, out var dict))
                {
                    return t.SelectInternal(instance, instanceHashCode, instanceType, dict, NullableCache[instanceHashCode], false);
                }

                dict = new Dictionary<int, Delegate>();
                var nullableHash = new HashSet<int>();
                return t.SelectInternal(instance, instanceHashCode, instanceType, dict, nullableHash, true);
            });
        }

        public static IAsyncEnumerable<T> SelectAsync<T>(this IAsyncEnumerable<IList<(string name, object value)>> tuples) where T : new()
        {
            var instanceType = typeof(T);
            var instanceHashCode = instanceType.GetHashCode();

            return tuples.Select(t =>
            {
                var instance = new T();

                if (TypeCache.TryGetValue(instanceHashCode, out var dict))
                {
                    return t.SelectInternal(instance, instanceHashCode, instanceType, dict, NullableCache[instanceHashCode], false);
                }

                dict = new Dictionary<int, Delegate>();
                var nullableHash = new HashSet<int>();
                return t.SelectInternal(instance, instanceHashCode, instanceType, dict, nullableHash, true);
            });
        }
    }
}