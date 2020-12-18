namespace Norm
{

    public abstract class SqlFun 
    {
        public abstract string ToValue();
    }

    public abstract class OneArgFun<T> : SqlFun
    {
        protected readonly string value;

        public OneArgFun(string value)
        {
            this.value = value;
        }

        public override string ToValue()
        {
            return $"{typeof(T).Name.Name()}({value.Name()})";
        }

        public override string ToString()
        {
            var val = value.Name();
            return $"{typeof(T).Name.Name()}({val}) as {val}";
        }

        public string As(string @as) => $"{this.ToValue()} as {@as}";
    }
}
