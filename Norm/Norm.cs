using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using System.Threading.Tasks;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm : IDisposable, INorm
    {
        private enum DbType
        {
            Ms, Pg, Lt, Other
        }

        private bool disposed = false;
        private CommandType commandType;
        private int? commandTimeout;
        private JsonSerializerOptions jsonOptions;
        private readonly bool convertsDbNull;
        private readonly DbType dbType;
        private static readonly Type StringType = typeof(string);

        public DbConnection Connection { get; }

        public Norm(DbConnection connection, CommandType commandType = CommandType.Text, int? commandTimeout = null,
            JsonSerializerOptions jsonOptions = null)
        {
            Connection = connection;
            this.commandType = commandType;
            this.commandTimeout = commandTimeout;
            this.jsonOptions = jsonOptions;
            var name = connection.GetType().Name;
            (dbType, convertsDbNull) = name switch
            {
                "SqlConnection" => (DbType.Ms, false),
                "NpgsqlConnection" => (DbType.Pg, true),
                "SQLiteConnection" => (DbType.Lt, false),
                _ => (DbType.Other, false)
            };
        }

        public INorm As(CommandType type)
        {
            commandType = type;
            return this;
        }

        public INorm AsProcedure() => As(CommandType.StoredProcedure);

        public INorm AsText() => As(CommandType.Text);

        public INorm Timeout(int? timeout)
        {
            commandTimeout = timeout;
            return this;
        }

        public INorm WithJsonOptions(JsonSerializerOptions options)
        {
            this.jsonOptions = options;
            return this;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                Connection?.Dispose();
            }

            disposed = true;
        }

        private JsonSerializerOptions JsonOptions => 
            this.jsonOptions ?? GlobalJsonSerializerOptions.Options;

        private void SetCommand(DbCommand cmd, string command)
        {
            cmd.SetCommandParameters(command, commandType, commandTimeout);
        }

        private bool CheckDbNull<T>() => (!convertsDbNull || typeof(T) == StringType);

        private T GetFieldValue<T>(DbDataReader reader, int ordinal)
        {
            if (CheckDbNull<T>())
            {
                return reader.IsDBNull(ordinal) ? default : reader.GetFieldValue<T>(ordinal);
            }
            return reader.GetFieldValue<T>(ordinal);
        }

        private async ValueTask<T> GetFieldValueAsync<T>(DbDataReader reader, int ordinal)
        {
            if (CheckDbNull<T>())
            {
                return await reader.IsDBNullAsync(ordinal) ? default : await reader.GetFieldValueAsync<T>(ordinal);
            }
            return await reader.GetFieldValueAsync<T>(ordinal);
        }
    }
}
