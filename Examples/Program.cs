using System.Data.Common;
using Norm;
using Npgsql;
using static System.Console;

using TitleDescriptionYear = (string title, string description, int year);
using IdName = (int id, string name);

//
// Sample database: https://www.postgresqltutorial.com/postgresql-getting-started/postgresql-sample-database/
//

using var connection = new NpgsqlConnection("Server=localhost;Database=dvdrental;Port=5432;User Id=postgres;Password=postgres;");

// Iterate public static methods in Examples class
foreach (var method in typeof(Examples).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
{
    ForegroundColor = ConsoleColor.Cyan;
    WriteLine($"{method.Name}");
    ResetColor();
    method.Invoke(null, new object[] { connection });
    WriteLine();
}

public class ExtraFilm : Film
{
    public string Extra { get; set; } = "not-mapped";
}

public class Film
{
    public int FilmId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public decimal RentalRate { get; set; }
}

public class NonPublicFilm
{
    public int FilmId { get; private set; } // not mapped
    public string Title { get; protected set; } // not mapped
    public int ReleaseYear { get; set; } // mapped
    public decimal RentalRate { get; set; } // mapped
}

public class FilmDto
{
    public int FilmId { get; set; }
    public string Title { get; set; }
}

public class ActorDto
{
    public int ActorId { get; set; }
    public string Name { get; set; }
}

public static class Examples
{
    public static void CountActors(DbConnection connection)
    {
        var count = connection.Read<int>("select count(*) from actor").Single();
        WriteLine($"There are {count} actors in the database.");
    }

    public static void PrintTuples(DbConnection connection)
    {
        // tuples mapping
        foreach (var tuple in connection.Read<string, string, int>("select title, description, release_year from film limit 3"))
        {
            Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.Item1, tuple.Item2, tuple.Item3);
        }
    }

    public static void PrintDeconstructedTuples(DbConnection connection)
    {
        // tuples deconstruction
        foreach (var (title, description, year) in 
            connection.Read<string, string, int>("select title, description, release_year from film limit 3"))
        {
            WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year);
        }
    }

    public static void PrintNamedTuples(DbConnection connection)
    {
        // named tuples
        foreach (var tuple in
            connection
                .Read<(string title, string description, int year)>("select title, description, release_year from film limit 3"))
        {
            WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.title, tuple.description, tuple.year);
        }
    }

    public static void TitleDescriptionYearTupleAlias(DbConnection connection)
    {
        foreach (var tuple in connection.Read<TitleDescriptionYear>("select title, description, release_year from film limit 3"))
        {
            WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.title, tuple.description, tuple.year);
        }
    }

    public static void IdNameTupleAlias(DbConnection connection)
    {
        foreach (var tuple in connection.Read<IdName>("select film_id, title from film limit 3"))
        {
            WriteLine("Film Id: {0}, Name: {1}", tuple.id, tuple.name);
        }
    }

    public static void ConfigureGlobalSettings(DbConnection connection)
    {
        // set global command timeout to 60 seconds
        // this call should be execute once from a program startup
        NormOptions.Configure(options =>
        {
            options.CommandTimeout = 60;
        });

        var count = connection.Read<int>("select count(*) from actor").Single();
        WriteLine($"There are {count} actors in the database. I executed this with command timeout of 60 seconds.");
    }

    public static void NonGeneric(DbConnection connection)
    {
        // dictionary where key is film_id and value is file title
        var dict = connection
            .Read("select film_id, title from film limit 3")
            .ToDictionary(
                tuples => (int)tuples.First().value,
                tuples => tuples.Last().value?.ToString());

        WriteLine("Dictionary first key-value {0}-{1} ", dict.Keys.First(), dict.Values.First());
    }

    public static void NonGenericAny(DbConnection connection)
    {
        WriteLine($"Film id=111 {(connection.Read("select 1 from film where film_id=111").Any() ? "exists" : "not exists")}");
    }

    public static void TuplesDictionary(DbConnection connection)
    {
        // dictionary where key is film_id and value is file title
        var dict = connection
            .Read<int, string>("select film_id, title from film limit 3")
            .ToDictionary(
                tuple => tuple.Item1,
                tuple => tuple.Item2);

        WriteLine("Dictionary first key-value {0}-{1} ", dict.Keys.First(), dict.Values.First());
    }

    public static void DoesFilmExists(DbConnection connection)
    {
        WriteLine("Film id {0} exists: {1} ", 999, connection.Read("select 1 from film where film_id = @id", 999).Any());
    }

    public static void UnamedNamedTuplesDictionary(DbConnection connection)
    {
        // dictionary where key is film_id and value is file title
        var dict = connection
            .Read<(int, string)>("select film_id, title from film limit 3")
            .ToDictionary(
                tuple => tuple.Item1,
                tuple => tuple.Item2);

        WriteLine("Dictionary first key-value {0}-{1} ", dict.Keys.First(), dict.Values.First());
    }

    public static void NamedTuplesDictionary(DbConnection connection)
    {
        // dictionary where key is film_id and value is file title
        var dict = connection
            .Read<(int id, string name)>("select film_id, title from film limit 3")
            .ToDictionary(
                tuple => tuple.id,
                tuple => tuple.name);

        WriteLine("Dictionary first key-value {0}-{1} ", dict.Keys.First(), dict.Values.First());
    }

    public static void PrintMultipleNamedTuples(DbConnection connection)
    {
        // deconstruction of named tuples
        foreach (var (actor, film) in connection.Read<
            (int id, string name),
            (int id, string name)>(@"
            select 
                actor_id, first_name || ' ' || last_name, 
                film_id, title
            from 
                actor
                join film_actor using (actor_id)
                join film using (film_id)
            limit 3"))
        {
            WriteLine("Actor: {0}-{1}, Film: {2}-{3}", actor.id, actor.name, film.id, film.name);
        }
    }

    public static void PrintMultipleClassInstances(DbConnection connection)
    {
        // deconstruction of class instances
        foreach (var (actor, film) in connection.Read<ActorDto, FilmDto>(@"
            select 
                actor_id, 
                first_name || ' ' || last_name as name, 
                film_id, 
                title
            from 
                actor
                join film_actor using (actor_id)
                join film using (film_id)
            limit 3"))
        {
            WriteLine("Actor: {0}-{1}, Film: {2}-{3}", actor.ActorId, actor.Name, film.FilmId, film.Title);
        }
    }


    public static void MixingNamedTupleAndClassInstances(DbConnection connection)
    {
        try
        {
            foreach (var (actor, film) in connection.Read<(int id, string name), FilmDto>(@"
            select 
                actor_id, 
                first_name || ' ' || last_name as name, 
                film_id, 
                title
            from 
                actor
                join film_actor using (actor_id)
                join film using (film_id)
            limit 3"))
            {
                //...
            }
        }
        catch (System.InvalidCastException e)
        {
            WriteLine("Mixing named tuples and class instances is not allowed: InvalidCastException: {0}", e.Message);
        }
    }

    public static void MixingSimpleValuesAndClassInstances(DbConnection connection)
    {
        try
        {
            foreach (var (actorId, name, film) in connection.Read<int, string, FilmDto>(@"
            select 
                actor_id, 
                first_name || ' ' || last_name as name, 
                film_id, 
                title
            from 
                actor
                join film_actor using (actor_id)
                join film using (film_id)
            limit 3"))
            {
                //...
            }
        }
        catch (System.InvalidCastException e)
        {
            WriteLine("Mixing simple values and class instances is not allowed: InvalidCastException: {0}", e.Message);
        }
    }

    public static void MixingSimpleValuesAndNamedTuples(DbConnection connection)
    {
        try
        {
            foreach (var (actorId, name, film) in connection.Read<int, string, (int id, string name)>(@"
            select 
                actor_id, 
                first_name || ' ' || last_name as name, 
                film_id, 
                title
            from 
                actor
                join film_actor using (actor_id)
                join film using (film_id)
            limit 3"))
            {
                //...
            }
        }
        catch (System.InvalidCastException e)
        {
            WriteLine("Mixing simple values and class instances is not allowed: InvalidCastException: {0}", e.Message);
        }
    }

    public static void PrintFirstFilmFromClass(DbConnection connection)
    {
        var film = connection
            .Read<Film>(@"
                select film_id, title, release_year, rental_rate 
                from film
                limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
            film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
    }

    public static void PrintFirstFilmFromAnonymousClass(DbConnection connection)
    {
        var film = connection
            .Read(new
            {
                filmId = default(int),
                title = default(string),
                releaseYear = default(int),
                rentalRate = default(decimal)
            }, @"
                select film_id, title, release_year, rental_rate 
                from film
                limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
            film.filmId, film.title, film.releaseYear, film.rentalRate);
    }

    public static void PrintFirstFilmFromAnonymousClass2(DbConnection connection)
    {
        var film = connection
            .Read(new
            {
                filmId = 1,
                title = "",
                releaseYear = 1,
                rentalRate = 1m
            }, @"
                select film_id, title, release_year, rental_rate 
                from film
                limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
            film.filmId, film.title, film.releaseYear, film.rentalRate);
    }

    public static void PrintFirstFilmFromBlueprintInstance(DbConnection connection)
    {
        var instance = new Film();
        
        var film = connection
            .Read(instance, @"
                select film_id, title, release_year, rental_rate 
                from film
                limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
            film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
    }

    public static void PrintFirstFilmReverseOrderFromClass(DbConnection connection)
    {
        var film = connection
            .Read<Film>(@"
                select rental_rate, release_year, title, film_id
                from film
                limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
            film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
    }

    public static void PrintFirstFilmMapAllFromClass(DbConnection connection)
    {
        var film = connection
            .Read<Film>("select * from film limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
            film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
    }

    public static void PrintFilmsFromMappedClass(DbConnection connection)
    {
        foreach (var film in connection.Read<Film>(@"
            select film_id, title, release_year, rental_rate 
            from film
            limit 3"))
        {
            WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}", 
                film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
        }
    }

    public static void PrintFirstExtraFilmFromClass(DbConnection connection)
    {
        var film = connection
            .Read<ExtraFilm>("select * from film limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Extra: {2}", film.FilmId, film.Title, film.Extra);
    }

    public static void FilmFromClassSnakeCaseMappings(DbConnection connection)
    {
        // keep original names
        NormOptions.Configure(options =>
        {
            options.KeepOriginalNames = true;
        });
        
        var film = connection
            .Read<Film>("select * from film limit 1")
            .Single();

        WriteLine("Film id: {0}", film.FilmId); // film id defaults to 0

        // use snake case
        NormOptions.Configure(options =>
        {
            options.KeepOriginalNames = false;
        });

        film = connection
            .Read<Film>("select * from film limit 1")
            .Single();

        WriteLine("Film id: {0}", film.FilmId); // film id is mapped

        // fall-back to default
        NormOptions.Configure(options =>
        {
        });

        film = connection
            .Read<Film>("select * from film limit 1")
            .Single();

        WriteLine("Film id: {0}", film.FilmId); // film id is mapped
    }

    public enum MyEnum { Value1, Value2, Value3 }
    
    public static void ArraysAndEnumSimpleValue(DbConnection connection)
    {
        var t = connection
            .Read<int[], int?[], MyEnum, MyEnum, MyEnum?, MyEnum?> (@"
            select
                array[1,2,3] as array_not_null,
                array[1,null,3] as array_null,
                'Value1' as text_enum_not_null,
                0 as int_enum_not_null,
                null::text as text_enum_null,
                null::int as int_enum_null")
            .Single();

        WriteLine("{0} {1} {2} {3} {4} {5}",
            string.Join(", ", t.Item1), string.Join(", ", t.Item2), t.Item3, t.Item4, t.Item5, t.Item6);
    }

    class MyComplexType
    {
        public int[] ArrayNotNull { get; set; }
        public MyEnum TextEnumNotNull { get; set; }
        public MyEnum IntEnumNotNull { get; set; }
        public MyEnum? TextEnumNull { get; set; }
        public MyEnum? IntEnumNull { get; set; }
        public MyEnum[] TextEnumArray { get; set; }
        public MyEnum[] IntEnumArray { get; set; }
    }

    public static void NameTupleArray(DbConnection connection)
    {
        var tuple = connection
            .Read<(int[] intArray, string[] strArray)>("select array[1,2,3], array['a','b','c']")
            .Single();

        WriteLine("{0} {1}",
            string.Join(", ", tuple.intArray),
            string.Join(", ", tuple.strArray));
    }

    public static void ArraysAndEnumComplexType(DbConnection connection)
    {
        var instance = connection
            .Read<MyComplexType>(@"
            select 
                array[1,2,3] as array_not_null,
                'Value1' as text_enum_not_null,
                0 as int_enum_not_null,
                null::text as text_enum_null,
                null::int as int_enum_null,
                array['Value1', 'Value2', 'Value2'] as text_enum_array,
                array[0,1,2] as int_enum_array")
            .Single();

        WriteLine("{0} {1} {2} {3} {4} {5} {6}",
            string.Join(", ", instance.ArrayNotNull), 
            instance.IntEnumNotNull, 
            instance.TextEnumNull, 
            instance.TextEnumNull, 
            instance.IntEnumNull,
            string.Join(", ", instance.TextEnumArray),
            string.Join(", ", instance.IntEnumArray));
    }

    public static void PrintNonPublicFilmFromClass(DbConnection connection)
    {
        var film = connection
            .Read<NonPublicFilm>("select * from film limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
            film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);

        // map private and protected members too
        NormOptions.Configure(options =>
        {
            options.MapPrivateSetters = true;
        });

        film = connection
            .Read<NonPublicFilm>("select * from film limit 1")
            .Single();

        WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
            film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);

        // fall-back to default
        NormOptions.Configure(options =>
        {
        });
    }
    
    public static void DelayedExecution(DbConnection connection)
    {
        // create two iterators, no database calls yet

        // iterator over int type
        var result1 = connection.Read<int>("select count(*) from actor");
        // iterator name-value array
        var result2 = connection.Read("select title from film");

        // Execute by initiating iterations 

        // execute count in database and print single result from count(*)
        WriteLine($"There are {result1.Single()} actors in the database.");
        // execute select in database, return all records and print iteration count
        WriteLine($"There are {result2.Count()} films in the database.");
    }
    
    public static async Task DelayedExecutionAsync(DbConnection connection)
    {
        // create two iterators, no database calls yet

        // async iterator over int type
        var result1 = connection.ReadAsync<int>("select count(*) from actor");
        // async iterator name-value array
        var result2 = connection.ReadAsync("select title from film");

        // Execute by initiating iterations

        // execute count in database and print and await single async result from count(*)
        WriteLine($"There are {await result1.SingleAsync()} actors in the database.");
        // execute select in database, return all records and print and await iteration count async
        WriteLine($"There are {await result2.CountAsync()} films in the database.");
    }
}
