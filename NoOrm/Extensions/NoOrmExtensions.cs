using System.Collections.Generic;
using System.Linq;

namespace NoOrm.Extensions
{
    public static class NoOrmExtensions
    {
        public static IDictionary<string, object> ToDictionary(this IEnumerable<(string name, object value)> tuples) =>
            tuples.ToDictionary(t => t.name, t => t.value);

        public static IEnumerable<IDictionary<string, object>> ToDictionaries(this IEnumerable<IEnumerable<(string name, object value)>> tuples) => 
            tuples.Select(t => t.ToDictionary());
    }
}
