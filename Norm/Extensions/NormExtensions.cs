using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Norm.Extensions
{
    public static class NormExtensions
    {
        /// <summary>
        /// Add expression to build a dictionary from (name, value) tuple
        /// </summary>
        public static IDictionary<string, object> SelectDictionary(this IEnumerable<(string name, object value)> tuples) =>
            tuples.ToDictionary(t => t.name, t => t.value);

        /// <summary>
        /// Add expression to build a enumerator of dictionaries from enumerator of (name, value) tuples
        /// </summary>
        public static IEnumerable<IDictionary<string, object>> SelectDictionaries(
            this IEnumerable<IEnumerable<(string name, object value)>> tuples) =>
            tuples.Select(t => t.SelectDictionary());

        /// <summary>
        /// Add expression to build a enumerator of lists of (name, value) tuples
        /// </summary>
        public static IEnumerable<List<(string, object)>> SelectToLists(
            this IEnumerable<IEnumerable<(string name, object value)>> tuples) =>
            tuples.Select(t => t.ToList());

        /// <summary>
        /// Builds a list of lists of (name, value) tuples
        /// </summary>
        public static List<List<(string, object)>> ToListOfLists(
            this IEnumerable<IEnumerable<(string name, object value)>> tuples) =>
            tuples.SelectToLists().ToList();

        /// <summary>
        /// Add expression to build a dictionary from (name, value) tuple
        /// </summary>
        public static async ValueTask<IDictionary<string, object>> SelectDictionaryAsync(
            this IAsyncEnumerable<(string name, object value)> tuples) =>
            await tuples.ToDictionaryAsync(t => t.name, t => t.value);

        /// <summary>
        /// Add expression to build a enumerator of dictionaries from enumerator of (name, value) tuples
        /// </summary>
        public static async IAsyncEnumerable<IDictionary<string, object>> SelectDictionariesAsync(
            this IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> tuples)
        {
            await foreach (var tuple in tuples)
            {
                yield return await tuple.SelectDictionaryAsync();
            }
        }

        /// <summary>
        /// Add expression to build a enumerator of lists of (name, value) tuples
        /// </summary>
        public static IAsyncEnumerable<List<(string, object)>> SelectToListsAsync(
            this IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> tuples) =>
            tuples.SelectAwait(t => t.ToListAsync());


        /// <summary>
        /// Builds a list of lists of (name, value) tuples
        /// </summary>
        public static async ValueTask<List<List<(string, object)>>> ToListOfListsAsync(
            this IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> tuples) =>
            await tuples.SelectToListsAsync().ToListAsync();
    }
}