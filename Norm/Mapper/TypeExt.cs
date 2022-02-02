using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Norm
{
    public static class TypeExt
    {
        internal static readonly Type TimeSpanType = typeof(TimeSpan);
        internal static readonly Type GuidType = typeof(Guid);
        internal static readonly Type DateTimeOffsetType = typeof(DateTimeOffset);
        internal static readonly Type StringType = typeof(string);
        internal static readonly Type IntType = typeof(int);
        internal static readonly Type NullableIntType = typeof(int?);

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

        internal static bool IsAnonymousType(this Type type)
        {
            if (Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && type.Attributes.HasFlag(TypeAttributes.NotPublic))
            {
                return true;
            }
            return false;
        }

        internal static (Type type, bool simple, bool valueTuple, bool isString) GetMetadata(this Type type)
        {
            var simpleNotString = (type, true, false, false);
            var simpleString = (type, true, false, true);
            if (type.IsPrimitive)
            {
                return simpleNotString;
            }

            TypeCode code;
            if (type.IsArray)
            {
                return simpleNotString;
            }
            else
            {
                if (type.IsGenericType)
                {
                    if (ValueTupleTypes.Contains(type.GetGenericTypeDefinition()))
                    {
                        return (type, true, true, false);
                    }

                    code = Type.GetTypeCode(type.GenericTypeArguments[0]);
                    if (code == TypeCode.Object && type.GenericTypeArguments[0] == TimeSpanType
                        || code == TypeCode.Object && type.GenericTypeArguments[0] == GuidType
                        || code == TypeCode.Object && type.GenericTypeArguments[0] == DateTimeOffsetType)
                    {
                        return simpleNotString;
                    }
                }
                else
                {
                    code = Type.GetTypeCode(type);
                    if (code == TypeCode.Object && type == TimeSpanType
                        || code == TypeCode.Object && type == GuidType
                        || code == TypeCode.Object && type == DateTimeOffsetType)
                    {
                        return simpleNotString;
                    }
                }
            }

            return code switch
            {
                TypeCode.Int32 => simpleNotString,
                TypeCode.DateTime => simpleNotString,
                TypeCode.String => simpleString,
                TypeCode.Boolean => simpleNotString,
                TypeCode.Byte => simpleNotString,
                TypeCode.Char => simpleNotString,
                TypeCode.Decimal => simpleNotString,
                TypeCode.Double => simpleNotString,
                TypeCode.Int16 => simpleNotString,
                TypeCode.Int64 => simpleNotString,
                TypeCode.SByte => simpleNotString,
                TypeCode.Single => simpleNotString,
                TypeCode.UInt16 => simpleNotString,
                TypeCode.UInt32 => simpleNotString,
                TypeCode.UInt64 => simpleNotString,
                _ => (type, false, false, false),
            };
        }
    }
}