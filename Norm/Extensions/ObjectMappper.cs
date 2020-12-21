using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Norm
{
    public static partial class NormExtensions
    {
        internal static IEnumerable<T> Map<T>(this IEnumerable<(string name, object value)[]> tuples, Type type = null)
        {
            type ??= typeof(T);
            var ctorInfo = TypeCache<T>.GetCtorInfo(type);
            if (ctorInfo.Item1 == null)
            {
                throw new ArgumentException($"When mapping database results, feneric paramater for must be iether class, record or value tuple. Your type is {type.FullName}");
            }
            foreach (var t in tuples.MapInternal<T>(type, ctorInfo))
            {
                yield return t;
            }
        }

        internal static IEnumerable<T> MapValueTuple<T>(this IEnumerable<(string name, object value)[]> tuples, Type type = null)
        {
            type ??= typeof(T);
            ConstructorInfo defaultCtor = type.GetConstructors()[0];
            ParameterInfo[] ctorParams = defaultCtor.GetParameters();
            var len = ctorParams.Length;
            if (len < 8)
            {
                foreach (var t in tuples)
                {
                    yield return (T)defaultCtor.Invoke(t.Take(7).Select(t => t.value).ToArray());
                }
            }
            else
            {
                var lastCtor = ctorParams[7].ParameterType.GetConstructors()[0];
                if (lastCtor.GetParameters().Length > 7)
                {
                    throw new ArgumentException($"Too many named tuple members. Maximum is 14.");
                }
                foreach (var t in tuples)
                {
                    yield return (T)defaultCtor.Invoke(t.Take(7).Select(t => t.value).Union(Enumerable.Repeat(lastCtor.Invoke(t.Skip(7).Select(t => t.value).ToArray()), 1)).ToArray());
                }
            }
        }

        internal static async IAsyncEnumerable<T> Map<T>(this IAsyncEnumerable<(string name, object value)[]> tuples, Type type = null)
        {
            type ??= typeof(T);
            var ctorInfo = TypeCache<T>.GetCtorInfo(type);
            if (ctorInfo.Item1 == null)
            {
                throw new ArgumentException($"When mapping database results, feneric paramater for must be iether class, record or value tuple. Your type is {type.FullName}");
            }
            await foreach (var t in tuples.MapInternal<T>(type, ctorInfo))
            {
                yield return t;
            }
        }

        internal static async IAsyncEnumerable<T> MapValueTuple<T>(this IAsyncEnumerable<(string name, object value)[]> tuples, Type type = null)
        {
            type ??= typeof(T);
            ConstructorInfo defaultCtor = type.GetConstructors()[0];
            ParameterInfo[] ctorParams = defaultCtor.GetParameters();
            var len = ctorParams.Length;
            if (len < 8)
            {
                await foreach (var t in tuples)
                {
                    yield return (T)defaultCtor.Invoke(t.Take(7).Select(t => t.value).ToArray());
                }
            }
            else
            {
                var lastCtor = ctorParams[7].ParameterType.GetConstructors()[0];
                if (lastCtor.GetParameters().Length > 7)
                {
                    throw new ArgumentException($"Too many named tuple members. Maximum is 14.");
                }
                await foreach (var t in tuples)
                {
                    yield return (T)defaultCtor.Invoke(t.Take(7).Select(t => t.value).Union(Enumerable.Repeat(lastCtor.Invoke(t.Skip(7).Select(t => t.value).ToArray()), 1)).ToArray());
                }
            }
        }

        private static IEnumerable<T> MapInternal<T>(
            this IEnumerable<(string name, object value)[]> tuples, Type type, (T instance, Func<T, object> clone) ctorInfo)
        {
            foreach (var tuple in tuples)
            {
                yield return tuple.MapInstance(type, TypeCache<T>.CreateInstance(ctorInfo));
            }
        }

        private static async IAsyncEnumerable<T> MapInternal<T>(
            this IAsyncEnumerable<(string name, object value)[]> tuples, Type type, (T instance, Func<T, object> clone) ctorInfo)
        {
            await foreach (var tuple in tuples)
            {
                yield return tuple.MapInstance(type, TypeCache<T>.CreateInstance(ctorInfo));
            }
        }

        private static T MapInstance<T>(this (string name, object value)[] tuple, Type type, T instance)
        {
            ushort i = 0;
            var properties = TypeCache<T>.GetProperties(type);
            var delegates = TypeCache<T>.GetDelegates(properties.Length);
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
                        names = TypeCache<T>.GetNames(tuple);
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

            return code switch
            {
                TypeCode.Int32 => (isArray ? CreateDelegateValue<T, int[]>(property) : CreateDelegateStruct<T, int>(property, nullable), code, isArray),
                TypeCode.DateTime => (isArray ? CreateDelegateValue<T, DateTime[]>(property) : CreateDelegateStruct<T, DateTime>(property, nullable), code, isArray),
                TypeCode.String => (isArray ? CreateDelegateValue<T, string[]>(property) : CreateDelegateValue<T, string>(property), code, isArray),
                TypeCode.Boolean => (isArray ? CreateDelegateValue<T, bool[]>(property) : CreateDelegateStruct<T, bool>(property, nullable), code, isArray),
                TypeCode.Byte => (isArray ? CreateDelegateValue<T, byte[]>(property) : CreateDelegateStruct<T, byte>(property, nullable), code, isArray),
                TypeCode.Char => (isArray ? CreateDelegateValue<T, char[]>(property) : CreateDelegateStruct<T, char>(property, nullable), code, isArray),
                TypeCode.Decimal => (isArray ? CreateDelegateValue<T, decimal[]>(property) : CreateDelegateStruct<T, decimal>(property, nullable), code, isArray),
                TypeCode.Double => (isArray ? CreateDelegateValue<T, double[]>(property) : CreateDelegateStruct<T, double>(property, nullable), code, isArray),
                TypeCode.Int16 => (isArray ? CreateDelegateValue<T, short[]>(property) : CreateDelegateStruct<T, short>(property, nullable), code, isArray),
                TypeCode.Int64 => (isArray ? CreateDelegateValue<T, long[]>(property) : CreateDelegateStruct<T, long>(property, nullable), code, isArray),
                TypeCode.SByte => (isArray ? CreateDelegateValue<T, sbyte[]>(property) : CreateDelegateStruct<T, sbyte>(property, nullable), code, isArray),
                TypeCode.Single => (isArray ? CreateDelegateValue<T, float[]>(property) : CreateDelegateStruct<T, float>(property, nullable), code, isArray),
                TypeCode.UInt16 => (isArray ? CreateDelegateValue<T, ushort[]>(property) : CreateDelegateStruct<T, ushort>(property, nullable), code, isArray),
                TypeCode.UInt32 => (isArray ? CreateDelegateValue<T, uint[]>(property) : CreateDelegateStruct<T, uint>(property, nullable), code, isArray),
                TypeCode.UInt64 => (isArray ? CreateDelegateValue<T, ulong[]>(property) : CreateDelegateStruct<T, ulong>(property, nullable), code, isArray),
                _ => throw new NotImplementedException($"TypeCode {code} not implemnted"),
            };
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