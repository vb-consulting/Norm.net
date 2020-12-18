using System.Linq;

namespace Norm
{
    public enum NameConvention { None, LowerCamelCase, CamelCase, LowerCase }

    public static class SqlOptions
    {
        public static NameConvention NameConvention { get; set; } = NameConvention.LowerCamelCase;

        public static string Name(this string name)
        {
            return NameConvention switch
            {
                NameConvention.None => name,
                NameConvention.LowerCamelCase => string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower(),
                NameConvention.CamelCase => string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())),
                NameConvention.LowerCase => name.ToLower(),
                _ => throw new System.NotImplementedException()
            };
        }

        public static string As(this string name, string @as)
        {
            return $"{name.Name()} as {@as}";
        }
    }
}