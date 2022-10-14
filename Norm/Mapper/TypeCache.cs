using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Norm.Mapper
{
    internal static class TypeCache<T>
    {
        private static readonly object PropertiesLocker = new object();
        private static int? _propertiesLen;
        private static volatile (Type type, string name, PropertyInfo info)[] _properties;

        public static Span<(Type type, string name, PropertyInfo info)> GetProperties()
        {
            if (_properties != null)
            {
                return new Span<(Type type, string name, PropertyInfo info)>(_properties, 0, _properties.Length);
            }
            lock (PropertiesLocker)
            {
                if (_properties != null)
                {
                    return new Span<(Type type, string name, PropertyInfo info)>(_properties, 0, _properties.Length);
                }
                var props = typeof(T).GetProperties();
                _propertiesLen = props.Length;
                var result = new (Type type, string name, PropertyInfo info)[_propertiesLen.Value];
                short i = 0;
                foreach(var p in props)
                {
                    result[i++] = (p.PropertyType, p.Name.ToLowerInvariant(), p);
                }
                _properties = result; 
                return new Span<(Type type, string name, PropertyInfo info)>(_properties, 0, _properties.Length);
            }
        }

        public static int GetPropertiesLength()
        {
            if (_propertiesLen.HasValue)
            {
                return _propertiesLen.Value;
            }
            GetProperties();
            return _propertiesLen.Value;
        }

        private static readonly object CtorLocker = new object();
        private static (T, Func<T, object>) _ctorInfo;

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
                if (type == typeof(object))
                {
                    type = typeof(ExpandoObject);
                }
                var defaultCtor = type.GetConstructors()[0];
                return _ctorInfo = (
                    (T)defaultCtor.Invoke(Enumerable.Repeat<object>(default, defaultCtor.GetParameters().Length).ToArray()),
                    (Func<T, object>)Delegate.CreateDelegate(typeof(Func<T, object>), type.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)));
            }
        }

        private static readonly object AnonInfoLocker = new object();
        private static (ConstructorInfo ctorInfo, (string name, Type type)[] props) _anonInfoInfo;

        internal static (ConstructorInfo ctorInfo, (string name, Type type)[] props) GetAnonInfo(Type type)
        {
            if (_anonInfoInfo.ctorInfo != null)
            {
                return _anonInfoInfo;
            }
            lock (AnonInfoLocker)
            {
                if (_anonInfoInfo.ctorInfo != null)
                {
                    return _anonInfoInfo;
                }
                if (!type.IsAnonymousType())
                {
                    throw new ArgumentException("Anonymous Type is required for this call.");
                }
                var defaultCtor = type.GetConstructors()[0];
                var ctorParams = defaultCtor.GetParameters();
                var p = new (string Name, Type Type)[ctorParams.Length];
                for (var i = 0; i < ctorParams.Length; i++)
                {
                    p[i] = (ctorParams[i].Name.ToLowerInvariant().Replace("_", ""), ctorParams[i].ParameterType);
                }
                return _anonInfoInfo = (defaultCtor, p);
            }
        }

        internal static T CreateInstance((T instance, Func<T, object> clone) info)
        {
            return (T)info.clone.Invoke(info.instance);
        }

        private static (Type type, bool simple, bool valueTuple, bool isString) _metadata;
        private static readonly object MetadataLocker = new object();

        internal static (Type type, bool simple, bool valueTuple, bool isString) GetMetadata()
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