using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Norm
{
    public partial class Norm
    {
        public (string name, object value)[] ReadToArray(DbDataReader reader)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            (string name, object value)[] result = new (string name, object value)[count];
            for (var index = 0; index < count; index++)
            {
                n = reader.GetName(index);
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r);
            }
            return result;
        }

        public (string name, object value)[] ReadToArray(DbDataReader reader,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            (string name, object value)[] result = new (string name, object value)[count];
            for (var index = 0; index < count; index++)
            {
                n = reader.GetName(index);
                var callback = readerCallback((n, index, reader));
                if (callback != null)
                {
                    result[index] = (n, callback == DBNull.Value ? null : callback);
                    continue;
                }
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r);
            }
            return result;
        }
    }
}