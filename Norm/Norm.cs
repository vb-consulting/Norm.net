using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
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
            My,
            Other
        }

        private CommandType commandType;
        private int? commandTimeout;
        private CancellationToken? cancellationToken;
        private bool prepared;
        private bool usingPostgresFormatParamsMode;
        private readonly bool convertsDbNull;
        private readonly DatabaseType dbType;

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
                "MySqlConnection" => (DatabaseType.My, false),
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

        public INorm Timeout(int timeout)
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
                throw new NormCannotUsePostgresFormatParamsModeOnPreparedStatementException();
            }
            return this;
        }

        public INorm UsingPostgresFormatParamsMode()
        {
            this.usingPostgresFormatParamsMode = true;
            if (dbType != DatabaseType.Pg)
            {
                throw new NormCannotUsePostgresFormatParamsModeWhenNotPostgreSqlException();
            }
            if (prepared && usingPostgresFormatParamsMode)
            {
                throw new NormCannotUsePostgresFormatParamsModeOnPreparedStatementException();
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

        private DbCommand AddParametersUnknownType(DbCommand cmd, params (string name, object value, object type)[] parameters)
        {
            if (usingPostgresFormatParamsMode)
            {
                usingPostgresFormatParamsMode = false;
                return Prepare(cmd.AddPgFormatParameters(parameters));
            }

            return Prepare(cmd.AddUnknownTypeParameters(parameters));
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

        private async ValueTask<DbCommand> AddParametersUnknownTypeAsync(DbCommand cmd, params (string name, object value, object type)[] parameters)
        {
            if (usingPostgresFormatParamsMode)
            {
                usingPostgresFormatParamsMode = false;
                return await PrepareAsync(cmd.AddPgFormatParameters(parameters));
            }

            return await PrepareAsync(cmd.AddUnknownTypeParameters(parameters));
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

        public DbCommand CreateCommand(string command)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            return Prepare(cmd);
        }

        public async ValueTask<DbCommand> CreateCommandAsync(string command)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            return await PrepareAsync(cmd);
        }

        public DbCommand CreateCommand(string command, params object[] parameters)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            return cmd;
        }

        public async ValueTask<DbCommand> CreateCommandAsync(string command, params object[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            return cmd;
        }

        public DbCommand CreateCommand(string command, params (string name, object value)[] parameters)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            return cmd;
        }

        public async ValueTask<DbCommand> CreateCommandAsync(string command, params (string name, object value)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            return cmd;
        }

        public DbCommand CreateCommand(string command, params (string name, object value, DbType type)[] parameters)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            return cmd;
        }

        public async ValueTask<DbCommand> CreateCommandAsync(string command, params (string name, object value, DbType type)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            await AddParametersAsync(cmd, parameters);
            return cmd;
        }

        public DbCommand CreateCommand(string command, params (string name, object value, object type)[] parameters)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParametersUnknownType(cmd, parameters);
            return cmd;
        }

        public async ValueTask<DbCommand> CreateCommandAsync(string command, params (string name, object value, object type)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            await AddParametersUnknownTypeAsync(cmd, parameters);
            return cmd;
        }
    }
}
