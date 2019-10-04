using System;
using System.Collections.Generic;
using System.Linq;
using FastMember;

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


        /// <summary>
        /// Select results mapped to a class instance (O/R mapping, case sensitive). 
        /// </summary>
        public static T Select<T>(this IEnumerable<(string name, object value)> tuples) where T : new()
        {
            var instance = new T();
            var acc = ObjectAccessor.Create(instance);
            foreach (var (name, value) in tuples)
            {

                acc[name] = value == DBNull.Value ? null : value;
            }
            return instance;
        }

        /// <summary>
        /// Select results mapped to a class instance (O/R mapping, case sensitive). 
        /// </summary>
        public static IEnumerable<T> Select<T>(this IEnumerable<IEnumerable<(string name, object value)>> tuples) where T : new() =>
            tuples.Select(t => t.Select<T>());

        /// <summary>
        /// Select results mapped to a class instance (O/R mapping, case sensitive). 
        /// </summary>
        public static IAsyncEnumerable<T> SelectAsync<T>(this IAsyncEnumerable<IEnumerable<(string name, object value)>> tuples) where T : new() =>
            tuples.Select(t => t.Select<T>());
    }
}