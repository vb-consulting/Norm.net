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

        public static string CreateSchema(string schema, bool ifNotExists = false, string authorization = null) => 
            $"create schema {(ifNotExists ? "if not exists " : "")}{schema}{(authorization != null ? $" authorization {authorization}" : "")}";

        public static string DropSchema(string schema) => $"drop schema {schema}";

        public static string DropSchemaCascade(string schema) => $"drop schema {schema} cascade";

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

        public static string Script(string content) =>
            @$"
            do $$
            begin
                {content}
            end
            $$";

        public static string Script((string name, string type)[] parameters, string content) =>
            @$"
            do $$
                {
                    string.Join(
                        string.Concat(";", Environment.NewLine),
                        parameters.Select(t => $"declare {t.name} {t.type};"))
                }
            begin
                {content}
            end
            $$";

        public static string Script((string name, string type, string defaultValue)[] parameters, string content) =>
            @$"
            do $$
                {
                    string.Join(
                        string.Concat(";", Environment.NewLine),
                        parameters.Select(t => $"declare {t.name} {t.type}{(t.defaultValue == null ? ";" : $" = {t.defaultValue};")}"))
                }
            begin
                {content}
            end
            $$";

        public static string Info(string info, params string[] parameters) =>
            $"raise info {(parameters.Length == 0 ? string.Concat("'", info, "'") : string.Concat("'", info, "', ", string.Join(", ", parameters)))}";
    }

    public static class PgTypes
    {
        public const string Int = "int";
        public const string Text = "text";
        public const string Json = "json";
        // etc
    }
}