namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_Parallel_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        string actual1 = "", expected1 = $"/*\ncomment1\n*/\nselect 1";
        string actual2 = "", expected2 = $"/*\ncomment2\n*/\nselect 2";
        string actual3 = "", expected3 = $"/*\ncomment3\n*/\nselect 3";
        string actual4 = "", expected4 = $"/*\ncomment4\n*/\nselect 4";

        Task.WaitAll(
            Task.Factory.StartNew(() =>
            {
                using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
                connection.WithComment("comment1").WithCommandCallback(c => actual1 = c.CommandText).Execute("select 1");
            }),
            Task.Factory.StartNew(() =>
            {
                using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
                connection.WithComment("comment2").WithCommandCallback(c => actual2 = c.CommandText).Execute("select 2");
            }),
            Task.Factory.StartNew(() =>
            {
                using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
                connection.WithComment("comment3").WithCommandCallback(c => actual3 = c.CommandText).Execute("select 3");
            }),
            Task.Factory.StartNew(() =>
            {
                using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
                connection.WithComment("comment4").WithCommandCallback(c => actual4 = c.CommandText).Execute("select 4");
            }));

        Assert.Equal(expected1, actual1);
        Assert.Equal(expected2, actual2);
        Assert.Equal(expected3, actual3);
        Assert.Equal(expected4, actual4);
    }
}