using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
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
        private readonly DatabaseType dbType;

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
            dbType = name switch
            {
                "SqlConnection" => DatabaseType.Ms,
                "NpgsqlConnection" => DatabaseType.Pg,
                "SQLiteConnection" => DatabaseType.Lt,
                "MySqlConnection" => DatabaseType.My,
                _ => DatabaseType.Other
            };
        }

        internal Norm Clone() => new Norm(Connection, commandType, commandTimeout, cancellationToken);

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

        private DbCommand CreateCommand(string command)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            return Prepare(cmd);
        }

        private async ValueTask<DbCommand> CreateCommandAsync(string command)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            return await PrepareAsync(cmd);
        }

        private DbCommand CreateCommand(string command, params object[] parameters)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            return cmd;
        }

        private async ValueTask<DbCommand> CreateCommandAsync(string command, params object[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            return cmd;
        }

        private DbCommand CreateCommand(string command, params (string name, object value)[] parameters)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            return cmd;
        }

        private async ValueTask<DbCommand> CreateCommandAsync(string command, params (string name, object value)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await AddParametersAsync(cmd, parameters);
            return cmd;
        }

        private DbCommand CreateCommand(string command, params (string name, object value, object type)[] parameters)
        {
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParametersUnknownType(cmd, parameters);
            return cmd;
        }

        private async ValueTask<DbCommand> CreateCommandAsync(string command, params (string name, object value, object type)[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            await AddParametersUnknownTypeAsync(cmd, parameters);
            return cmd;
        }

        private DbCommand CreateCommand(FormattableString command)
        {
            var args = command.GetArguments();
            var commandString = string.Format(command.Format, args.Select((p, idx) => 
            {
                if (p is DbParameter dbParameter)
                {
                    dbParameter.ParameterName = $"p{idx}";
                }
                return $"@p{idx}"; 
            }).ToArray());
            return CreateCommand(commandString, args);
        }

        private async ValueTask<DbCommand> CreateCommandAsync(FormattableString command)
        {
            var args = command.GetArguments();
            var commandString = string.Format(command.Format, args.Select((p, idx) =>
            {
                if (p is DbParameter dbParameter)
                {
                    dbParameter.ParameterName = $"p{idx}";
                }
                return $"@p{idx}";
            }).ToArray());
            return await CreateCommandAsync(commandString, args);
        }
    }
}
