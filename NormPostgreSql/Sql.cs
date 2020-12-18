namespace Norm
{
    public static partial class Sql
    {
        public static SqlValue Value(object value) => new SqlValue(value);
        public static SqlString String(string value) => new SqlString(value);
    }

    public class SqlValue
    {
        protected readonly object value;

        public SqlValue(object value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public string As(string @as)
        {
            return $"{this} as {@as}";
        }
    }

    public class SqlString : SqlValue
    {
        public SqlString(string value) : base(value)
        {
        }

        public override string ToString()
        {
            return $"'{value}'";
        }
    }
}
