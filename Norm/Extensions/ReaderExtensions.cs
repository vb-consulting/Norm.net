using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Norm.Extensions
{
    public static class ReaderExtensions
    {
        public static IList<(string name, object value)> ToList(this DbDataReader reader)
        {
            var result = new List<(string name, object value)>(reader.FieldCount);
            for (var index = 0; index < reader.FieldCount; index++)
            {
                result.Add((reader.GetName(index), reader.GetValue(index)));
            }

            return result;
        }
    }
}
