using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using NoOrm.Extensions;
using Npgsql;


namespace TestWebApiApp
{
    public class TestClass
    {
        public int Id { get; set; }
        public string Foo { get; set; }
        public string Bar { get; set; }
        public DateTime Datetime { get; set; }

        public TestClass()
        {
        }

        public TestClass(IDictionary<string, object> dictionary)
        {
            Id = (int)dictionary["id"];
            Foo = (string)dictionary["foo"];
            Bar = (string)dictionary["bar"];
            Datetime = (DateTime)dictionary["datetime"];
        }

        public TestClass((int id, string foo, string bar, DateTime dateTime) tuple)
        {
            Id = tuple.id;
            Foo = tuple.foo;
            Bar = tuple.bar;
            Datetime = tuple.dateTime;
        }
    }

    public class TestController : ControllerBase
    {
        private readonly NpgsqlConnection connection;

        private const int Count = 8000;

        private static readonly string TestQuery = $@"
            select 
                i as id, 
                'foo' || i::text as foo, 
                'bar' || i::text as bar, 
                ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
            from generate_series(1, {Count}) as i
        ";

        private static readonly string JsonTestQuery = $@"
                select to_json(t)
                from (
                    select 
                        i as id, 
                        'foo' || i::text as foo, 
                        'bar' || i::text as bar, 
                        ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
                    from generate_series(1, {Count}) as i
                ) t
        ";

        public TestController(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        [HttpGet]
        [Route("api/test-dapper-sync")]
        public IEnumerable<TestClass> GetDapperSync()
        {
            return connection.Query<TestClass>(TestQuery);
        }

        [HttpGet]
        [Route("api/test-dapper-async")]
        public async Task<IEnumerable<TestClass>> GetDapperASync()
        {
            return await connection.QueryAsync<TestClass>(TestQuery);
        }

        [HttpGet]
        [Route("api/test-norm-dict-sync")]
        public IEnumerable<IDictionary<string, object>> GetNormDictSync()
        {
            return connection.Read(TestQuery).ToDictionaries();
        }

        [HttpGet]
        [Route("api/test-norm-dict-async")]
        public IAsyncEnumerable<IDictionary<string, object>> GetNormDictAsync()
        {
            return connection.ReadAsync(TestQuery).ToDictionariesAsync();
        }

        [HttpGet]
        [Route("api/test-norm-inst-sync")]
        public IEnumerable<TestClass> GetNormInstanceSync()
        {
            return connection.Read<int, string, string, DateTime>(TestQuery).Select(tuple => new TestClass(tuple));
        }

        [HttpGet]
        [Route("api/test-norm-inst-async")]
        public IAsyncEnumerable<TestClass> GetNormInstanceAsync()
        {
            return connection.ReadAsync<int, string, string, DateTime>(TestQuery).Select(tuple => new TestClass(tuple));
        }

    }
}
