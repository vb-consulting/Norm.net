using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Norm
{
    public static class Sql
    {
        public static Select<T> Select<T>() => new Select<T>();
        public static Select<T> Select<T>(Func<T, object> selector) => new Select<T>(selector);
    }

    delegate void Params<T>(params object[] args);

    public class Select<T> : SqlBase
    {
        protected string name;
        protected string[] fields;

        private string aliasTemplate => $"t{tableCount++}";
        private ushort tableCount = 0;

        public Select()
        {
            var type = typeof(T);
            this.name = TranslateName(type.Name);
            this.fields = TypeCache<T>.GetProperties(type).Select(p => TranslateName(p.Name)).ToArray();
        }

        public Select(Func<T, object> selector)
        {
            this.name = TranslateName(typeof(T).Name);
            var instance = TypeCache<T>.CreateInstance();
            var selection = selector(instance);
            this.fields = selection.GetType().GetProperties().Select(p => TranslateName(p.Name)).ToArray();
        }

        public override string ToString()
        {
            var result = new StringBuilder("select");
            result.AppendLine();
            var len = fields.Length;
            var i = 0;
            foreach (var field in fields)
            {
                result.AppendLine($"    {field}{(++i == len ? "" : ",")}");
            }
            result.AppendLine($"from");
            result.AppendLine($"    {name}");
            return result.ToString();
        }
    }
}
