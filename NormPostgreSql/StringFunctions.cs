namespace Norm
{
    public static partial class Sql
    {
        public static class Fun
        {
            public static class String
            {
                public static Md5 Md5(string value) => new Md5(value);
            }
        }
    }

    public class Md5 : OneArgFun<Md5>
    { 
        public Md5(string value) : base(value) { }
    }
}
