using System.Linq;

namespace Norm
{
    public enum NameConvention { None, CamelCase, LowerCase }

    public class SqlOptions
    {
        public NameConvention NameConvention { get; set; }
    }

    public abstract class SqlBase 
    {
        protected SqlOptions Options = new SqlOptions 
        { 
            NameConvention = NameConvention.CamelCase 
        };

        public void SetOptions(SqlOptions options)
        {
            Options = options;
        }

        protected string TranslateName(string name)
        {
            return Options.NameConvention switch
            {
                NameConvention.None => name,
                NameConvention.CamelCase => string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower(),
                NameConvention.LowerCase => name.ToLower(),
                _ => throw new System.NotImplementedException()
            };
        }
    }
}