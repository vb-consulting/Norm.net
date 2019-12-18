using System;
using System.Data;
using System.Data.Common;


namespace Norm.Extensions.PostgreSQL
{
    public static class Extensions
    {
        public static string CurrentSchema(this DbConnection connection) => connection.Single<string>(Sql.CurrentSchema);

        public static long TransactionId(this DbConnection connection) => connection.Single<long>(Sql.TransactionId);

        public static long BackendPid(this DbConnection connection) => connection.Single<long>(Sql.BackendPid);

        public static string TrySearchPathSchema(this DbConnection connection, string schema) => connection.Execute(Sql.SearchPathSchema(schema)).CurrentSchema();

        public static DbConnection SearchPathSchema(this DbConnection connection, string schema)
        {
            var current = connection.Execute(Sql.SearchPathSchema(schema)).CurrentSchema();
            if (current == null)
            {
                throw new DataException($"Could not switch schema to {schema}. It may not exists. Use create schema statement.");
            }
            return connection;
        }

        public static void CreateSchema(this DbConnection connection, string schema, bool ifNotExists = false, string authorization = null) 
            => connection.Execute(Sql.CreateSchema(schema, ifNotExists, authorization));

        public static bool TableExists(this DbConnection connection, string table, string schema = null) =>
            connection.Single(Sql.TableExists(schema), table, schema).Count != 0;

        public static DbConnection Begin(this DbConnection connection) => connection.Execute(Sql.Begin);

        public static DbConnection End(this DbConnection connection) => connection.Execute(Sql.End);

        public static DbConnection Savepoint(this DbConnection connection, string name) => connection.Execute(Sql.Savepoint(name));

        public static DbConnection ReleaseSavepoint(this DbConnection connection, string name) => connection.Execute(Sql.ReleaseSavepoint(name));

        public static DbConnection Rollback(this DbConnection connection, string savepoint = null) => 
            connection.Execute(Sql.Rollback(savepoint));

        public static DbConnection Script(this DbConnection connection, (string name, string type, string defaultValue)[] parameters, string content) =>
            connection.Execute(Sql.Script(parameters, content));

        public static DbConnection Script(this DbConnection connection, (string name, string type)[] parameters, string content) =>
            connection.Execute(Sql.Script(parameters, content));

        public static DbConnection Script(this DbConnection connection, string content) =>
            connection.Execute(Sql.Script(content));
    }
}
