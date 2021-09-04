using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable StaticMemberInGenericType

namespace Norm
{
    internal static class TypeCache<T>
    {
        private static readonly object CtorLocker = new object();
        private static (T, Func<T, object>) _ctorInfo;
        private static readonly object PropertiesLocker = new object();
        private static volatile (Type type, string name, PropertyInfo info)[] _properties;

        internal static Func<(string name, object value)[], T> CustomMapTuples { get; set; }
        internal static Func<object[], T> CustomMapValues { get; set; }
        internal static Func<IDictionary<string, object>, T> CustomMapDict { get; set; }

        public static (Type type, string name, PropertyInfo info)[] GetProperties(Type type)
        {
            if (_properties != null)
            {
                return _properties;
            }
            lock (PropertiesLocker)
            {
                if (_properties != null)
                {
                    return _properties;
                }
                var props = type.GetProperties();
                var result = new (Type type, string name, PropertyInfo info)[props.Length];
                short i = 0;
                foreach(var p in props)
                {
                    result[i++] = (p.PropertyType, p.Name.ToLower(), p);
                }
                return _properties = result;
            }
        }

        internal static (T, Func<T, object>) GetCtorInfo(Type type)
        {
            if (_ctorInfo.Item1 != null)
            {
                return _ctorInfo;
            }
            lock (CtorLocker)
            {
                if (_ctorInfo.Item1 != null)
                {
                    return _ctorInfo;
                }
                var defaultCtor = type.GetConstructors()[0];
                return _ctorInfo = (
                    (T)defaultCtor.Invoke(Enumerable.Repeat<object>(default, defaultCtor.GetParameters().Length).ToArray()),
                    (Func<T, object>)Delegate.CreateDelegate(typeof(Func<T, object>), type.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)));
            }
        }

        internal static T CreateInstance((T instance, Func<T, object> clone) info)
        {
            return (T)info.clone.Invoke(info.instance);
        }

        private static (Type type, bool simple, bool valueTuple) _metadata;
        private static readonly object MetadataLocker = new object();

        internal static (Type type, bool simple, bool valueTuple) GetMetadata()
        {
            if (_metadata.type != null)
            {
                return _metadata;
            }
            lock (MetadataLocker)
            {
                return _metadata = typeof(T).GetMetadata();
            }
        }

        private static (ConstructorInfo defaultCtor, int defaultCtorLen, ConstructorInfo lastCtor, int lastCtorLen) _valueTupleCtorInfo;
        private static readonly object ValueTupleCtorInfoLocker = new object();
        
        internal static (ConstructorInfo defaultCtor, int defaultCtorLen, ConstructorInfo lastCtor, int lastCtorLen) GetValueTupleCtorInfo(Type type)
        {
            if (_valueTupleCtorInfo.defaultCtor != null)
            {
                return _valueTupleCtorInfo;
            }
            lock (ValueTupleCtorInfoLocker)
            {
                if (_valueTupleCtorInfo.defaultCtor != null)
                {
                    return _valueTupleCtorInfo;
                }

                var defaultCtor = type.GetConstructors()[0];
                var ctorParams = defaultCtor.GetParameters();
                var len = ctorParams.Length;
                if (len < 8)
                {
                    return _valueTupleCtorInfo = (defaultCtor, len, null, 0);
                }

                var lastCtor = ctorParams[7].ParameterType.GetConstructors()[0];
                var lastLen = lastCtor.GetParameters().Length;
                if (lastLen > 7)
                {
                    throw new NormValueTupleTooLongException();
                }
                return _valueTupleCtorInfo = (defaultCtor, len, lastCtor, lastLen);
            }
        }
    }
}