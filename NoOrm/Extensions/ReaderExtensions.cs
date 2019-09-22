using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NoOrm
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

        public static IDictionary<string, object> ToDictionary(this IEnumerable<(string name, object value)> tuples) =>
            tuples.ToDictionary(t => t.name, t => t.value);

        public static IEnumerable<IDictionary<string, object>> ToDictionaries(this IEnumerable<IEnumerable<(string name, object value)>> tuples) => 
            tuples.Select(t => t.ToDictionary());
    }
}
