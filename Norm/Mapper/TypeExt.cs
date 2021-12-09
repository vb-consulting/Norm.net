using System;
using System.Collections.Generic;

namespace Norm
{
    public static class TypeExt
    {
        private static readonly Type TimeSpanType = typeof(TimeSpan);
        private static readonly Type GuidType = typeof(Guid);
        private static readonly Type DateTimeOffsetType = typeof(DateTimeOffset);
        
        private static readonly HashSet<Type> ValueTupleTypes = new HashSet<Type>(
            new[]
            {
                typeof(ValueTuple<>),
                typeof(ValueTuple<,>),
                typeof(ValueTuple<,,>),
                typeof(ValueTuple<,,,>),
                typeof(ValueTuple<,,,,>),
                typeof(ValueTuple<,,,,,>),
                typeof(ValueTuple<,,,,,,>),
                typeof(ValueTuple<,,,,,,,>)
            });

        internal static (Type type, bool simple, bool valueTuple) GetMetadata(this Type type)
        {
            var simple = (type, true, false);
            if (type.IsPrimitive)
            {
                return simple;
            }

            TypeCode code;
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                code = Type.GetTypeCode(elementType);
                if (code == TypeCode.Object && elementType == TimeSpanType
                    || code == TypeCode.Object && elementType == GuidType
                    || code == TypeCode.Object && elementType == DateTimeOffsetType)
                {
                    return simple;
                }
            }
            else
            {
                if (type.IsGenericType)
                {
                    if (ValueTupleTypes.Contains(type.GetGenericTypeDefinition()))
                    {
                        return (type, true, true);
                    }

                    code = Type.GetTypeCode(type.GenericTypeArguments[0]);
                    if (code == TypeCode.Object && type.GenericTypeArguments[0] == TimeSpanType
                        || code == TypeCode.Object && type.GenericTypeArguments[0] == GuidType
                        || code == TypeCode.Object && type.GenericTypeArguments[0] == DateTimeOffsetType)
                    {
                        return simple;
                    }
                }
                else
                {
                    code = Type.GetTypeCode(type);
                    if (code == TypeCode.Object && type == TimeSpanType
                        || code == TypeCode.Object && type == GuidType
                        || code == TypeCode.Object && type == DateTimeOffsetType)
                    {
                        return simple;
                    }
                }
            }

            return code switch
            {
                TypeCode.Int32 => simple,
                TypeCode.DateTime => simple,
                TypeCode.String => simple,
                TypeCode.Boolean => simple,
                TypeCode.Byte => simple,
                TypeCode.Char => simple,
                TypeCode.Decimal => simple,
                TypeCode.Double => simple,
                TypeCode.Int16 => simple,
                TypeCode.Int64 => simple,
                TypeCode.SByte => simple,
                TypeCode.Single => simple,
                TypeCode.UInt16 => simple,
                TypeCode.UInt32 => simple,
                TypeCode.UInt64 => simple,
                _ => (type, false, false),
            };
        }
    }
}