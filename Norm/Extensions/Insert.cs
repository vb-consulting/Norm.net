using System.Collections.Generic;
using System.Data.Common;

namespace Norm
{
    public static partial class ConnectionExtensions
    {
        //connection.Insert<TestClass>(nameof(TestClass.Id)).Values(1, "a").Values(2, "b").Execute();
        public static ValuesBuilder<T> Insert<T>(this DbConnection connection)
        {
            var (name, fields, _) = GetCommandData<T>();
            return new ValuesBuilder<T>(connection, name, fields);
            //return connection.GetNoOrmInstance().Read(select).Select<T>();
        }

        public static ValuesBuilder<T> Insert<T>(this DbConnection connection, params string[] names)
        {
            var (name, _, _) = GetCommandData<T>();
            return new ValuesBuilder<T>(connection, name, names);
            //return connection.GetNoOrmInstance().Read(select).Select<T>();
        }
    }

    public class ValuesBuilder<T>
    {
        private readonly Norm norm;
        private readonly string name;
        private readonly string[] fields;

        public ValuesBuilder(DbConnection connection, string name, string[] fields)
        {
            this.norm = connection.GetNoOrmInstance();
            this.name = name;
            this.fields = fields;

            //var norm = connection.GetNoOrmInstance();
            //norm.
        }

        public ValuesBuilder<T> Values(params object[] values)
        {
            return this;
        }

        public ValuesBuilder<T> Values(T instance)
        {
            return this;
        }

        public void Execute()
        {
        }

        public IEnumerable<T> Returning()
        {
            return null;
        }
    }
}
