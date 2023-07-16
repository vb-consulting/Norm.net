using System;
using System.Collections.Generic;
using System.Reflection;

namespace Norm.Mapper
{
    public static partial class NormExtensions
    {
        internal static IEnumerable<T> MapInstance<T>(this IEnumerable<ReadOnlyMemory<(string name, object value)>> tuples, 
            Type type1)
        {
            MapDescriptor descriptor = null;
            var (ctorInfo, props, anon) = TypeCache<T>.GetInstInfo(type1);
            if (anon)
            {
                foreach (var tuple in tuples)
                {
                    descriptor ??= BuildDescriptor(tuple);
                    yield return (T)ctorInfo.Invoke(BuildAnonParameters(ref props, ref descriptor, tuple));
                }
            }
            else
            {
                var ctorInfo1 = TypeCache<T>.GetCtorInfo(ref type1);
                var delegates = CreateDelegateArray(TypeCache<T>.GetPropertiesLength());
                foreach (var t in tuples)
                {
                    descriptor ??= BuildDescriptor(t);
                    var t1 = TypeCache<T>.CreateInstance(ref ctorInfo1);
                    MapInstance(t, ref t1, ref descriptor, ref delegates);
                    yield return t1;
                }
            }
        }

        internal static async IAsyncEnumerable<T> MapInstance<T>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1)
        {
            MapDescriptor descriptor = null;
            var (ctorInfo, props, anon) = TypeCache<T>.GetInstInfo(type1);
            if (anon)
            {
                await foreach (var tuple in tuples)
                {
                    descriptor ??= BuildDescriptor(tuple);
                    yield return (T)ctorInfo.Invoke(BuildAnonParameters(ref props, ref descriptor, tuple));
                }
            }
            else
            {
                var ctorInfo1 = TypeCache<T>.GetCtorInfo(ref type1);
                var delegates = CreateDelegateArray(TypeCache<T>.GetPropertiesLength());
                await foreach (var t in tuples)
                {
                    descriptor ??= BuildDescriptor(t);
                    var t1 = TypeCache<T>.CreateInstance(ref ctorInfo1);
                    MapInstance(t, ref t1, ref descriptor, ref delegates);
                    yield return t1;
                }
            }
        }

        private static object[] BuildAnonParameters(
            ref (string name, Type type)[] props, 
            ref MapDescriptor descriptor, 
            ReadOnlyMemory<(string name, object value)> tuple)
        {
            var parameters = new object[props.Length];

            ushort propIndex = 0;
            ushort index = 0;
            foreach (var (name, type) in props)
            {
                object value;
                if (!descriptor.Names.TryGetValue(name, out var indexArr))
                {
                    value = default;
                }
                else
                {
                    index = indexArr[0];
                    value = tuple.Span[index].value;
                    if (value != null)
                    {
                        var valueType = value.GetType();
                        if (type.IsEnum && valueType == typeof(string))
                        {
                            value = Enum.Parse(type, (string)value);
                        }
                        else
                        {
                            var nullableType = Nullable.GetUnderlyingType(type);
                            if (nullableType != null)
                            {
                                if (nullableType.IsEnum)
                                {
                                    if (valueType == typeof(string))
                                    {
                                        value = Enum.Parse(nullableType, (string)value);
                                    }
                                    else if (valueType == typeof(int))
                                    {
                                        value = Enum.ToObject(nullableType, value);
                                    }
                                }
                            }
                            else
                            {
                                if (type.IsArray && valueType.IsArray)
                                {
                                    var elementType = type.GetElementType();
                                    if (elementType.IsEnum)
                                    {
                                        if (valueType.GetElementType() == typeof(string))
                                        {
                                            var valueArray = (object[])value;
                                            var enumArray = Array.CreateInstance(elementType, valueArray.Length);
                                            for (int i = 0; i < valueArray.Length; i++)
                                            {
                                                enumArray.SetValue(Enum.Parse(elementType, (string)valueArray[i]), i);
                                            }
                                            value = enumArray;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                parameters[propIndex] = value;
                propIndex++;
            }

            return parameters;
        }
    }
}