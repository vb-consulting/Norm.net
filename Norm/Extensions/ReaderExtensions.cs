using System;
using System.Data.Common;

namespace Norm.Extensions
{
    public static class ReaderExtensions
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
    }
}
