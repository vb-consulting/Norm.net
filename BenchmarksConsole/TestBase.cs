using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BenchmarksConsole
{
    public abstract class TestBase
    {
        protected readonly NpgsqlConnection connection;

        public TestBase(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public static string GetQuery(int records)
        {
            return $@"
    select 
        i as id1, 
        'foo' || i::text as foo1, 
        'bar' || i::text as bar1, 
        ('2000-01-01'::date) + (i::text || ' days')::interval as datetime1, 
        i+1 as id2, 
        'foo' || (i+1)::text as foo2, 
        'bar' || (i+1)::text as bar2, 
        ('2000-01-01'::date) + ((i+1)::text || ' days')::interval as datetime2,
        'long_foo_bar_' || (i+2)::text as longfoobar, 
        (i % 2)::boolean as isfoobar
    from generate_series(1, {records}) as i
";
        }

        public static double Kb(long kilobytes)
        {
            return kilobytes / 1024f;
        }
    }
}
