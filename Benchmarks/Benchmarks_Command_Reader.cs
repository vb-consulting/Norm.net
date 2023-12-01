using Npgsql;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void Command_Reader()
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }
        using var command = new NpgsqlCommand(query, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var i = new PocoClass
            {
                Id1 = reader.GetInt32(0),
                Foo1 = reader.GetString(1),
                Bar1 = reader.GetString(2),
                DateTime1 = reader.GetDateTime(3),
                Id2 = reader.GetInt32(4),
                Foo2 = reader.GetString(5),
                Bar2 = reader.GetString(6),
                DateTime2 = reader.GetDateTime(7),
                LongFooBar = reader.GetString(8),
                IsFooBar = reader.GetBoolean(9),
            };

            var c = i;
        }
    }
}
