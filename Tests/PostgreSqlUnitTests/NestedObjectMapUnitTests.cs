using System.Collections.Generic;
using System.Linq;
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
            public int Id { get; set; }
            public string Name { get; set; }
            public IList<Account> Accounts { get; set; }
        }

        public class Account
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Country { get; set; }
            public int ShopId { get; set; }
            public Shop Shop { get; set; }
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

            var shops = connection.Read<Shop, Account>(@"
                select s.*, a.*
                from shop s 
                inner join account a on s.id = a.shop_id")
                .GroupBy(item => item.Item1.Id)
                .Select(group =>
                {
                    var shop = group.First().Item1;
                    shop.Accounts = group.Select(item =>
                    {
                        var account = item.Item2;
                        account.Shop = shop;
                        return account;
                    }).ToList();
                    return shop;
                })
                .ToList();

            Assert.Equal(2, shops.Count);

            Assert.Equal(1, shops[0].Id);
            Assert.Equal(2, shops[1].Id);
            Assert.Equal("shop1", shops[0].Name);
            Assert.Equal("shop2", shops[1].Name);

            Assert.Equal(3, shops[0].Accounts.Count);
            Assert.Equal(2, shops[1].Accounts.Count);

            Assert.Equal("account3", shops[0].Accounts[0].Name);
            Assert.Equal("account2", shops[0].Accounts[1].Name);
            Assert.Equal("account1", shops[0].Accounts[2].Name);
            Assert.Equal("addr3", shops[0].Accounts[0].Address);
            Assert.Equal("addr2", shops[0].Accounts[1].Address);
            Assert.Equal("addr1", shops[0].Accounts[2].Address);
            Assert.Equal("country3", shops[0].Accounts[0].Country);
            Assert.Equal("country2", shops[0].Accounts[1].Country);
            Assert.Equal("country1", shops[0].Accounts[2].Country);

            Assert.Equal(shops[0], shops[0].Accounts[0].Shop);
            Assert.Equal(shops[0], shops[0].Accounts[1].Shop);
            Assert.Equal(shops[0], shops[0].Accounts[2].Shop);

            Assert.Equal(3, shops[0].Accounts[0].Shop.Accounts.Count);
            Assert.Equal(3, shops[0].Accounts[1].Shop.Accounts.Count);
            Assert.Equal(3, shops[0].Accounts[2].Shop.Accounts.Count);

            Assert.Equal("account5", shops[1].Accounts[0].Name);
            Assert.Equal("account4", shops[1].Accounts[1].Name);
            Assert.Equal("addr5", shops[1].Accounts[0].Address);
            Assert.Equal("addr4", shops[1].Accounts[1].Address);
            Assert.Equal("country5", shops[1].Accounts[0].Country);
            Assert.Equal("country4", shops[1].Accounts[1].Country);

            Assert.Equal(shops[1], shops[1].Accounts[0].Shop);
            Assert.Equal(shops[1], shops[1].Accounts[1].Shop);

            Assert.Equal(2, shops[1].Accounts[0].Shop.Accounts.Count);
            Assert.Equal(2, shops[1].Accounts[1].Shop.Accounts.Count);

            connection
                .Execute("rollback");
        }
    }
}
