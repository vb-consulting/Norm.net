using System;
using System.Collections.Generic;

namespace Norm.Mapper
{
    public static partial class NormExtensions
    {
        public static IEnumerable<T> MapAnonymous<T>(this IEnumerable<(string name, object value)[]> tuples, 
            Type type1)
        {
            var (ctorInfo, props) = TypeCache<T>.GetAnonInfo(type1);
            Dictionary<string, ushort> names = null;
            foreach (var tuple in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(tuple);
                }
                yield return (T)ctorInfo.Invoke(BuildAnonParameters(ref props, ref names, tuple));
            }
        }

        public static async IAsyncEnumerable<T> MapAnonymous<T>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1)
        {
            var (ctorInfo, props) = TypeCache<T>.GetAnonInfo(type1);
            Dictionary<string, ushort> names = null;
            await foreach (var tuple in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(tuple);
                }
                yield return (T)ctorInfo.Invoke(BuildAnonParameters(ref props, ref names, tuple));
            }
        }

        private static object[] BuildAnonParameters(
            ref (string name, Type type)[] props, 
            ref Dictionary<string, ushort> names, 
            (string name, object value)[] tuple)
        {
            var parameters = new object[props.Length];

            ushort propIndex = 0;
            foreach (var (name, type) in props)
            {
                object value;
                if (!names.TryGetValue(name, out var index))
                {
                    value = default;
                }
                else
                {
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