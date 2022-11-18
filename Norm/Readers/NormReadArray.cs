using System;
using System.Data.Common;

namespace Norm
{
    public partial class Norm
    {
        public static ReadOnlyMemory<(string name, object value)> ReadToArray(DbDataReader reader)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            var result = new (string name, object value)[count];
            for (var index = 0; index < count; index++)
            {
                n = reader.GetName(index);
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r);
            }
            return new ReadOnlyMemory<(string name, object value)>(result);
        }

        public static ReadOnlyMemory<(string name, object value)> ReadToArray(DbDataReader reader,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            var result = new (string name, object value)[count];
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
            return new ReadOnlyMemory<(string name, object value)>(result);
        }

        protected ReadOnlyMemory<(string name, object value, bool set)> ReadToArrayWithSet(DbDataReader reader)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            var result = new (string name, object value, bool set)[count];
            for (var index = 0; index < count; index++)
            {
                n = reader.GetName(index);
                var callback = readerCallback((n, index, reader));
                if (callback != null)
                {
                    result[index] = (n, callback == DBNull.Value ? null : callback, true);
                    continue;
                }
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r, false);
            }
            return new ReadOnlyMemory<(string name, object value, bool set)>(result);
        }
    }
}