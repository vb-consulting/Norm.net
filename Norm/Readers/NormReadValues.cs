using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        internal T GetFieldValue<T>(DbDataReader reader, int ordinal,Type type)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            if (type.IsEnum || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum))
            {
                var fieldType = reader.GetFieldType(ordinal);
                if (fieldType == typeof(string))
                {
                    if (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum)
                    {
                        return (T)Enum.Parse(type.GenericTypeArguments[0], reader.GetString(ordinal));
                    }
                    return (T)Enum.Parse(type, reader.GetString(ordinal));
                }
                if (fieldType == typeof(int))
                {
                    if (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum)
                    {
                        return (T)Enum.ToObject(type.GenericTypeArguments[0], reader.GetInt32(ordinal));
                    }
                    return (T)Enum.ToObject(type, reader.GetInt32(ordinal));
                }
            }

            return reader.GetFieldValue<T>(ordinal);
        }

        internal T GetFieldValueWithCallback<T>(DbDataReader reader, int ordinal, Type type)
        {
            var name = reader.GetName(ordinal);
            var callback = readerCallback((name, ordinal, reader));
            if (callback == null)
            {
                return GetFieldValue<T>(reader, ordinal, type);
            }
            return (T)(callback == DBNull.Value ? null : callback);
        }

        internal async ValueTask<T> GetFieldValueAsync<T>(DbDataReader reader, int ordinal, Type type)
        {
            if (await reader.IsDBNullAsync(ordinal))
            {
                return default;
            }

            if (type.IsEnum || (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum))
            {
                var fieldType = reader.GetFieldType(ordinal);
                if (fieldType == typeof(string))
                {
                    if (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum)
                    {
                        return (T)Enum.Parse(type.GenericTypeArguments[0], reader.GetString(ordinal));
                    }
                    return (T)Enum.Parse(type, reader.GetString(ordinal));
                }
                if (fieldType == typeof(int))
                {
                    if (type.GenericTypeArguments.Length > 0 && type.GenericTypeArguments[0].IsEnum)
                    {
                        return (T)Enum.ToObject(type.GenericTypeArguments[0], reader.GetInt32(ordinal));
                    }
                    return (T)Enum.ToObject(type, reader.GetInt32(ordinal));
                }
            }

            return await reader.GetFieldValueAsync<T>(ordinal);
        }

        internal async ValueTask<T> GetFieldValueWithReaderCallbackAsync<T>(DbDataReader reader, int ordinal, Type type)
        {
            var name = reader.GetName(ordinal);
            var callback = readerCallback((name, ordinal, reader));
            if (callback == null)
            {
                return await GetFieldValueAsync<T>(reader, ordinal, type);
            }
            return (T)(callback == DBNull.Value ? null : callback);
        }
    }
}