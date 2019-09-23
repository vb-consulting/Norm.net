using System.Collections.Generic;
using System.Data.Common;

namespace NoOrm.Extensions
{
    public static class ReaderExtensions
    {
        public static IEnumerable<(string name, object value)> GetTuplesFromReader(this DbDataReader reader)
        {
            for (var index = 0; index < reader.FieldCount; index++)
            {
                yield return (reader.GetName(index), reader.GetValue(index));
            }
        }

        public static IEnumerable<object> GetValuesFromReader(this DbDataReader reader)
        {
            for (var index = 0; index < reader.FieldCount; index++)
            {
                yield return reader.GetValue(index);
            }
        }
    }
}
