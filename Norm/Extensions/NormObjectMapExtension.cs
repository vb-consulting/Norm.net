using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Norm.Extensions
{
    public static partial class NormExtensions
    {
        private static readonly ConcurrentDictionary<int, IDictionary<byte, Delegate>> TypeCache = new ConcurrentDictionary<int, IDictionary<byte, Delegate>>();
        private static readonly ConcurrentDictionary<int, HashSet<byte>> NullableCache = new ConcurrentDictionary<int, HashSet<byte>>();

        private static T SelectInternal<T>(
            this IEnumerable<(string name, object value)> tuples,
            T instance,
            int instanceHashCode,
            IReflect instanceType, 
            IDictionary<byte, Delegate> typeDict,
            HashSet<byte> nullableHash,
            bool isNewEntry)
        {
            foreach (var ((name, value), index) in tuples.Select((item, index) => (item, (byte)index)))
            {
                void SetNullableValue<TProp>(TProp propertyValue) where TProp : struct
                {
                    if (typeDict.TryGetValue(index, out var cachedSetter))
                    {
                        if (!nullableHash.Contains(index))
                        {
                            ((Action<T, TProp>)cachedSetter).Invoke(instance, propertyValue);
                            return;
                        }
                        ((Action<T, TProp?>)cachedSetter).Invoke(instance, propertyValue);
                        return;
                    }

                    var propertyInfo = instanceType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (propertyInfo == null)
                    {
                        return;
                    }

                    var reflectionSetter = propertyInfo.GetSetMethod(true);
                    var nullable = Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;
                    if (!nullable)
                    {
                        var setter = Delegate.CreateDelegate(typeof(Action<T, TProp>), reflectionSetter);
                        typeDict.Add(index, setter);
                        ((Action<T, TProp>)setter).Invoke(instance, propertyValue);
                    }
                    else
                    {
                        var setter = Delegate.CreateDelegate(typeof(Action<T, TProp?>), reflectionSetter);
                        typeDict.Add(index, setter);
                        nullableHash.Add(index);
                        ((Action<T, TProp?>)setter).Invoke(instance, propertyValue);
                    }
                }

                void SetStringValue(ReadOnlySpan<char> propertyValue)
                {
                    if (typeDict.TryGetValue(index, out var cachedSetter))
                    {
                        ((Action<T, string>)cachedSetter).Invoke(instance, propertyValue.ToString());
                        return;
                    }
                    var propertyInfo = instanceType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (propertyInfo == null)
                    {
                        return;
                    }
                    var reflectionSetter = propertyInfo.GetSetMethod(true);
                    var setter = Delegate.CreateDelegate(typeof(Action<T, string>), reflectionSetter);
                    typeDict.Add(index, setter);
                    ((Action<T, string>)setter).Invoke(instance, propertyValue.ToString());
                }

                var code = Type.GetTypeCode(value.GetType());
                switch (code)
                {
                    case TypeCode.Boolean:
                        SetNullableValue((bool)value);
                        continue;
                    case TypeCode.Byte:
                        SetNullableValue((byte)value);
                        continue;
                    case TypeCode.Char:
                        SetNullableValue((char)value);
                        continue;
                    case TypeCode.DateTime:
                        SetNullableValue((DateTime)value);
                        continue;
                    case TypeCode.Decimal:
                        SetNullableValue((decimal)value);
                        continue;
                    case TypeCode.Double:
                        SetNullableValue((double)value);
                        continue;
                    case TypeCode.Int16:
                        SetNullableValue((short)value);
                        continue;
                    case TypeCode.Int32:
                        SetNullableValue((int)value);
                        continue;
                    case TypeCode.Int64:
                        SetNullableValue((long)value);
                        continue;
                    case TypeCode.SByte:
                        SetNullableValue((sbyte)value);
                        continue;
                    case TypeCode.Single:
                        SetNullableValue((float)value);
                        continue;
                    case TypeCode.String:
                        SetStringValue((string) value);
                        continue;
                    case TypeCode.UInt16:
                        SetNullableValue((ushort)value);
                        continue;
                    case TypeCode.UInt32:
                        SetNullableValue((uint)value);
                        continue;
                    case TypeCode.UInt64:
                        SetNullableValue((ulong)value);
                        continue;
                }
            }

            if (!isNewEntry) return instance;
            TypeCache.TryAdd(instanceHashCode, typeDict);
            NullableCache.TryAdd(instanceHashCode, nullableHash);
            return instance;
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

                dict = new Dictionary<byte, Delegate>();
                var nullableHash = new HashSet<byte>();
                return t.SelectInternal(instance, instanceHashCode, instanceType, dict, nullableHash, true);
            });
        }

        public static async IAsyncEnumerable<T> Select<T>(this IAsyncEnumerable<IList<(string name, object value)>> tuples) where T : new()
        {
            var instanceType = typeof(T);
            var instanceHashCode = instanceType.GetHashCode();

            await foreach (var t in tuples)
            {
                var instance = new T();

                if (TypeCache.TryGetValue(instanceHashCode, out var dict))
                {
                    yield return t.SelectInternal(instance, instanceHashCode, instanceType, dict, NullableCache[instanceHashCode], false);
                    continue;
                }

                dict = new Dictionary<byte, Delegate>();
                var nullableHash = new HashSet<byte>();
                yield return t.SelectInternal(instance, instanceHashCode, instanceType, dict, nullableHash, true);
            }
        }
    }
}