using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class NestedObjectMapUnitTests
    {
        private readonly PostgreSqlFixture fixture;

      
        public NestedObjectMapUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        public class Shop
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public IList<Account> Accounts { get; set; }
        }

        public class Account
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Country { get; set; }
            public int ShopId { get; set; }
        }

        [Fact]
        public void NestedObjectMapUnitTests_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .Execute("begin")
                .Execute("create table shop (id int, name text)")
                .Execute("create table account (id int, name text, address text, country text, shop_id int)")
                .Execute("insert into shop (id, name) values (1, 'shop1'), (2, 'shop2'), (3, 'shop3')")
                .Execute(@"insert into account (id, name, address, country, shop_id) values 
                                (1, 'account1', 'addr1', 'country1', 1), 
                                (2, 'account2', 'addr2', 'country2', 1), 
                                (3, 'account3', 'addr3', 'country3', 1), 
                                (4, 'account4', 'addr4', 'country4', 2), 
                                (5, 'account5', 'addr5', 'country5', 2)");

            var result = connection.Read<Shop, Account>(@"
                select s.*, a.*
                from shop s 
                inner join account a on s.id = a.shop_id
            ")
                .ToList();


            connection
                .Execute("rollback");
        }
    }
}
