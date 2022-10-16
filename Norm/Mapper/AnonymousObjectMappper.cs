using System;
using System.Collections.Generic;
using System.Reflection;

namespace Norm.Mapper
{
    public static partial class NormExtensions
    {
        public static IEnumerable<T> MapAnonymous<T>(this IEnumerable<(string name, object value)[]> tuples, 
            Type type1)
        {
            var (ctorInfo, props) = TypeCache<T>.GetAnonInfo(type1);
            MapDescriptor descriptor = null;
            foreach (var tuple in tuples)
            {
                descriptor ??= BuildDescriptor(tuple);
                yield return (T)ctorInfo.Invoke(BuildAnonParameters(ref props, ref descriptor, tuple));
            }
        }

        public static async IAsyncEnumerable<T> MapAnonymous<T>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1)
        {
            var (ctorInfo, props) = TypeCache<T>.GetAnonInfo(type1);
            MapDescriptor descriptor = null;
            await foreach (var tuple in tuples)
            {
                descriptor ??= BuildDescriptor(tuple);
                yield return (T)ctorInfo.Invoke(BuildAnonParameters(ref props, ref descriptor, tuple));
            }
        }

        private static object[] BuildAnonParameters(
            ref (string name, Type type)[] props, 
            ref MapDescriptor descriptor, 
            (string name, object value)[] tuple)
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
                    value = tuple[index].value;
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