using Norm;
using Npgsql;
using System.Linq;

using var connection = new NpgsqlConnection("Server=localhost;Database=dvdrental;Port=5432;User Id=postgres;Password=postgres;");

var result = CountActors();
Console.WriteLine($"There are {result} actors in the database.");

int CountActors()
{
    return connection.Read<int>("select count(*) from actor").Single();
}

void DelayedExecution()
{
    var result1 = connection.Read<int>("select count(*) from actor");
    var result2 = connection.Read("select title from film");

    Console.WriteLine($"There are {result1} actors in the database.");
    Console.WriteLine($"There are {result2.Count} actors in the database.");
}
