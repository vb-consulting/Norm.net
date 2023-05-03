global using System.Threading.Tasks;
global using Xunit;
global using Norm;
global using System;
global using System.Linq;
global using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace SqlSerialUnitTests;

public static class _DatabaseFixture
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TestConfig.TestConfig config;
    public static string ConnectionString { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [ModuleInitializer]
    internal static void Initializer()
    {
        Init();
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Dispose();
    }

    internal static void Init()
    {
        config = TestConfig.Config.Value;
        var builder = new SqlConnectionStringBuilder(config.Default)
        {
            InitialCatalog = config.TestDatabase
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
        catch (SqlException e)
        {
            switch (e.Number)
            {

                case 1801:
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
        Execute($"alter database {config.TestDatabase} set single_user with rollback immediate;");
        Execute($"drop database {config.TestDatabase};");
    }

    private static void Execute(string command)
    {
        using var conn = new SqlConnection(config.Default);
        using var cmd = new SqlCommand(command, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
    }
}

