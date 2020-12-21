using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public static partial class NormExtensions
    {
        internal static (string name, object value)[] ToArray(this DbDataReader reader)
        {
            (string name, object value)[] result = new (string name, object value)[reader.FieldCount];
            for (var index = 0; index < reader.FieldCount; index++)
            {
                var v = reader.GetValue(index);
                result[index] = (reader.GetName(index), v == DBNull.Value ? null : v);
            }
            return result;
        }

        private static readonly Type StringType = typeof(string);

        internal static T GetFieldValue<T>(this DbDataReader reader, int ordinal, bool convertsDbNull)
        {
            if (!convertsDbNull || typeof(T) == StringType)
            {
                return reader.IsDBNull(ordinal) ? default : reader.GetFieldValue<T>(ordinal);
            }
            return reader.GetFieldValue<T>(ordinal);
        }

        internal static async ValueTask<T> GetFieldValueAsync<T>(this DbDataReader reader, int ordinal, bool convertsDbNull)
        {
            if (!convertsDbNull || typeof(T) == StringType)
            {
                return await reader.IsDBNullAsync(ordinal) ? default : await reader.GetFieldValueAsync<T>(ordinal);
            }
            return await reader.GetFieldValueAsync<T>(ordinal);
        }
    }
}
