using System;
using System.Collections.Generic;

namespace Norm
{
    public static class SqlMapper
    {
        /// <summary>
        /// Define global custom mapping for class type by defining a row handler with name and value tuple array.
        /// </summary>
        /// <typeparam name="T">Class type that will register for custom mapping.</typeparam>
        /// <param name="handler">Function handler that sends a name and value tuple array as aparameter for each row.</param>
        public static void AddTypeByTuples<T>(Func<(string name, object value)[], T> handler) where T : class
        {
            TypeCache<T>.CustomMapTuples = handler;
        }

        /// <summary>
        /// Define global custom mapping for class type by defining a row handler with value array.
        /// </summary>
        /// <typeparam name="T">Class type that will register for custom mapping.</typeparam>
        /// <param name="handler">Function handler that sends a value array as aparameter for each row.</param>
        public static void AddTypeByValues<T>(Func<object[], T> handler) where T : class
        {
            TypeCache<T>.CustomMapValues = handler;
        }

        /// <summary>
        /// Define global custom mapping for class type by defining a row handler with dictionary of names and values.
        /// </summary>
        /// <typeparam name="T">Class type that will register for custom mapping.</typeparam>
        /// <param name="handler">Function handler that sends a dictionary of names and values as aparameter for each row.</param>
        public static void AddTypeByDict<T>(Func<IDictionary<string, object>, T> handler) where T : class
        {
            TypeCache<T>.CustomMapDict = handler;
        }
    }
}
