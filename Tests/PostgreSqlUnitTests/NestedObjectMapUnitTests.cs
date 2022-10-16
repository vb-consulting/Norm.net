using System.Collections.Generic;
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

            Dictionary<int, Shop> shops = new();
            Dictionary<int, Account> accounts = new();

            foreach (var (shop, account) in connection.Read<Shop, Account>(@"
                select s.*, a.*
                from shop s 
                inner join account a on s.id = a.shop_id"))
            {
                accounts[account.Id] = account;
                if (shops.TryGetValue(shop.Id, out var existing))
                {
                    existing.Accounts.Add(account);
                    account.Shop = existing;
                }
                else
                {
                    shop.Accounts = new List<Account> { account };
                    shops.Add(shop.Id, shop);
                    account.Shop = shop;
                }
            }

            Assert.Equal(2, shops.Count);
            Assert.Equal(5, accounts.Count);

            Assert.Equal("shop1", shops[1].Name);
            Assert.Equal("shop2", shops[2].Name);

            Assert.Equal("account1", accounts[1].Name);
            Assert.Equal("account2", accounts[2].Name);
            Assert.Equal("account3", accounts[3].Name);
            Assert.Equal("account4", accounts[4].Name);
            Assert.Equal("account5", accounts[5].Name);

            Assert.Equal("addr1", accounts[1].Address);
            Assert.Equal("addr2", accounts[2].Address);
            Assert.Equal("addr3", accounts[3].Address);
            Assert.Equal("addr4", accounts[4].Address);
            Assert.Equal("addr5", accounts[5].Address);

            Assert.Equal("country1", accounts[1].Country);
            Assert.Equal("country2", accounts[2].Country);
            Assert.Equal("country3", accounts[3].Country);
            Assert.Equal("country4", accounts[4].Country);
            Assert.Equal("country5", accounts[5].Country);

            Assert.Equal(1, accounts[1].ShopId);
            Assert.Equal(1, accounts[2].ShopId);
            Assert.Equal(1, accounts[3].ShopId);
            Assert.Equal(2, accounts[4].ShopId);
            Assert.Equal(2, accounts[5].ShopId);

            Assert.Equal(shops[1], accounts[1].Shop);
            Assert.Equal(shops[1], accounts[2].Shop);
            Assert.Equal(shops[1], accounts[3].Shop);
            Assert.Equal(shops[2], accounts[4].Shop);
            Assert.Equal(shops[2], accounts[5].Shop);

            Assert.Equal(3, shops[1].Accounts.Count);
            Assert.Equal(2, shops[2].Accounts.Count);

            Assert.Equal(accounts[3], shops[1].Accounts[0]);
            Assert.Equal(accounts[2], shops[1].Accounts[1]);
            Assert.Equal(accounts[1], shops[1].Accounts[2]);

            Assert.Equal(accounts[5], shops[2].Accounts[0]);
            Assert.Equal(accounts[4], shops[2].Accounts[1]);

            connection
                .Execute("rollback");
        }
    }
}
