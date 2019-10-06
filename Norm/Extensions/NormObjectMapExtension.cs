using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Norm.Extensions
{
    public static partial class NormExtensions
    {
        private struct MapCacheItem
        {
            public bool Nullable { get; }
            public Delegate Setter { get; }

            public MapCacheItem(bool nullable, Delegate setter)
            {
                Nullable = nullable;
                Setter = setter;
            }
        }

        private static readonly ConcurrentDictionary<int, IDictionary<int, MapCacheItem>> MapCache = 
            new ConcurrentDictionary<int, IDictionary<int, MapCacheItem>>();

        private static void SetNullableValue<TInst, TProp>(
            TInst instance,
            string propertyName,
            TProp value, 
            Type instanceType, 
            IDictionary<int, MapCacheItem> dict,
            int index)
            where TProp : struct
        {
            if (dict.TryGetValue(index, out var item))
            {
                if (!item.Nullable)
                {
                    ((Action<TInst, TProp>)item.Setter).Invoke(instance, value);
                }
                else
                {
                    ((Action<TInst, TProp?>)item.Setter).Invoke(instance, value);
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
                dict.TryAdd(index, new MapCacheItem(false, setter));
                ((Action<TInst, TProp>)setter).Invoke(instance, value);
            }
            else
            {
                var setter = Delegate.CreateDelegate(typeof(Action<TInst, TProp?>), reflectionSetter);
                dict.TryAdd(index, new MapCacheItem(true, setter));
                ((Action<TInst, TProp?>)setter).Invoke(instance, value);
            }
        }

        private static void SetValue<TInst, TProp>(
            TInst instance,
            string propertyName,
            TProp value,
            Type instanceType,
            IDictionary<int, MapCacheItem> dict,
            int index)
        {
            if (dict.TryGetValue(index, out var item))
            {
                ((Action<TInst, TProp>)item.Setter).Invoke(instance, value);
                return;
            }
            var propertyInfo = instanceType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (propertyInfo == null)
            {
                return;
            }
            var reflectionSetter = propertyInfo.GetSetMethod(true);
            var setter = Delegate.CreateDelegate(typeof(Action<TInst, TProp>), reflectionSetter);
            dict.TryAdd(index, new MapCacheItem(false, setter));
            ((Action<TInst, TProp>) setter).Invoke(instance, value);
        }

        private static T SelectInternal<T>(
            this IList<(string name, object value)> tuples,
            T instance,
            int instanceHashCode,
            Type instanceType, 
            IDictionary<int, MapCacheItem> dict,
            bool isNewEntry)
        {
            foreach (var ((name, value), index) in tuples.Select((item, index) => (item, index)))
            {
                var code = Type.GetTypeCode(value.GetType());
                switch (code)
                {
                    case TypeCode.Boolean:
                        SetNullableValue(instance, name, (bool)value, instanceType, dict, index);
                        break;
                    case TypeCode.Byte:
                        SetNullableValue(instance, name, (byte)value, instanceType, dict, index);
                        break;
                    case TypeCode.Char:
                        SetNullableValue(instance, name, (char)value, instanceType, dict, index);
                        break;
                    case TypeCode.DateTime:
                        SetNullableValue(instance, name, (DateTime)value, instanceType, dict, index);
                        break;
                    case TypeCode.Decimal:
                        SetNullableValue(instance, name, (decimal)value, instanceType, dict, index);
                        break;
                    case TypeCode.Double:
                        SetNullableValue(instance, name, (double)value, instanceType, dict, index);
                        break;
                    case TypeCode.Int16:
                        SetNullableValue(instance, name, (short)value, instanceType, dict, index);
                        break;
                    case TypeCode.Int32:
                        SetNullableValue(instance, name, (int)value, instanceType, dict, index);
                        break;
                    case TypeCode.Int64:
                        SetNullableValue(instance, name, (long)value, instanceType, dict, index);
                        break;
                    case TypeCode.Object:
                        SetValue(instance, name, (object)value, instanceType, dict, index);
                        break;
                    case TypeCode.SByte:
                        SetNullableValue(instance, name, (sbyte)value, instanceType, dict, index);
                        break;
                    case TypeCode.Single:
                        SetNullableValue(instance, name, (float)value, instanceType, dict, index);
                        break;
                    case TypeCode.String:
                        SetValue(instance, name, (string)value, instanceType, dict, index);
                        break;
                    case TypeCode.UInt16:
                        SetNullableValue(instance, name, (ushort)value, instanceType, dict, index);
                        break;
                    case TypeCode.UInt32:
                        SetNullableValue(instance, name, (uint)value, instanceType, dict, index);
                        break;
                    case TypeCode.UInt64:
                        SetNullableValue(instance, name, (ulong)value, instanceType, dict, index);
                        break;
                    default:
                        break;
                }
            }
            if (isNewEntry)
            {
               MapCache.TryAdd(instanceHashCode, dict);
            }
            return instance;
        }


        public static T Select<T>(this IList<(string name, object value)> tuples) where T : new()
        {
            var instanceType = typeof(T);
            var instanceHashCode = instanceType.GetHashCode();
            var instance = new T();

            if (MapCache.TryGetValue(instanceHashCode, out var dict))
                return tuples.SelectInternal(instance, instanceHashCode, instanceType, dict, false);
            dict = new Dictionary<int, MapCacheItem>();
            return tuples.SelectInternal(instance, instanceHashCode, instanceType, dict, true);
        }
        
        public static IEnumerable<T> Select<T>(this IEnumerable<IList<(string name, object value)>> tuples) where T : new()
        {
            var instanceType = typeof(T);
            var instanceHashCode = instanceType.GetHashCode();

            return tuples.Select(t =>
            {
                var instance = new T();
                if (MapCache.TryGetValue(instanceHashCode, out var dict))
                    return t.SelectInternal(instance, instanceHashCode, instanceType, dict, false);
                dict = new Dictionary<int, MapCacheItem>();
                return t.SelectInternal(instance, instanceHashCode, instanceType, dict, true);
            });
        }

        public static IAsyncEnumerable<T> SelectAsync<T>(this IAsyncEnumerable<IList<(string name, object value)>> tuples) where T : new()
        {
            var instanceType = typeof(T);
            var instanceHashCode = instanceType.GetHashCode();

            return tuples.Select(t =>
            {
                var instance = new T();
                if (MapCache.TryGetValue(instanceHashCode, out var dict))
                    return t.SelectInternal(instance, instanceHashCode, instanceType, dict, false);
                dict = new Dictionary<int, MapCacheItem>();
                return t.SelectInternal(instance, instanceHashCode, instanceType, dict, true);
            });
        }
    }
}