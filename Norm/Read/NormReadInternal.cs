using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Norm
{
    public partial class Norm
    {
        internal IEnumerable<T> ReadInternal<T>(string command, Func<DbDataReader, T> readerAction)
        {
            using var cmd = CreateCommand(command);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        internal IEnumerable<T> ReadInternal<T>(FormattableString command, Func<DbDataReader, T> readerAction)
        {
            using var cmd = CreateCommand(command);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        internal IEnumerable<(string name, object value)[]> ReadToArrayInternal(string command)
        {
            using var cmd = CreateCommand(command);
            using var reader = cmd.ExecuteReader();

            if (this.readerCallback == null)
            {
                while (reader.Read())
                {
                    yield return ReadToArray(reader);
                }
            }
            else
            {
                while (reader.Read())
                {
                    yield return ReadToArray(reader, this.readerCallback);
                }
            }
        }

        internal IEnumerable<(string name, object value, bool set)[]> ReadToArrayWithCallbackInternal(string command)
        {
            using var cmd = CreateCommand(command);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return ReadToArrayWithSet(reader);
            }
        }

        internal IEnumerable<(string name, object value)[]> ReadToArrayInternal(FormattableString command)
        {
            using var cmd = CreateCommand(command);
            using var reader = cmd.ExecuteReader();
            if (this.readerCallback == null)
            {
                while (reader.Read())
                {
                    yield return ReadToArray(reader);
                }
            }
            else
            {
                while (reader.Read())
                {
                    yield return ReadToArray(reader, readerCallback);
                }
            }
        }

        internal IEnumerable<(string name, object value, bool set)[]> ReadToArrayWithCallbackInternal(FormattableString command)
        {
            using var cmd = CreateCommand(command);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return ReadToArrayWithSet(reader);
            }
        }

        internal (string name, object value)[] ReadToArray(DbDataReader reader)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            (string name, object value)[] result = new (string name, object value)[count];
            for (var index = 0; index < count; index++)
            {
                n = reader.GetName(index);
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r);
            }
            return result;
        }

        internal (string name, object value)[] ReadToArray(DbDataReader reader,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            (string name, object value)[] result = new (string name, object value)[count];
            for (var index = 0; index < count; index++)
            {
                n = reader.GetName(index);
                var callback = readerCallback((n, index, reader));
                if (callback != null)
                {
                    result[index] = (n, callback == DBNull.Value ? null : callback);
                    continue;
                }
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r);
            }
            return result;
        }

        internal (string name, object value, bool set)[] ReadToArrayWithSet(DbDataReader reader)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            (string name, object value, bool set)[] result = new (string name, object value, bool set)[count];
            for (var index = 0; index < count; index++)
            {
                n = reader.GetName(index);
                var callback = readerCallback((n, index, reader));
                if (callback != null)
                {
                    result[index] = (n, callback == DBNull.Value ? null : callback, true);
                    continue;
                }
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r, false);
            }
            return result;
        }

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
    }
}