using System;
using System.Collections.Generic;
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
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            if (commandTimeout.HasValue)
            {
                cmd.CommandTimeout = commandTimeout.Value;
            }
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
            AddParametersInternal(cmd, parameters);
            return Prepare(cmd);
        }

        private async ValueTask<DbCommand> AddParametersAsync(DbCommand cmd, params object[] parameters)
        {
            AddParametersInternal(cmd, parameters);
            return await PrepareAsync(cmd);
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

        private DbCommand CreateCommand(FormattableString command)
        {
            var (commandString, parameters) = ParseFormattableCommand(command);
            return CreateCommand(commandString, parameters);
        }

        private async ValueTask<DbCommand> CreateCommandAsync(FormattableString command)
        {
            var (commandString, parameters) = ParseFormattableCommand(command);
            return await CreateCommandAsync(commandString, parameters);
        }

        private (string commandString, object[] parameters) ParseFormattableCommand(FormattableString command)
        {
            var args = command.GetArguments();
            var parameters = new List<object>(args.Length);
            var commandString = string.Format(command.Format, args.Select((p, idx) =>
            {
                if (p is DbParameter dbParameter)
                {
                    dbParameter.ParameterName = $"p{idx}";
                    parameters.Add(p);
                    return $"@p{idx}";
                }
                if (p is string)
                {
                    if (command.Format.Contains($"{{{idx}:raw"))
                    {
                        return p;
                    }
                }
                parameters.Add(p);
                return $"@p{idx}";
            }).ToArray());

            return (commandString, parameters.ToArray());
        }
    }
}
