using System;
using System.Collections.Generic;
using System.Linq;


namespace Norm.Extensions
{
    public static partial class NormExtensions
    {
        /// <summary>
        /// Add expression to build a dictionary from (name, value) tuple
        /// </summary>
        public static IDictionary<string, object> SelectDictionary(this IEnumerable<(string name, object value)> tuples) =>
            tuples.ToDictionary(t => t.name, t => t.value);

        /// <summary>
        /// Add expression to build a enumerator of dictionaries from enumerator of (name, value) tuples
        /// </summary>
        public static IEnumerable<IDictionary<string, object>> SelectDictionaries(this IEnumerable<IEnumerable<(string name, object value)>> tuples) =>
            tuples.Select(t => t.SelectDictionary());


        /// <summary>
        /// Add expression to build a enumerator of dictionaries from enumerator of (name, value) tuples
        /// </summary>
        public static async IAsyncEnumerable<IDictionary<string, object>> SelectDictionariesAsync(this IAsyncEnumerable<IEnumerable<(string name, object value)>> tuples)
        {
            await foreach (var tuple in tuples)
            {
                yield return tuple.SelectDictionary();
            }
        }

        /// <summary>
        /// Select single values  enumeration (without name)
        /// </summary>
        public static IEnumerable<object> SelectValues(this IEnumerable<(string name, object value)> tuples) =>
            tuples.Select(t => t.value);

        /// <summary>
        /// Select single values  enumeration (without name)
        /// </summary>
        public static IEnumerable<IEnumerable<object>> SelectValues(this IEnumerable<IEnumerable<(string name, object value)>> tuples) =>
            tuples.Select(t => t.SelectValues());
    }
}