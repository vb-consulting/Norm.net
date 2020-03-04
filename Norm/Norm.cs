using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Norm.Extensions;
using Norm.Interfaces;

namespace Norm
{
    public partial class Norm : INorm
    {
        private enum DatabaseType
        {
            Ms,
            Pg,
            Lt,
            Other
        }

        private CommandType commandType;
        private int? commandTimeout;
        private JsonSerializerOptions jsonOptions;
        private CancellationToken? cancellationToken;
        private bool prepared;
        private readonly bool convertsDbNull;
        private readonly DatabaseType dbType;
        private static readonly Type StringType = typeof(string);

        public DbConnection Connection { get; }

        internal Norm(
            DbConnection connection, 
            CommandType commandType = CommandType.Text, 
            int? commandTimeout = null,
            JsonSerializerOptions jsonOptions = null,
            CancellationToken? cancellationToken = null,
            bool prepared = false)
        {
            Connection = connection;
            this.commandType = commandType;
            this.commandTimeout = commandTimeout;
            this.jsonOptions = jsonOptions;
            this.cancellationToken = cancellationToken;
            this.prepared = prepared;
            var name = connection.GetType().Name;
            (dbType, convertsDbNull) = name switch
            {
                "SqlConnection" => (DatabaseType.Ms, false),
                "NpgsqlConnection" => (DatabaseType.Pg, true),
                "SQLiteConnection" => (DatabaseType.Lt, false),
                _ => (DatabaseType.Other, false)
            };
        }

        internal Norm Clone() => new Norm(Connection, commandType, commandTimeout, jsonOptions, cancellationToken);

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

        public INorm WithCancellationToken(CancellationToken token)
        {
            this.cancellationToken = token;
            return this;
        }

        public INorm Prepared()
        {
            prepared = true;
            return this;
        }

        private JsonSerializerOptions JsonOptions => 
            this.jsonOptions ?? GlobalJsonSerializerOptions.Options;

        private void SetCommand(DbCommand cmd, string command)
        {
            cmd.SetCommandParameters(command, commandType, commandTimeout);
        }

        private DbCommand Prepare(DbCommand cmd)
        {
            if (!prepared)
            {
                return cmd;
            }

            cmd.Prepare();
            prepared = false;

            return cmd;
        }

        private async ValueTask<DbCommand> PrepareAsync(DbCommand cmd)
        {
            if (!prepared)
            {
                return cmd;
            }

            if (cancellationToken.HasValue)
            {
                await cmd.PrepareAsync(cancellationToken.Value);
            }
            else
            {
                await cmd.PrepareAsync();
            }
            prepared = false;

            return cmd;
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
