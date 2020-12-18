using System;
using System.Linq;
using System.Text;

namespace Norm
{
    public static partial class Sql
    {
        public static Select Select(params object[] fields) => new Select(fields);
        public static Select<T> Select<T>() => new Select<T>();
        public static Select<T> Select<T>(params object[] fields) => new Select<T>(fields);
        public static Select<T> Select<T>(Func<T, object> selector) => new Select<T>(selector);
    }

    public class Select
    {
        protected string name;
        protected string[] fields;

        public Select(params object[] fields)
        {
            this.fields = fields.Select(f =>
            {
                if (f.GetType() == typeof(SqlValue) || f.GetType().BaseType == typeof(SqlValue))
                {
                    return f.ToString();
                }
                return f.ToString().Name();
            }).ToArray();
        }

        public override string ToString()
        {
            var result = new StringBuilder("select ");
            var len = fields.Length;
            var i = 0;
            foreach (var field in fields)
            {
                result.Append($"{field}{(++i == len ? "" : ", ")}");
            }
            if (name != null)
            {
                result.Append($" from {name}");
            }
            return result.ToString();
        }
    }

    public class Select<T> : Select
    {
        public Select(params object[] fields) : base(fields)
        {
            name = typeof(T).Name.Name();
        }

        public Select()
        {
            var type = typeof(T);
            name = type.Name.Name();
            fields = TypeCache<T>.GetProperties(type).Select(p => p.Name.Name()).ToArray();
        }

        public Select(Func<T, object> selector)
        {
            name = typeof(T).Name.Name();
            var instance = TypeCache<T>.CreateInstance();
            var selection = selector(instance);
            fields = selection.GetType().GetProperties().Select(p => 
            {
                if (p.PropertyType.BaseType.BaseType == typeof(SqlFun) || p.PropertyType.BaseType == typeof(SqlFun))
                {
                    var value = p.GetValue(selection) as SqlFun;
                    return $"{value.ToValue()} as {p.Name.Name()}";
                }
                if (p.PropertyType == typeof(SqlValue) || p.PropertyType.BaseType == typeof(SqlValue))
                {
                    var value = p.GetValue(selection) as SqlValue;
                    return $"{value} as {p.Name.Name()}";
                }
                if (p.PropertyType == typeof(string))
                {
                    var value = p.GetValue(selection) as string;
                    if (value != default)
                    {
                        return $"{value.Name()} as {p.Name.Name()}";
                    }
                }
                return p.Name.Name(); 
            }).ToArray();
        }
    }
}
