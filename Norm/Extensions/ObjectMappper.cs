using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Norm
{
    public static partial class NormExtensions
    {
        public static IEnumerable<T> Map<T>(this IEnumerable<(string name, object value)[]> tuples, Type type = null, int? hash = null)
        {
            type ??= typeof(T);
            int th = hash ?? type.TypeHash();
            var ctorInfo = GetCtorInfo(type, th);
            foreach (var t in tuples.MapInternal<T>(type, ctorInfo, th))
            {
                yield return t;
            }
        }

        public static async IAsyncEnumerable<T> Map<T>(this IAsyncEnumerable<(string name, object value)[]> tuples, Type type = null, int? hash = null)
        {
            type ??= typeof(T);
            int th = hash ?? type.TypeHash();
            var ctorInfo = GetCtorInfo(type, th);
            await foreach (var t in tuples.MapInternal<T>(type, ctorInfo, th))
            {
                yield return t;
            }
        }

        private static IEnumerable<T> MapInternal<T>(
            this IEnumerable<(string name, object value)[]> tuples, Type type, (object instance, MethodInfo clone) ctorInfo, int hash)
        {
            foreach (var tuple in tuples)
            {
                yield return tuple.MapInstance(type, CreateInstance<T>(ctorInfo), hash);
            }
        }

        private static async IAsyncEnumerable<T> MapInternal<T>(
            this IAsyncEnumerable<(string name, object value)[]> tuples, Type type, (object instance, MethodInfo clone) ctorInfo, int hash)
        {
            await foreach (var tuple in tuples)
            {
                yield return tuple.MapInstance(type, CreateInstance<T>(ctorInfo), hash);
            }
        }

        private static T MapInstance<T>(this (string name, object value)[] tuple, Type type, T instance, int hash)
        {
            ushort i = 0;
            var properties = GetProperties(hash, type);
            var delegates = GetDelegates(hash, properties.Length);
            Dictionary<string, ushort> names = null;
            foreach (var property in properties)
            {
                var (method, nullable, code, isArray, index) = delegates[i];
                if (method == null)
                {
                    var propType = property.PropertyType;
                    nullable = Nullable.GetUnderlyingType(propType) != null;
                    (method, code, isArray) = CreateDelegate<T>(property, nullable);

                    var name = property.Name.ToLower();
                    if (names == null)
                    {
                        names = tuple.Select((t, i) => (t, i)).ToDictionary(t => t.t.name.ToLower().Replace("_", ""), t => (ushort)t.i);
                    }
                    if (!names.TryGetValue(name, out index))
                    {
                        index = ushort.MaxValue;
                    }
                    delegates[i] = (method, nullable, code, isArray, index);
                }
                i++;
                if (index == ushort.MaxValue)
                {
                    continue;
                }
                InvokeSet(method, nullable, code, instance, tuple[index].value, isArray);
            }
            return instance;
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