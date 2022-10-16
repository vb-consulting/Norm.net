using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Norm.Mapper
{
    internal class MapDescriptor
    {
        public Dictionary<string, ushort[]> Names;
        public HashSet<ushort> Used;
        public int Length;

        public void Reset()
        {
            Used = new HashSet<ushort>(Length);
        }
    }

    public static partial class NormExtensions
    {
        private enum StructType { None, TimeSpan, DateTimeOffset, Guid, Enum }

        private static void MapInstance<T>(this (string name, object value)[] tuple,
            ref T instance,
            ref MapDescriptor descriptor,
            ref (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType, bool created)[] delegates)
        {
            ushort i = 0;
            foreach (var property in TypeCache<T>.GetProperties())
            {
                var (method, nullable, code, isArray, index, structType, created) = delegates[i];
                if (!created)
                {

                    if (!descriptor.Names.TryGetValue(property.name, out var indexArr))
                    {
                        continue;
                    }
                    if (descriptor.Used != null)
                    {
                        if (descriptor.Used.Count == 0)
                        {
                            index = indexArr[0];
                        }
                        else
                        {
                            bool found = false;
                            foreach(var val in new Span<ushort>(indexArr, 0, indexArr.Length))
                            {
                                if (!descriptor.Used.Contains(val))
                                {
                                    index = val;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        index = indexArr[0];
                    }

                    nullable = Nullable.GetUnderlyingType(property.type) != null;
                    (method, code, isArray, structType) = CreateDelegate<T>(property.info, nullable);
                    delegates[i] = (method, nullable, code, isArray, index, structType, true);
                }
                i++;
                if (method != null)
                {
                    InvokeSet(method, nullable, code, instance, tuple[index].value, isArray, structType);
                }
                else if (structType == StructType.Enum)
                {
                    SetEnum(tuple[index].value, instance, property, nullable, isArray);
                }
                if (descriptor.Used != null)
                {
                    descriptor.Used.Add(index);
                }
            }

            if (i == 0 && instance.GetType() == typeof(ExpandoObject))
            {
                ushort index = 0; 
                foreach (var (name, value) in tuple)
                {
                    var parsedName = ParseName(name);
                    if (!descriptor.Names.TryGetValue(parsedName, out var indexArr))
                    {
                        continue;
                    }
                    if (descriptor.Used != null)
                    {
                        if (descriptor.Used.Count == 0)
                        {
                            index = indexArr[0];
                        }
                        else
                        {
                            bool found = false;
                            foreach (var val in new Span<ushort>(indexArr, 0, indexArr.Length))
                            {
                                if (!descriptor.Used.Contains(val))
                                {
                                    index = val;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        index = indexArr[0];
                    }
                    (instance as ExpandoObject).TryAdd(parsedName, value);
                    if (descriptor.Used != null)
                    {
                        descriptor.Used.Add(index);
                    }
                }
            }
        }

        private static void MapInstance<T>(this (string name, object value, bool set)[] tuple,
            ref T instance,
            ref MapDescriptor descriptor,
            ref (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType, bool created)[] delegates)
        {
            ushort i = 0;
            foreach (var property in TypeCache<T>.GetProperties())
            {
                var (method, nullable, code, isArray, index, structType, created) = delegates[i];
                if (!created)
                {
                    if (!descriptor.Names.TryGetValue(property.name, out var indexArr))
                    {
                        continue;
                    }
                    if (descriptor.Used != null)
                    {
                        if (descriptor.Used.Count == 0)
                        {
                            index = indexArr[0];
                        }
                        else
                        {
                            bool found = false;
                            foreach (var val in new Span<ushort>(indexArr, 0, indexArr.Length))
                            {
                                if (!descriptor.Used.Contains(val))
                                {
                                    index = val;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        index = indexArr[0];
                    }

                    i++;
                    var current = tuple[index];
                    if (current.set)
                    {
                        property.info.SetValue(instance, current.value);
                        if (descriptor.Used != null)
                        {
                            descriptor.Used.Add(index);
                        }
                        continue;
                    }
                    nullable = Nullable.GetUnderlyingType(property.type) != null;
                    (method, code, isArray, structType) = CreateDelegate<T>(property.info, nullable);
                    delegates[i - 1] = (method, nullable, code, isArray, index, structType, true);
                }
                else
                {
                    i++;
                }
                if (method != null)
                {
                    InvokeSet(method, nullable, code, instance, tuple[index].value, isArray, structType);
                }
                else if (structType == StructType.Enum)
                {
                    SetEnum(tuple[index].value, instance, property, nullable, isArray);
                }
                if (descriptor.Used != null)
                {
                    descriptor.Used.Add(index);
                }
            }
            if (i == 0 && instance.GetType() == typeof(ExpandoObject))
            {
                ushort index = 0;
                foreach (var (name, value, set) in tuple)
                {
                    var parsedName = ParseName(name);
                    if (!descriptor.Names.TryGetValue(parsedName, out var indexArr))
                    {
                        continue;
                    }
                    if (descriptor.Used != null)
                    {
                        if (descriptor.Used.Count == 0)
                        {
                            index = indexArr[0];
                        }
                        else
                        {
                            bool found = false;
                            foreach (var val in new Span<ushort>(indexArr, 0, indexArr.Length))
                            {
                                if (!descriptor.Used.Contains(val))
                                {
                                    index = val;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        index = indexArr[0];
                    }
                    if (set)
                    {
                        (instance as ExpandoObject).TryAdd(parsedName, value);
                        if (descriptor.Used != null)
                        {
                            descriptor.Used.Add(index);
                        }
                    }

                }
            }
        }

        private static MapDescriptor BuildDescriptor((string name, object value)[] tuple)
        {
            var descriptor = new MapDescriptor();
            if (tuple == null)
            {
                return descriptor;
            }
            descriptor.Names = new Dictionary<string, ushort[]>();
            descriptor.Length = tuple.Length;
            ushort i = 0;
            foreach (var t in tuple)
            {
                var name = ParseName(t.name);
                if (descriptor.Names.TryGetValue(name, out var arr))
                {
                    var len = arr.Length;
                    Array.Resize(ref arr, len + 1);
                    arr[len] = i;
                    descriptor.Names[name] = arr;
                }
                else
                {
                    descriptor.Names.Add(name, new ushort[1] { i });
                }
                i++;
            }
            return descriptor;
        }

        private static MapDescriptor BuildDescriptor((string name, object value, bool set)[] tuple)
        {
            var descriptor = new MapDescriptor();
            if (tuple == null)
            {
                return descriptor;
            }
            descriptor.Names = new Dictionary<string, ushort[]>();
            descriptor.Length = tuple.Length;
            ushort i = 0;
            foreach (var t in tuple)
            {
                var name = ParseName(t.name);
                if (descriptor.Names.TryGetValue(name, out var arr))
                {
                    var len = arr.Length;
                    Array.Resize(ref arr, len + 1);
                    arr[len] = i;
                    descriptor.Names[name] = arr;
                }
                else
                {
                    descriptor.Names.Add(name, new ushort[1] { i });
                }
                i++;
            }
            return descriptor;
        }

        private static string ParseName(string input)
        {
            return input.ToLowerInvariant().Replace("@", "").Replace("_", "");
        }

        private static void SetEnum<T>(
            object value,
            T instance,
            (Type type, string name, PropertyInfo info) property,
            bool nullable,
            bool isArray)
        {
            if (value == null)
            {
                property.info.SetValue(instance, null);
                return;
            }
            var type = isArray ? value?.GetType().GetElementType() : value?.GetType();
            if (nullable)
            {
                if (type == typeof(string))
                {
                    property.info.SetValue(instance, Enum.Parse(property.type.GenericTypeArguments[0], (string)value));
                }
                else
                {
                    property.info.SetValue(instance, Enum.ToObject(property.type.GenericTypeArguments[0], value));
                }
            }
            else
            {
                if (type == typeof(string))
                {
                    if (isArray)
                    {
                        var elementType = property.type.GetElementType();
                        var valueArray = (object[])value;
                        var enumArray = Array.CreateInstance(elementType, valueArray.Length);
                        for (int i = 0; i < valueArray.Length; i++)
                        {
                            enumArray.SetValue(Enum.Parse(elementType, (string)valueArray[i]), i);
                        }

                        property.info.SetValue(instance, enumArray);
                    }
                    else
                    {
                        property.info.SetValue(instance, Enum.Parse(property.type, (string)value));
                    }
                }
                else
                {
                    if (isArray)
                    {
                        var elementType = property.type.GetElementType();
                        var valueArray = (int[])value;
                        var enumArray = Array.CreateInstance(elementType, valueArray.Length);
                        for (int i = 0; i < valueArray.Length; i++)
                        {
                            enumArray.SetValue(Enum.ToObject(elementType, valueArray[i]), i);
                        }

                        property.info.SetValue(instance, enumArray);
                    }
                    else
                    {
                        property.info.SetValue(instance, Enum.ToObject(property.type, value));
                    }
                }
            }
        }

        private static 
            (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType, bool created)[]
            CreateDelegateArray(int length)
        {
            return new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType, bool created)[length];
        }

        private static (Delegate method, TypeCode code, bool isArray, StructType structType) CreateDelegate<T>(PropertyInfo property, bool nullable)
        {
            var type = property.PropertyType;
            if (property.GetMethod.IsVirtual)
            {
                if (type.IsGenericType || (type.IsClass && type != typeof(string)))
                {
                    return (null, TypeCode.Empty, false, StructType.None);
                }
            }
            TypeCode code;
            bool isArray;

            if (type.IsArray)
            {
                isArray = true;
                var elementType = type.GetElementType();
                code = Type.GetTypeCode(elementType);
                
                if (elementType.IsEnum || (nullable && elementType.GenericTypeArguments[0].IsEnum))
                {
                    return (null, code, isArray, StructType.Enum);
                }
                
                if (code == TypeCode.Object)
                {
                    if (elementType == typeof(TimeSpan))
                    {
                        return (CreateDelegateValue<T, TimeSpan[]>(property), code, isArray, StructType.TimeSpan);
                    }
                    if (elementType == typeof(Guid))
                    {
                        return (CreateDelegateValue<T, Guid[]>(property), code, isArray, StructType.Guid);
                    }
                    if (elementType == typeof(DateTimeOffset))
                    {
                        return (CreateDelegateValue<T, DateTimeOffset[]>(property), code, isArray, StructType.DateTimeOffset);
                    }
                }
            }
            else
            {
                isArray = false;
                code = nullable ? Type.GetTypeCode(type.GenericTypeArguments[0]) : Type.GetTypeCode(type);
                
                if (type.IsEnum || (nullable && type.GenericTypeArguments[0].IsEnum))
                {
                    return (null, code, isArray, StructType.Enum);
                }
                
                if (code == TypeCode.Object)
                {
                    if (type == typeof(TimeSpan) || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0] == typeof(TimeSpan)))
                    {
                        return (CreateDelegateStruct<T, TimeSpan>(property, nullable), code, isArray, StructType.TimeSpan);
                    }
                    if (type == typeof(Guid) || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0] == typeof(Guid)))
                    {
                        return (CreateDelegateStruct<T, Guid>(property, nullable), code, isArray, StructType.Guid);
                    }
                    if (type == typeof(DateTimeOffset) || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0] == typeof(DateTimeOffset)))
                    {
                        return (CreateDelegateStruct<T, DateTimeOffset>(property, nullable), code, isArray, StructType.DateTimeOffset);
                    }
                }
            }

            return code switch
            {
                TypeCode.Int32 => (isArray ? CreateDelegateValue<T, int[]>(property) : CreateDelegateStruct<T, int>(property, nullable), code, isArray, StructType.None),
                TypeCode.DateTime => (isArray ? CreateDelegateValue<T, DateTime[]>(property) : CreateDelegateStruct<T, DateTime>(property, nullable), code, isArray, StructType.None),
                TypeCode.String => (isArray ? CreateDelegateValue<T, string[]>(property) : CreateDelegateValue<T, string>(property), code, isArray, StructType.None),
                TypeCode.Boolean => (isArray ? CreateDelegateValue<T, bool[]>(property) : CreateDelegateStruct<T, bool>(property, nullable), code, isArray, StructType.None),
                TypeCode.Byte => (isArray ? CreateDelegateValue<T, byte[]>(property) : CreateDelegateStruct<T, byte>(property, nullable), code, isArray, StructType.None),
                TypeCode.Char => (isArray ? CreateDelegateValue<T, char[]>(property) : CreateDelegateStruct<T, char>(property, nullable), code, isArray, StructType.None),
                TypeCode.Decimal => (isArray ? CreateDelegateValue<T, decimal[]>(property) : CreateDelegateStruct<T, decimal>(property, nullable), code, isArray, StructType.None),
                TypeCode.Double => (isArray ? CreateDelegateValue<T, double[]>(property) : CreateDelegateStruct<T, double>(property, nullable), code, isArray, StructType.None),
                TypeCode.Int16 => (isArray ? CreateDelegateValue<T, short[]>(property) : CreateDelegateStruct<T, short>(property, nullable), code, isArray, StructType.None),
                TypeCode.Int64 => (isArray ? CreateDelegateValue<T, long[]>(property) : CreateDelegateStruct<T, long>(property, nullable), code, isArray, StructType.None),
                TypeCode.SByte => (isArray ? CreateDelegateValue<T, sbyte[]>(property) : CreateDelegateStruct<T, sbyte>(property, nullable), code, isArray, StructType.None),
                TypeCode.Single => (isArray ? CreateDelegateValue<T, float[]>(property) : CreateDelegateStruct<T, float>(property, nullable), code, isArray, StructType.None),
                TypeCode.UInt16 => (isArray ? CreateDelegateValue<T, ushort[]>(property) : CreateDelegateStruct<T, ushort>(property, nullable), code, isArray, StructType.None),
                TypeCode.UInt32 => (isArray ? CreateDelegateValue<T, uint[]>(property) : CreateDelegateStruct<T, uint>(property, nullable), code, isArray, StructType.None),
                TypeCode.UInt64 => (isArray ? CreateDelegateValue<T, ulong[]>(property) : CreateDelegateStruct<T, ulong>(property, nullable), code, isArray, StructType.None),
                _ => throw new NotImplementedException($"TypeCode {code} not implemented"),
            };
        }

        private static Delegate CreateDelegateValue<T, TProp>(PropertyInfo property)
        {
            var setter = property.GetSetMethod(true);
            if (setter == null)
            {
                return null;
            }
            if (NormOptions.Value.MapPrivateSetters)
            {
                return Delegate.CreateDelegate(typeof(Action<T, TProp>), setter);
            }
            else if (setter.IsPublic)
            {
                return Delegate.CreateDelegate(typeof(Action<T, TProp>), setter);
            }
            return null;
        }

        private static Delegate CreateDelegateStruct<T, TProp>(PropertyInfo property, bool nullable) where TProp : struct
        {
            var setter = property.GetSetMethod(true);
            if (setter == null)
            {
                return null;
            }
            if (NormOptions.Value.MapPrivateSetters)
            {
                return nullable ?
                    Delegate.CreateDelegate(typeof(Action<T, TProp?>), setter) :
                    Delegate.CreateDelegate(typeof(Action<T, TProp>), setter);
            }
            else if (setter.IsPublic)
            {
                return nullable ?
                    Delegate.CreateDelegate(typeof(Action<T, TProp?>), setter) :
                    Delegate.CreateDelegate(typeof(Action<T, TProp>), setter);
            }
            return null;
        }

        private static void InvokeSet<T>(Delegate method, bool nullable, TypeCode code, T instance, object value, bool isArray, StructType structType)
        {
            if (structType == StructType.TimeSpan)
            {
                if (isArray)
                {
                    InvokeSetValue<T, TimeSpan[]>(method, instance, value);
                }
                else InvokeSetStruct<T, TimeSpan>(method, nullable, instance, value);
            }
            if (structType == StructType.Guid)
            {
                if (isArray)
                {
                    InvokeSetValue<T, Guid[]>(method, instance, value);
                }
                else InvokeSetStruct<T, Guid>(method, nullable, instance, value);
            }
            if (structType == StructType.DateTimeOffset)
            {
                if (isArray)
                {
                    DateTime[] values = (DateTime[])value;
                    DateTimeOffset[] result = new DateTimeOffset[values.Length];
                    short idx = 0;
                    foreach(var dt in values)
                    {
                        result[idx++] = new DateTimeOffset(dt);
                    }
                    InvokeSetValue<T, DateTimeOffset[]>(method, instance, result);
                }
                else
                {
                    if (nullable)
                    {
                        if (value == null)
                        {
                            ((Action<T, DateTimeOffset?>)method).Invoke(instance, null);
                        }
                        else
                        {
                            if (value is DateTimeOffset)
                            {
                                ((Action<T, DateTimeOffset?>)method).Invoke(instance, (DateTimeOffset)value);
                            }
                            else
                            {
                                ((Action<T, DateTimeOffset?>)method).Invoke(instance, new DateTimeOffset((DateTime)value));
                            }
                        }
                    }
                    else
                    {
                        if (value is DateTimeOffset)
                        {
                            ((Action<T, DateTimeOffset>)method).Invoke(instance, (DateTimeOffset)value);
                        }
                        else
                        {
                            ((Action<T, DateTimeOffset>)method).Invoke(instance, new DateTimeOffset((DateTime)value));
                        }
                    }
                }
            }
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