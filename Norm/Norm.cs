using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        private CancellationToken? cancellationToken;
        private bool prepared;
        private bool usingPostgresFormatParamsMode;
        private readonly bool convertsDbNull;
        private readonly DatabaseType dbType;
        private static readonly Type StringType = typeof(string);

        public DbConnection Connection { get; }

        internal Norm(
            DbConnection connection, 
            CommandType commandType = CommandType.Text, 
            int? commandTimeout = null,
            CancellationToken? cancellationToken = null,
            bool prepared = false,
            bool usingPostgresFormatParamsMode = false)
        {
            Connection = connection;
            this.commandType = commandType;
            this.commandTimeout = commandTimeout;
            this.cancellationToken = cancellationToken;
            this.prepared = prepared;
            this.usingPostgresFormatParamsMode = usingPostgresFormatParamsMode;
            var name = connection.GetType().Name;
            (dbType, convertsDbNull) = name switch
            {
                "SqlConnection" => (DatabaseType.Ms, false),
                "NpgsqlConnection" => (DatabaseType.Pg, true),
                "SQLiteConnection" => (DatabaseType.Lt, false),
                _ => (DatabaseType.Other, false)
            };
        }

        internal Norm Clone() => new Norm(Connection, commandType, commandTimeout, cancellationToken);

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

        public INorm WithCancellationToken(CancellationToken token)
        {
            this.cancellationToken = token;
            return this;
        }

        public INorm Prepared()
        {
            prepared = true;
            if (prepared && usingPostgresFormatParamsMode)
            {
                throw new ArgumentException("Cannot set UsingPostgresFormatParamsMode on prepared statements.");
            }
            return this;
        }

        public INorm UsingPostgresFormatParamsMode()
        {
            this.usingPostgresFormatParamsMode = true;
            if (dbType != DatabaseType.Pg)
            {
                throw new ArgumentException("Cannot set UsingPostgresFormatParamsMode on connection other than PostgreSQL.");
            }
            if (prepared && usingPostgresFormatParamsMode)
            {
                throw new ArgumentException("Cannot set UsingPostgresFormatParamsMode on prepared statements.");
            }
            return this;
        }

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

        private DbCommand AddParameters(DbCommand cmd, params object[] parameters)
        {
            if (usingPostgresFormatParamsMode)
            {
                usingPostgresFormatParamsMode = false;
                return Prepare(cmd.AddPgFormatParameters(parameters));
            }

            return Prepare(cmd.AddParameters(parameters));
        }

        private DbCommand AddParameters(DbCommand cmd, params (string name, object value)[] parameters)
        {
            if (usingPostgresFormatParamsMode)
            {
                usingPostgresFormatParamsMode = false;
                return Prepare(cmd.AddPgFormatParameters(parameters));
            }

            return Prepare(cmd.AddParameters(parameters));
        }

        private DbCommand AddParameters(DbCommand cmd, params (string name, object value, DbType type)[] parameters)
        {
            if (usingPostgresFormatParamsMode)
            {
                usingPostgresFormatParamsMode = false;
                return Prepare(cmd.AddPgFormatParameters(parameters));
            }

            return Prepare(cmd.AddParameters(parameters));
        }

        private async ValueTask<DbCommand> AddParametersAsync(DbCommand cmd, params object[] parameters)
        {
            if (usingPostgresFormatParamsMode)
            {
                usingPostgresFormatParamsMode = false;
                return await PrepareAsync(cmd.AddPgFormatParameters(parameters));
            }

            return await PrepareAsync(cmd.AddParameters(parameters));
        }

        private async ValueTask<DbCommand> AddParametersAsync(DbCommand cmd, params (string name, object value)[] parameters)
        {
            if (usingPostgresFormatParamsMode)
            {
                usingPostgresFormatParamsMode = false;
                return await PrepareAsync(cmd.AddPgFormatParameters(parameters));
            }

            return await PrepareAsync(cmd.AddParameters(parameters));
        }

        private async ValueTask<DbCommand> AddParametersAsync(DbCommand cmd, params (string name, object value, DbType type)[] parameters)
        {
            if (usingPostgresFormatParamsMode)
            {
                usingPostgresFormatParamsMode = false;
                return await PrepareAsync(cmd.AddPgFormatParameters(parameters));
            }

            return await PrepareAsync(cmd.AddParameters(parameters));
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
