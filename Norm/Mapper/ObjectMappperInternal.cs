using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Norm
{
    public static partial class NormExtensions
    {
        private static Type TimeSpanType = typeof(TimeSpan);
        private static Type DateTimeOffsetType = typeof(DateTimeOffset);
        private static Type GuidType = typeof(Guid);

        private enum StructType { None, TimeSpan, DateTimeOffset, Guid, Enum }

        private static T MapInstance<T>(this (string name, object value)[] tuple,
            ref T instance,
            ref Dictionary<string, ushort> names,
            ref (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[] delegates)
        {
            ushort i = 0;

            foreach (var property in TypeCache<T>.GetProperties())
            {
                var (method, nullable, code, isArray, index, structType) = delegates[i];
                if (method == null && structType != StructType.Enum)
                {
                    nullable = Nullable.GetUnderlyingType(property.type) != null;
                    (method, code, isArray, structType) = CreateDelegate<T>(property.info, nullable);

                    if (!names.TryGetValue(property.name, out index))
                    {
                        index = ushort.MaxValue;
                    }
                    delegates[i] = (method, nullable, code, isArray, index, structType);
                }
                i++;
                if (index == ushort.MaxValue)
                {
                    continue;
                }
                if (method != null)
                {
                    InvokeSet(method, nullable, code, instance, tuple[index].value, isArray, structType);
                }
                else if (structType == StructType.Enum)
                {
                    if (nullable)
                    {
                        if (tuple[index].value == null)
                        {
                            property.info.SetValue(instance, null);
                        }
                        else
                        {
                            property.info.SetValue(instance, Enum.Parse(property.type.GenericTypeArguments[0], (string)tuple[index].value));
                        }
                    }
                    else
                    {
                        property.info.SetValue(instance, Enum.Parse(property.type, (string)tuple[index].value));
                    }
                }
            }
            return instance;
        }

        private static T MapInstance<T>(this (string name, object value)[] tuple,
            ref T instance,
            ref Dictionary<string, ushort> names,
            ref HashSet<ushort> used,
            ref (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[] delegates)
        {
            ushort i = 0;
            foreach (var property in TypeCache<T>.GetProperties())
            {
                var (method, nullable, code, isArray, index, structType) = delegates[i];
                if (method == null)
                {
                    nullable = Nullable.GetUnderlyingType(property.type) != null;
                    (method, code, isArray, structType) = CreateDelegate<T>(property.info, nullable);
                    if (!names.TryGetValue(property.name, out index))
                    {
                        index = ushort.MaxValue;
                    }
                    if (used.Contains(index))
                    {
                        continue;
                    }
                    delegates[i] = (method, nullable, code, isArray, index, structType);
                }
                i++;
                if (index == ushort.MaxValue)
                {
                    continue;
                }
                if (method != null)
                {
                    InvokeSet(method, nullable, code, instance, tuple[index].value, isArray, structType);
                }
                else if (structType == StructType.Enum)
                {
                    if (nullable)
                    {
                        if (tuple[index].value == null)
                        {
                            property.info.SetValue(instance, null);
                        }
                        else
                        {
                            property.info.SetValue(instance, Enum.Parse(property.type.GenericTypeArguments[0], (string)tuple[index].value));
                        }
                    }
                    else
                    {
                        property.info.SetValue(instance, Enum.Parse(property.type, (string)tuple[index].value));
                    }
                }
                used.Add(index);
            }
            return instance;
        }

        private static Dictionary<string, ushort> GetNamesDictFromTuple((string name, object value)[] tuple)
        {
            if (tuple == null)
            {
                return null;
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
            return result;
        }

        private static (Delegate method, TypeCode code, bool isArray, StructType structType) CreateDelegate<T>(PropertyInfo property, bool nullable)
        {
            var type = property.PropertyType;
            if (property.GetMethod.IsVirtual)
            {
                if (type.IsGenericType || (type.IsClass && type != StringType))
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
                if (type.IsEnum || (nullable && type.GenericTypeArguments[0].IsEnum))
                {
                    return (null, code, isArray, StructType.Enum);
                }
                if (code == TypeCode.Object)
                {
                    if (elementType == TimeSpanType)
                    {
                        return (CreateDelegateValue<T, TimeSpan[]>(property), code, isArray, StructType.TimeSpan);
                    }
                    if (elementType == GuidType)
                    {
                        return (CreateDelegateValue<T, Guid[]>(property), code, isArray, StructType.Guid);
                    }
                    if (elementType == DateTimeOffsetType)
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
                    if (type == TimeSpanType || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0] == TimeSpanType))
                    {
                        return (CreateDelegateStruct<T, TimeSpan>(property, nullable), code, isArray, StructType.TimeSpan);
                    }
                    if (type == GuidType || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0] == GuidType))
                    {
                        return (CreateDelegateStruct<T, Guid>(property, nullable), code, isArray, StructType.Guid);
                    }
                    if (type == DateTimeOffsetType || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0] == DateTimeOffsetType))
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
            if (setter.IsPrivate)
            {
                return null;
            }
            return Delegate.CreateDelegate(typeof(Action<T, TProp>), setter);
        }

        private static Delegate CreateDelegateStruct<T, TProp>(PropertyInfo property, bool nullable) where TProp : struct
        {
            var setter = property.GetSetMethod(true);
            if (setter == null)
            {
                return null;
            }
            if (setter.IsPublic)
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
                            ((Action<T, DateTimeOffset?>)method).Invoke(instance, new DateTimeOffset((DateTime)value));
                        }
                    }
                    else
                    {
                        ((Action<T, DateTimeOffset>)method).Invoke(instance, new DateTimeOffset((DateTime)value));
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