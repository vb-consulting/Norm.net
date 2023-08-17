using Norm;
using Npgsql;

using var connection = new NpgsqlConnection("Server=localhost;Database=dvdrental;Port=5432;User Id=postgres;Password=postgres;");

// Iterate public static methods in Examples class
foreach (var method in typeof(Examples).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
{
    Console.WriteLine($"Running example: {method.Name}");
    method.Invoke(null, new object[] { connection });
    Console.WriteLine();
}

public class Examples
{
    public static void CountActors(NpgsqlConnection connection)
    {
        var count = connection.Read<int>("select count(*) from actor").Single();
        Console.WriteLine($"There are {count} actors in the database.");
    }

    public static void DelayedExecution(NpgsqlConnection connection)
    {
        // create two iterators, no database calls yet

        // iterator over int type
        var result1 = connection.Read<int>("select count(*) from actor");
        // iterator name-value array
        var result2 = connection.Read("select title from film");

        // Execute by initiating iterations 

        // execute count in database and print single result from count(*)
        Console.WriteLine($"There are {result1.Single()} actors in the database.");
        // execute select in database, return all records and print iteration count
        Console.WriteLine($"There are {result2.Count()} films in the database.");
    }

    public static async Task DelayedExecutionAsync(NpgsqlConnection connection)
    {
        // create two iterators, no database calls yet

        // async iterator over int type
        var result1 = connection.ReadAsync<int>("select count(*) from actor");
        // async iterator name-value array
        var result2 = connection.ReadAsync("select title from film");

        // Execute by initiating iterations

        // execute count in database and print and await single async result from count(*)
        Console.WriteLine($"There are {await result1.SingleAsync()} actors in the database.");
        // execute select in database, return all records and print and await iteration count async
        Console.WriteLine($"There are {await result2.CountAsync()} films in the database.");
    }

    public static void PrintTuples(NpgsqlConnection connection)
    {
        // tuples mapping
        foreach (var tuple in connection.Read<string, string, int>("select title, description, release_year from film limit 3"))
        {
            Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.Item1, tuple.Item2, tuple.Item3);
        }
    }

    public static void PrintDeconstructedTuples(NpgsqlConnection connection)
    {
        // tuples deconstruction
        foreach (var (title, description, year) in 
            connection.Read<string, string, int>("select title, description, release_year from film limit 3"))
        {
            Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year);
        }
    }

    public static void PrintNamedTuples(NpgsqlConnection connection)
    {
        // named tuples
        foreach (var tuple in
            connection
                .Read<(string title, string description, int year)>("select title, description, release_year from film limit 3"))
        {
            Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.title, tuple.description, tuple.year);
        }
    }

    public static void ConfigureGlobalSettings(NpgsqlConnection connection)
    {
        // set global command timeout to 60 seconds
        // this call should be execute once from a program startup
        NormOptions.Configure(options =>
        {
            options.CommandTimeout = 60;
        });

        var count = connection.Read<int>("select count(*) from actor").Single();
        Console.WriteLine($"There are {count} actors in the database. I executed this with command timeout of 60 seconds.");
    }

    public static void NonGeneric(NpgsqlConnection connection)
    {
        // list of dictionaries where key is field name and value is value
        var list1 = connection
            .Read("select film_id, title, release_year from film limit 3")
            .Select(tuples =>
                tuples.ToDictionary(
                        tuple => tuple.name,
                        tuple => tuple.value))
            .ToList();

        // dictionary where key is film_id and value is file title
        var dict = connection
            .Read("select film_id, title from film limit 3")
            .ToDictionary(
                tuples => (int)tuples.First().value,
                tuples => tuples.Last().value?.ToString());
    }

    public static void NonGenericAny(NpgsqlConnection connection)
    {
        Console.WriteLine($"Film id=111 {(connection.Read("select 1 from film where film_id=111").Any() ? "exists" : "not exists")}");
    }
}