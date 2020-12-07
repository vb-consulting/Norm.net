using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Norm.Extensions
{
    public static partial class NormExtensions
    {
        private static readonly ConcurrentDictionary<int, (object, MethodInfo)> CtorCache =
            new ConcurrentDictionary<int, (object, MethodInfo)>();

        private static readonly ConcurrentDictionary<int, PropertyInfo[]> PropertiesCache =
            new ConcurrentDictionary<int, PropertyInfo[]>();

        private static readonly ConcurrentDictionary<int, (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index)[]> DelegateCache =
            new ConcurrentDictionary<int, (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index)[]>();

        private static (object, MethodInfo) GetCtorInfo(Type type, int hash)
        {
            if (CtorCache.TryGetValue(hash, out var result))
            {
                return result;
            }
            var ctor = type.GetConstructors()[0];
            var tuple = (
                ctor.Invoke(Enumerable.Repeat<object>(default, ctor.GetParameters().Length).ToArray()),
                type.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic));
            CtorCache.TryAdd(hash, tuple);
            return tuple;
        }

        private static T CreateInstance<T>((object instance, MethodInfo clone) ctorInfo)
        {
            return (T)ctorInfo.clone.Invoke(ctorInfo.instance, null);
        }

        private static PropertyInfo[] GetProperties(int hash, Type type)
        {
            if (PropertiesCache.TryGetValue(hash, out var result))
            {
                return result;
            }
            result = type.GetProperties();
            PropertiesCache.TryAdd(hash, result);
            return result;
        }

        private static (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index)[] GetDelegates(int hash, int len)
        {
            if (DelegateCache.TryGetValue(hash, out var result))
            {
                return result;
            }
            result = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index)[len];
            DelegateCache.TryAdd(hash, result);
            return result;
        }
    }
}