using System;
using System.Data.Common;

namespace Norm
{
    public partial class Norm
    {
        internal static void Parse(ref string input)
        {
            if (NormOptions.Value.KeepOriginalNames)
            {
                input = input.ToLowerInvariant();
                return;
            }
            var result = new Span<char>(new char[input.Length]);
            int index = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                if (ch != '@' && ch != '_')
                {
                    result[index] = char.ToLowerInvariant(input[i]);
                    index++;
                }
            }
            input = result[..index].ToString();
        }

        internal void ResetNames()
        {
            // For GC Ready!
            names = null;
        }


        internal ReadOnlyMemory<(string name, object value)> ReadToArray(DbDataReader reader)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            bool hasNames = names != null;
            if (!hasNames)
            {
                names = new string[count];
            }
            var result = new (string name, object value)[count];
            for (var index = 0; index < count; index++)
            {
                if (hasNames)
                {
                    n = names[index];
                }
                else
                {
                    n = reader.GetName(index);
                    Parse(ref n);
                    names[index] = n;
                }
                v = reader.GetValue(index);
                if (v == DBNull.Value) r = null; else r = v;
                result[index] = (n, r);
            }
            return new ReadOnlyMemory<(string name, object value)>(result);
        }

        internal ReadOnlyMemory<(string name, object value)> ReadToArray(DbDataReader reader,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            bool hasNames = names != null;
            if (!hasNames)
            {
                names = new string[count];
            }
            var result = new (string name, object value)[count];
            for (var index = 0; index < count; index++)
            {
                if (hasNames)
                {
                    n = names[index];
                }
                else
                {
                    n = reader.GetName(index);
                    Parse(ref n);
                    names[index] = n;
                }
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

        internal ReadOnlyMemory<(string name, object value, bool set)> ReadToArrayWithSet(DbDataReader reader)
        {
            var count = reader.FieldCount;
            object v;
            object r;
            string n;
            bool hasNames = names != null;
            if (!hasNames)
            {
                names = new string[count];
            }
            var result = new (string name, object value, bool set)[count];
            for (var index = 0; index < count; index++)
            {
                if (hasNames)
                {
                    n = names[index];
                }
                else
                {
                    n = reader.GetName(index);
                    Parse(ref n);
                    names[index] = n;
                }
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