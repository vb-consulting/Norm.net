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
        private static (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, bool isTimespan)[] delegateCache = null;
        private static (Type type, bool simple, bool valueTuple) metadata = default;
        private static readonly object metadataLocker = new object();
        private static readonly HashSet<Type> ValueTupleTypes = new HashSet<Type>(
           new Type[]
           {
                        typeof(ValueTuple<>),
                        typeof(ValueTuple<,>),
                        typeof(ValueTuple<,,>),
                        typeof(ValueTuple<,,,>),
                        typeof(ValueTuple<,,,,>),
                        typeof(ValueTuple<,,,,,>),
                        typeof(ValueTuple<,,,,,,>),
                        typeof(ValueTuple<,,,,,,,>)
           });

        private static readonly object valueTupleCtorInfoLocker = new object();
        private static (ConstructorInfo defaultCtor, int defaultCtorLen, ConstructorInfo lastCtor, int lastCtorLen) 
            ValueTupleCtorInfo = default;


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
                return properties = type.GetProperties();
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
                return names = result;
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
                return ctorInfo = (
                    (T)defaultCtor.Invoke(Enumerable.Repeat<object>(default, defaultCtor.GetParameters().Length).ToArray()),
                    (Func<T, object>)Delegate.CreateDelegate(typeof(Func<T, object>), type.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)));
            }
        }

        internal static T CreateInstance((T instance, Func<T, object> clone) info)
        {
            return (T)info.clone.Invoke(info.instance);
        }

        internal static (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, bool isTimespan)[] GetDelegates(int len)
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
                return delegateCache = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, bool isTimespan)[len];
            }
        }

        private static Type TimeSpanType = typeof(TimeSpan);

        internal static (Type type, bool simple, bool valueTuple) GetMetadata()
        {
            if (metadata.type != null)
            {
                return metadata;
            }
            lock (metadataLocker)
            {
                if (metadata.type != null)
                {
                    return metadata;
                }
                var type = typeof(T);
                var simple = (type, true, false);
                if (type.IsPrimitive)
                {
                    return metadata = simple;
                }
                TypeCode code;
                if (type.IsArray)
                {
                    var elementType = type.GetElementType();
                    code = Type.GetTypeCode(elementType);
                    if (code == TypeCode.Object && elementType == TimeSpanType)
                    {
                        return simple;
                    }
                }
                else
                {
                    if (type.IsGenericType)
                    {
                        if (ValueTupleTypes.Contains(type.GetGenericTypeDefinition()))
                        {
                            return metadata = (type, true, true);
                        }
                        code = Type.GetTypeCode(type.GenericTypeArguments[0]);
                        if (code == TypeCode.Object && type.GenericTypeArguments[0] == TimeSpanType)
                        {
                            return metadata = simple;
                        }
                    }
                    else
                    {
                        code = Type.GetTypeCode(type);
                        if (code == TypeCode.Object && type == TimeSpanType)
                        {
                            return metadata = simple;
                        }
                    }
                }
                
                return metadata = code switch
                {
                    TypeCode.Int32 => simple,
                    TypeCode.DateTime => simple,
                    TypeCode.String => simple,
                    TypeCode.Boolean => simple,
                    TypeCode.Byte => simple,
                    TypeCode.Char => simple,
                    TypeCode.Decimal => simple,
                    TypeCode.Double => simple,
                    TypeCode.Int16 => simple,
                    TypeCode.Int64 => simple,
                    TypeCode.SByte => simple,
                    TypeCode.Single => simple,
                    TypeCode.UInt16 => simple,
                    TypeCode.UInt32 => simple,
                    TypeCode.UInt64 => simple,
                    _ => (type, false, false),
                };
            }
        }

        internal static (ConstructorInfo defaultCtor, int defaultCtorLen, ConstructorInfo lastCtor, int lastCtorLen) GetValueTupleCtorInfo(Type type)
        {
            if (ValueTupleCtorInfo.defaultCtor != null)
            {
                return ValueTupleCtorInfo;
            }
            lock (valueTupleCtorInfoLocker)
            {
                if (ValueTupleCtorInfo.defaultCtor != null)
                {
                    return ValueTupleCtorInfo;
                }

                ConstructorInfo defaultCtor = type.GetConstructors()[0];
                ParameterInfo[] ctorParams = defaultCtor.GetParameters();
                var len = ctorParams.Length;
                if (len < 8)
                {
                    return ValueTupleCtorInfo = (defaultCtor, len, null, 0);
                }
                else
                {
                    var lastCtor = ctorParams[7].ParameterType.GetConstructors()[0];
                    var lastLen = lastCtor.GetParameters().Length;
                    if (lastLen > 7)
                    {
                        throw new NormValueTupleTooLongException();
                    }
                    return ValueTupleCtorInfo = (defaultCtor, len, lastCtor, lastLen);
                }
                
            }
        }
    }
}