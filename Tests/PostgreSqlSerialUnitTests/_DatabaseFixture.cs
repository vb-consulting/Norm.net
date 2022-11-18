global using System.Threading.Tasks;
global using Xunit;
global using Npgsql;
global using Norm;
global using System;
global using System.Linq;
using System.Runtime.CompilerServices;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace PostgreSqlSerialUnitTests;

public static class _DatabaseFixture
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TestConfig.TestConfig config;
    public static string ConnectionString { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [ModuleInitializer]
    internal static void Initializer()
    {
        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
        Init();
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Dispose();
    }

    internal static void Init()
    {
        config = TestConfig.Config.Value;
        var builder = new NpgsqlConnectionStringBuilder(config.Default)
        {
            Database = config.TestDatabase
        };
        ConnectionString = builder.ToString();
        CreateTestDatabase();
    }

    internal static void Dispose()
    {
        DropTestDatabase();
    }

    private static void CreateTestDatabase()
    {
        void DoCreate() => Execute($"create database {config.TestDatabase}");
        try
        {
            DoCreate();
        }
        catch (PostgresException e)
        {
            switch (e.SqlState)
            {

                case "42P04":
                    DropTestDatabase();
                    DoCreate();
                    break;
                default:
                    throw;
            }
        }
    }

    private static void DropTestDatabase()
    {
        Execute($@"
            revoke connect on database {config.TestDatabase} from public;
            
            select pid, pg_terminate_backend(pid) from pg_stat_activity 
            where datname = '{config.TestDatabase}' and pid <> pg_backend_pid();

            drop database {config.TestDatabase};");
    }

    private static void Execute(string command)
    {
        using var conn = new NpgsqlConnection(config.Default);
        using var cmd = new NpgsqlCommand(command, conn);
        cmd.CommandTimeout = 60 * 5; // 5 mins
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
    }
}

