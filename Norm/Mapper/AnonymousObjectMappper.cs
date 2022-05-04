using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Norm
{
    public static partial class NormExtensions
    {
        internal static IEnumerable<T> MapAnonymous<T>(this IEnumerable<(string name, object value)[]> tuples, 
            Type type1)
        {
            if (!type1.IsAnonymousType())
            {
                throw new ArgumentException("Anonymous Type is required for this call.");
            }
            var ctorInfo1 = TypeCache<T>.GetAnonInfo(type1);
            Dictionary<string, ushort> names = null;
            foreach (var tuple in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(tuple);
                }
                var parameters = new object[ctorInfo1.Props.Length];
                
                ushort i = 0;
                foreach (var (Name, Type) in ctorInfo1.Props)
                {
                    object value;
                    if (!names.TryGetValue(Name, out var index))
                    {
                        value = default;
                    }
                    else
                    {
                        value = tuple[index].value;
                    }
                    parameters[i] = value;
                    i++;
                }
                yield return (T)ctorInfo1.CtorInfo.Invoke(parameters);
            }
        }
    }
}