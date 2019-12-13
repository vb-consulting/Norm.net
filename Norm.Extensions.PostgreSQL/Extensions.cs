using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm.Extensions.PostgreSQL
{
    public static class Extensions
    {
        public static string GetSchema(this DbConnection connection) => connection.Single<string>("select current_schema()");

        public static long GetTransactionId(this DbConnection connection) => connection.Single<long>("select txid_current()");

        public static long GetBackendPid(this DbConnection connection) => connection.Single<long>("select pg_backend_pid()");

        public static DbConnection UseSchema(this DbConnection connection, string schema)
        {
            var current = connection.Execute($"set search_path to {schema}").GetSchema();
            if (current == null)
            {
                throw new DataException($"Could not switch schema to {schema}. It may not exists. Use create schema statement.");
            }
            return connection;
        }

        public static bool TableExists(this DbConnection connection, string table, string schema = null) =>
            connection.Single(@$"
                select 1 
                from 
                    information_schema.tables
                where 
                    table_name = @table
                {(schema != null ? "and table_schema = @schema" : "")}
                ",table, schema).Count != 0;

        public static DbConnection Begin(this DbConnection connection) => connection.Execute("begin");

        public static DbConnection End(this DbConnection connection) => connection.Execute("end");

        public static DbConnection Savepoint(this DbConnection connection, string name) => connection.Execute($"savepoint {name}");

        public static DbConnection ReleaseSavepoint(this DbConnection connection, string name) => connection.Execute($"release savepoint {name}");

        public static DbConnection Rollback(this DbConnection connection, string savepoint = null) => 
            connection.Execute(savepoint == null ? "rollback" : $"rollback to {savepoint}");
    }
}
