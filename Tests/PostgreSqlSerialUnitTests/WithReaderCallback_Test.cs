using System.Data.Common;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    //Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback

    private class JsonTest
    {
        public string I { get; set; }
        public JsonObject J { get; set; }
    }

    private object? ReaderCallback((string Name, int Ordinal, DbDataReader Reader) arg) => 
        arg.Reader.GetDataTypeName(arg.Ordinal) switch
    {
        "json" => JsonNode.Parse(arg.Reader.GetString(arg.Ordinal))?.AsObject(),
        _ => null
    };

    [Fact]
    public void WithReaderCallback_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        const string query = "select '{\"a\": 1}'::text as i, '{\"a\": 1}'::json as j";
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        var r1 = connection
            .WithReaderCallback(ReaderCallback)
            .Read(query)
            .Single();

        var r2 = connection
            .WithReaderCallback(ReaderCallback)
            .Read<string, JsonObject>(query)
            .Single();

        var r3 = connection
            .WithReaderCallback(ReaderCallback)
            .Read<JsonTest>(query)
            .Single();

        Assert.IsType<string>(r1[0].value);
        Assert.IsType<JsonObject>(r1[1].value);

        Assert.IsType<string>(r2.Item1);
        Assert.IsType<JsonObject>(r2.Item2);

        Assert.IsType<string>(r3.I);
        Assert.IsType<JsonObject>(r3.J);

        Assert.Throws<NormReaderAlreadyAssignedException>(() =>
        {
            connection
                .WithReaderCallback(ReaderCallback)
                .WithReaderCallback(ReaderCallback);
        });

        NormOptions.Configure(o =>
        {
            o.DbReaderCallback = ReaderCallback;
        });

        r1 = connection.Read(query).Single();
        r2 = connection.Read<string, JsonObject>(query).Single();
        r3 = connection.Read<JsonTest>(query).Single();

        Assert.IsType<string>(r1[0].value);
        Assert.IsType<JsonObject>(r1[1].value);

        Assert.IsType<string>(r2.Item1);
        Assert.IsType<JsonObject>(r2.Item2);

        Assert.IsType<string>(r3.I);
        Assert.IsType<JsonObject>(r3.J);

        Assert.Throws<NormReaderAlreadyAssignedException>(() =>
        {
            connection
                .WithReaderCallback(ReaderCallback);
        });
    }
}
