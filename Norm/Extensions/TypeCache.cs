using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Norm
{
    internal static class TypeCache<T>
    {
        private static readonly object ctorLocker = new object();
        private static (T, Func<T, object>) ctorInfo = default;
        private static readonly object nameLocker = new object();
        private static Dictionary<string, ushort> names = null;
        private static readonly object propertiesLocker = new object();
        private static PropertyInfo[] properties = null;
        private static readonly object delegateLocker = new object();
        private static (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index)[] delegateCache = null;

        ///<summary>
        ///    Return cached property info array
        ///</summary>
        ///<param name="type">Type for properties to be returned</param>
        ///<returns>PropertyInfo array</returns>
        public static PropertyInfo[] GetProperties(Type type)
        {
            if (properties != null)
            {
                return properties;
            }
            lock (propertiesLocker)
            {
                if (properties != null)
                {
                    return properties;
                }
                properties = type.GetProperties();
                return properties;
            }
        }

        internal static Dictionary<string, ushort> GetNames((string name, object value)[] tuple)
        {
            if (names != null)
            {
                return names;
            }
            lock (nameLocker)
            {
                if (names != null)
                {
                    return names;
                }
                var hashes = new HashSet<string>();
                var result = new Dictionary<string, ushort>();
                ushort i = 0;
                foreach (var t in tuple)
                {
                    var name = string.Concat(t.name.ToLower().Replace("_", ""));
                    if (hashes.Contains(name))
                    {
                        i++;
                        continue;
                    }
                    hashes.Add(name);
                    result[name] = i++;
                }
                names = result;
                return names;
            }
        }

        internal static (T, Func<T, object>) GetCtorInfo(Type type)
        {
            if (ctorInfo.Item1 != null)
            {
                return ctorInfo;
            }
            lock (ctorLocker)
            {
                if (ctorInfo.Item1 != null)
                {
                    return ctorInfo;
                }
                var defaultCtor = type.GetConstructors()[0];
                ctorInfo = (
                    (T)defaultCtor.Invoke(Enumerable.Repeat<object>(default, defaultCtor.GetParameters().Length).ToArray()),
                    (Func<T, object>)Delegate.CreateDelegate(typeof(Func<T, object>), type.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)));
                return ctorInfo;
            }
        }

        internal static T CreateInstance((T instance, Func<T, object> clone) info)
        {
            return (T)info.clone.Invoke(info.instance);
        }

        internal static (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index)[] GetDelegates(int len)
        {
            if (delegateCache != null)
            {
                return delegateCache;
            }
            lock (delegateLocker)
            {
                if (delegateCache != null)
                {
                    return delegateCache;
                }
                delegateCache = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index)[len];
                return delegateCache;
            }
        }

        internal static (Type type, bool complex) IsSimpleType()
        {
            var type = typeof(T);
            if (type.IsPrimitive)
            {
                return (type, true);
            }
            TypeCode code;
            if (type.IsArray)
            {
                code = Type.GetTypeCode(type.GetElementType());
            }
            else
            {
                code = Nullable.GetUnderlyingType(type) != null ? Type.GetTypeCode(type.GenericTypeArguments[0]) : Type.GetTypeCode(type);
            }
            return code switch
            {
                TypeCode.Int32 => (type, true),
                TypeCode.DateTime => (type, true),
                TypeCode.String => (type, true),
                TypeCode.Boolean => (type, true),
                TypeCode.Byte => (type, true),
                TypeCode.Char => (type, true),
                TypeCode.Decimal => (type, true),
                TypeCode.Double => (type, true),
                TypeCode.Int16 => (type, true),
                TypeCode.Int64 => (type, true),
                TypeCode.SByte => (type, true),
                TypeCode.Single => (type, true),
                TypeCode.UInt16 => (type, true),
                TypeCode.UInt32 => (type, true),
                TypeCode.UInt64 => (type, true),
                _ => (type, false),
            };
        }
    }
}