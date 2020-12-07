using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Norm.Extensions
{
    public static partial class NormExtensions
    {
        public static IEnumerable<T> Map<T>(this IEnumerable<(string name, object value)[]> tuples)
        {
            var type = typeof(T);
            var hash = type.GetHashCode();
            var (parametlessCtor, paramsCtor, parameters) = GetCtors(type, hash);

            if (paramsCtor == null && parametlessCtor != null)
            {
                foreach (var t in tuples.MapInternal<T>(type, parametlessCtor, hash))
                {
                    yield return t;
                }
            }
            else
            {
                foreach (var record in tuples)
                {
                    yield return (T)paramsCtor.Invoke(record.Select(r => r.value).ToArray());
                }
            }
        }

        public static async IAsyncEnumerable<T> Map<T>(this IAsyncEnumerable<(string name, object value)[]> tuples)
        {
            var type = typeof(T);
            var hash = type.GetHashCode();
            var (parametlessCtor, paramsCtor, parameters) = GetCtors(type, hash);

            if (paramsCtor == null && parametlessCtor != null)
            {
                await foreach (var t in tuples.MapInternal<T>(type, parametlessCtor, hash))
                {
                    yield return t;
                }
            }
            else
            {
                await foreach (var record in tuples)
                {
                    yield return (T)paramsCtor.Invoke(record.Select(r => r.value).ToArray());
                }
            }
        }

        private static readonly ConcurrentDictionary<int, (ConstructorInfo parametlessCtor, ConstructorInfo paramsCtor, ParameterInfo[] parameters)> CtorCache =
            new ConcurrentDictionary<int, (ConstructorInfo parametlessCtor, ConstructorInfo paramsCtor, ParameterInfo[] parameters)>();

        private static (ConstructorInfo parametlessCtor, ConstructorInfo paramsCtor, ParameterInfo[] parameters) GetCtors(Type type, int hash)
        {
            if (CtorCache.TryGetValue(hash, out var result))
            {
                return result;
            }
            ConstructorInfo parametlessCtor = null;
            ConstructorInfo paramsCtor = null;
            ParameterInfo[] parameters = null;
            foreach (var ctor in type.GetConstructors())
            {
                var p = ctor.GetParameters();
                var len = p.Length;
                if (len == 0)
                {
                    parametlessCtor = ctor;
                }
                if (len != 0 && (paramsCtor == null || parameters.Length < len))
                {
                    paramsCtor = ctor;
                    parameters = p;
                }
            }
            CtorCache.TryAdd(hash, (parametlessCtor, paramsCtor, parameters));
            return (parametlessCtor, paramsCtor, parameters);
        }

        private static IEnumerable<T> MapInternal<T>(
            this IEnumerable<(string name, object value)[]> tuples, Type type, ConstructorInfo ctor, int hash)
        {
            foreach(var tuple in tuples)
            {
                yield return tuple.GetInstance<T>(type, ctor, hash);
            }
        }

        private static async IAsyncEnumerable<T> MapInternal<T>(
            this IAsyncEnumerable<(string name, object value)[]> tuples, Type type, ConstructorInfo ctor, int hash)
        {
            await foreach (var tuple in tuples)
            {
                yield return tuple.GetInstance<T>(type, ctor, hash);
            }
        }

        private static T GetInstance<T>(this (string name, object value)[] tuple, Type type, ConstructorInfo ctor, int hash)
        {
            var instance = (T)ctor.Invoke(Array.Empty<object>());

            byte i = 0;
            var properties = GetProperties(hash, type);
            var delegates = GetDelegates(hash, properties.Length);
            foreach (var property in properties)
            {
                var value = tuple[i].value;
                var (method, nullable, code, isArray) = delegates[i];
                if (method == null)
                {
                    var propType = property.PropertyType;
                    nullable = Nullable.GetUnderlyingType(propType) != null;
                    (method, code, isArray) = CreateDelegate<T>(property, nullable);
                    delegates[i] = (method, nullable, code, isArray);
                }
                InvokeSet(method, nullable, code, instance, value, isArray);
                i++;
            }
            return instance;
        }

        private static readonly ConcurrentDictionary<int, PropertyInfo[]> PropertiesCache =
            new ConcurrentDictionary<int, PropertyInfo[]>();

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

        private static readonly ConcurrentDictionary<int, (Delegate method, bool nullable, TypeCode code, bool isArray)[]> DelegateCache = 
            new ConcurrentDictionary<int, (Delegate method, bool nullable, TypeCode code, bool isArray)[]>();

        private static (Delegate method, bool nullable, TypeCode code, bool isArray)[] GetDelegates(int hash, int len)
        {
            if (DelegateCache.TryGetValue(hash, out var result))
            {
                return result;
            }
            result = new (Delegate method, bool nullable, TypeCode code, bool isArray)[len];
            DelegateCache.TryAdd(hash, result);
            return result;
        }

        private static (Delegate method, TypeCode code, bool isArray) CreateDelegate<T>(PropertyInfo property, bool nullable)
        {
            TypeCode code;
            bool isArray;
            var type = property.PropertyType;
            if (type.IsArray)
            {
                isArray = true;
                code = Type.GetTypeCode(type.GetElementType());
            }
            else
            {
                isArray = false;
                code = nullable ? Type.GetTypeCode(type.GenericTypeArguments[0]) : Type.GetTypeCode(type);
            }

            switch (code)
            {
                case TypeCode.Int32:
                    return (isArray ? CreateDelegateValue<T, int[]>(property) : CreateDelegateStruct<T, int>(property, nullable), code, isArray);
                case TypeCode.DateTime:
                    return (isArray ? CreateDelegateValue<T, DateTime[]>(property) : CreateDelegateStruct<T, DateTime>(property, nullable), code, isArray);
                case TypeCode.String:
                    return (isArray ? CreateDelegateValue<T, string[]>(property) : CreateDelegateValue<T, string>(property), code, isArray);
                case TypeCode.Boolean:
                    return (isArray ? CreateDelegateValue<T, bool[]>(property) : CreateDelegateStruct<T, bool>(property, nullable), code, isArray);
                case TypeCode.Byte:
                    return (isArray ? CreateDelegateValue<T, byte[]>(property) : CreateDelegateStruct<T, byte>(property, nullable), code, isArray);
                case TypeCode.Char:
                    return (isArray ? CreateDelegateValue<T, char[]>(property) : CreateDelegateStruct<T, char>(property, nullable), code, isArray);
                case TypeCode.Decimal:
                    return (isArray ? CreateDelegateValue<T, decimal[]>(property) : CreateDelegateStruct<T, decimal>(property, nullable), code, isArray);
                case TypeCode.Double:
                    return (isArray ? CreateDelegateValue<T, double[]>(property) : CreateDelegateStruct<T, double>(property, nullable), code, isArray);
                case TypeCode.Int16:
                    return (isArray ? CreateDelegateValue<T, short[]>(property) : CreateDelegateStruct<T, short>(property, nullable), code, isArray);
                case TypeCode.Int64:
                    return (isArray ? CreateDelegateValue<T, long[]>(property) : CreateDelegateStruct<T, long>(property, nullable), code, isArray);
                case TypeCode.SByte:
                    return (isArray ? CreateDelegateValue<T, sbyte[]>(property) : CreateDelegateStruct<T, sbyte>(property, nullable), code, isArray);
                case TypeCode.Single:
                    return (isArray ? CreateDelegateValue<T, float[]>(property) : CreateDelegateStruct<T, float>(property, nullable), code, isArray);
                case TypeCode.UInt16:
                    return (isArray ? CreateDelegateValue<T, ushort[]>(property) : CreateDelegateStruct<T, ushort>(property, nullable), code, isArray);
                case TypeCode.UInt32:
                    return (isArray ? CreateDelegateValue<T, uint[]>(property) : CreateDelegateStruct<T, uint>(property, nullable), code, isArray);
                case TypeCode.UInt64:
                    return (isArray ? CreateDelegateValue<T, ulong[]>(property) : CreateDelegateStruct<T, ulong>(property, nullable), code, isArray);
            }
            throw new NotImplementedException($"TypeCode {code} not implemnted");
        }

        private static Delegate CreateDelegateValue<T, TProp>(PropertyInfo property)
        {
            return Delegate.CreateDelegate(typeof(Action<T, TProp>), property.GetSetMethod(true));
        }

        private static Delegate CreateDelegateStruct<T, TProp>(PropertyInfo property, bool nullable) where TProp : struct
        {
            return nullable ?
                Delegate.CreateDelegate(typeof(Action<T, TProp?>), property.GetSetMethod(true)) :
                Delegate.CreateDelegate(typeof(Action<T, TProp>), property.GetSetMethod(true));
        }

        private static void InvokeSet<T>(Delegate method, bool nullable, TypeCode code, T instance, object value, bool isArray)
        {
            switch (code)
            {
                case TypeCode.Int32:
                    if (isArray) InvokeSetValue<T, int[]>(method, instance, value); else InvokeSetStruct<T, int>(method, nullable, instance, value);
                    break;
                case TypeCode.DateTime:
                    if (isArray) InvokeSetValue<T, DateTime[]> (method, instance, value); else InvokeSetStruct<T, DateTime>(method, nullable, instance, value);
                    break;
                case TypeCode.String:
                    if (isArray) InvokeSetValue<T, string[]>(method, instance, value); else InvokeSetValue<T, string>(method, instance, value);
                    break;
                case TypeCode.Boolean:
                    if (isArray) InvokeSetValue<T, bool[]>(method, instance, value); else InvokeSetStruct<T, bool>(method, nullable, instance, value);
                    break;
                case TypeCode.Byte:
                    if (isArray) InvokeSetValue<T, byte[]>(method, instance, value); else InvokeSetStruct<T, byte>(method, nullable, instance, value);
                    break;
                case TypeCode.Char:
                    if (isArray) InvokeSetValue<T, char[]>(method, instance, value); else InvokeSetStruct<T, char>(method, nullable, instance, value);
                    break;
                case TypeCode.Decimal:
                    if (isArray) InvokeSetValue<T, decimal[]>(method, instance, value); else InvokeSetStruct<T, decimal>(method, nullable, instance, value);
                    break;
                case TypeCode.Double:
                    if (isArray) InvokeSetValue<T, double[]>(method, instance, value); else InvokeSetStruct<T, double>(method, nullable, instance, value);
                    break;
                case TypeCode.Int16:
                    if (isArray) InvokeSetValue<T, short[]>(method, instance, value); else InvokeSetStruct<T, short>(method, nullable, instance, value);
                    break;
                case TypeCode.Int64:
                    if (isArray) InvokeSetValue<T, long[]>(method, instance, value); else InvokeSetStruct<T, long>(method, nullable, instance, value);
                    break;
                case TypeCode.SByte:
                    if (isArray) InvokeSetValue<T, sbyte[]>(method, instance, value); else InvokeSetStruct<T, sbyte>(method, nullable, instance, value);
                    break;
                case TypeCode.Single:
                    if (isArray) InvokeSetValue<T, float[]>(method, instance, value); else InvokeSetStruct<T, float>(method, nullable, instance, value);
                    break;
                case TypeCode.UInt16:
                    if (isArray) InvokeSetValue<T, ushort[]>(method, instance, value); else InvokeSetStruct<T, ushort>(method, nullable, instance, value);
                    break;
                case TypeCode.UInt32:
                    if (isArray) InvokeSetValue<T, uint[]>(method, instance, value); else InvokeSetStruct<T, uint>(method, nullable, instance, value);
                    break;
                case TypeCode.UInt64:
                    if (isArray) InvokeSetValue<T, ulong[]>(method, instance, value); else InvokeSetStruct<T, ulong>(method, nullable, instance, value);
                    break;
            }
        }

        private static void InvokeSetValue<T, TProp>(Delegate method, T instance, object value)
        {
            ((Action<T, TProp>)method).Invoke(instance, (TProp)value);
        }

        private static void InvokeSetStruct<T, TProp>(Delegate method, bool nullable, T instance, object value) where TProp : struct
        {
            if (nullable)
            {
                ((Action<T, TProp?>)method).Invoke(instance, (TProp?)value);
            }
            else
            {
                ((Action<T, TProp>)method).Invoke(instance, (TProp)value);
            }
        }
    }
}