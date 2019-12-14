using System;
using System.Linq;

namespace Norm.Extensions.PostgreSQL
{
    public static class Sql
    {
        public static string CurrentSchema => "select current_schema()";
        public static string TransactionId => "select txid_current()";
        public static string BackendPid => "select pg_backend_pid()";
        public static string SearchPathSchema(string schema) => $"set search_path to {schema}";
        public static string Begin => "begin";
        public static string End => "end";
        public static string Savepoint(string name) => $"savepoint {name}";
        public static string ReleaseSavepoint(string name) => $"release savepoint {name}";
        public static string Rollback(string savepoint = null) => savepoint == null ? "rollback" : $"rollback to {savepoint}";

        public static string TableExists(string schema = null) => @$"
                select 1
                from
                    information_schema.tables
                where
                    table_name = @table
                {(schema != null ? "and table_schema = @schema" : "")}";


        public static string Script(string content, params (string, object)[] parameters) =>
            @$"
            do $$
            {string.Join(string.Concat(";", Environment.NewLine), parameters.Select(t => $"declare {t.Item1}{(t.Item2 == null ? ";" : $" = {t.Item2};")}"))}
            begin
                {content}
            end
            $$";

        public static string Info(string info, params string[] parameters) =>
            $"raise info {(parameters.Length == 0 ? string.Concat("'", info, "'") : string.Concat("'", info, "', ", string.Join(", ", parameters)))}";
    }
}