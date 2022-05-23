using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        protected enum DatabaseType
        {
            Sql,
            Npgsql,
            MySql,
            Other
        }

        protected string commandText;
        protected string commentHeader;
        protected readonly DatabaseType dbType;

        protected CommandType commandType = CommandType.Text;
        protected int? commandTimeout = null;
        protected CancellationToken? cancellationToken = null;
        protected bool prepared = false;

        protected Action<DbCommand> dbCommandCallback = null;
        protected bool commandCommentHeaderEnabled = false;
        protected string comment = null;
        protected bool includeCommandAttributes = true;
        protected bool includeParameters = false;
        protected bool includeCallerInfo = false;
        protected bool includeTimestamp = false;
        protected string memberName = null;
        protected string sourceFilePath = null;
        protected int sourceLineNumber = 0;

        internal Norm(
            DbConnection connection,
            CommandType commandType,
            int? commandTimeout,
            CancellationToken? cancellationToken,
            bool prepared,

            Action<DbCommand> dbCommandCallback,
            bool commandCommentHeaderEnabled,
            string comment,
            bool includeCommandAttributes,
            bool includeParameters,
            bool includeCallerInfo,
            bool includeTimestamp,
            string memberName,
            string sourceFilePath,
            int sourceLineNumber

        ) : this(connection)
        {
            this.commandType = commandType;
            this.commandTimeout = commandTimeout;
            this.cancellationToken = cancellationToken;
            this.prepared = prepared;

            this.dbCommandCallback = dbCommandCallback;
            this.commandCommentHeaderEnabled = commandCommentHeaderEnabled;
            this.comment = comment;
            this.includeCommandAttributes = includeCommandAttributes;
            this.includeParameters = includeParameters;
            this.includeCallerInfo = includeCallerInfo;
            this.includeTimestamp = includeTimestamp;
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
        }

        internal Norm Clone()
        {
            return new Norm(
                connection: Connection,
                commandType: commandType,
                commandTimeout: commandTimeout,
                cancellationToken: cancellationToken,
                prepared: prepared,

                dbCommandCallback: dbCommandCallback,
                commandCommentHeaderEnabled: commandCommentHeaderEnabled,
                comment: comment,
                includeCommandAttributes: includeCommandAttributes,
                includeParameters: includeParameters,
                includeCallerInfo: includeCallerInfo,
                includeTimestamp: includeTimestamp,
                memberName: memberName,
                sourceFilePath: sourceFilePath,
                sourceLineNumber: sourceLineNumber
            );
        }

        protected virtual void ApplyOptions(DbCommand cmd)
        {
            if (NormOptions.Value.CommandTimeout.HasValue)
            {
                cmd.CommandTimeout = NormOptions.Value.CommandTimeout.Value;
            }
            if (commandTimeout.HasValue)
            {
                cmd.CommandTimeout = commandTimeout.Value;
            }
            if (NormOptions.Value.CommandCommentHeader.Enabled || this.commandCommentHeaderEnabled)
            {
                var sb = new StringBuilder();

                if (this.commandCommentHeaderEnabled && this.comment != null)
                {
                    sb.AppendLine($"-- {this.comment}");
                }

                if ((NormOptions.Value.CommandCommentHeader.Enabled && NormOptions.Value.CommandCommentHeader.IncludeCommandAttributes) || 
                    (this.commandCommentHeaderEnabled && this.includeCommandAttributes))
                {
                    sb.AppendLine($"-- {(this.dbType == DatabaseType.Other ? "" : $"{this.dbType} ")}{cmd.CommandType.ToString()} Command. Timeout: {cmd.CommandTimeout} seconds.");
                }

                if ((NormOptions.Value.CommandCommentHeader.Enabled && NormOptions.Value.CommandCommentHeader.IncludeTimestamp) || 
                    (this.commandCommentHeaderEnabled && this.includeTimestamp))
                {
                    sb.AppendLine($"-- Timestamp: {DateTime.Now:o}");
                }

                if ((NormOptions.Value.CommandCommentHeader.Enabled && NormOptions.Value.CommandCommentHeader.IncludeParameters) || 
                    (this.commandCommentHeaderEnabled && this.includeParameters))
                {
                    foreach (DbParameter p in cmd.Parameters)
                    {
                        string paramType;
                        if (this.dbType == DatabaseType.Other)
                        {
                            paramType = p.DbType.ToString().ToLowerInvariant();
                        }
                        else
                        {
                            var prop = p.GetType().GetProperty($"{this.dbType}DbType");
                            if (prop != null)
                            {
                                paramType = prop.GetValue(p).ToString().ToLowerInvariant();
                            }
                            else
                            {
                                paramType = this.dbType.ToString().ToLowerInvariant();
                            }
                        }
                        object value = p.Value is DateTime time ? time.ToString("o") : p.Value;
                        if (value is string)
                        {
                            value = $"\"{value}\"";
                        }
                        else if (value is bool)
                        {
                            value = value.ToString().ToLowerInvariant();
                        }
                        sb.Append(string.Format(NormOptions.Value.CommandCommentHeader.ParametersFormat, p.ParameterName, paramType, value));
                    }
                }

                if (this.commandCommentHeaderEnabled && this.includeCallerInfo)
                {
                    sb.AppendLine($"-- at {memberName} in {sourceFilePath} {sourceLineNumber}");
                }

                commandText = cmd.CommandText;
                commentHeader = sb.ToString();
                cmd.CommandText = string.Concat(commentHeader, commandText);
            }
            else
            {
                commandText = cmd.CommandText;
                commentHeader = null;
            }

            NormOptions.Value.DbCommandCallback?.Invoke(cmd);
            dbCommandCallback?.Invoke(cmd);
        }

        protected DbCommand CreateCommand(string command)
        {
            var cmd = Connection.CreateCommand();
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            Connection.EnsureIsOpen();
            if (NormOptions.Value.Prepared || prepared)
            {
                cmd.Prepare();
                prepared = false;
            }
            ApplyOptions(cmd);
            return cmd;
        }

        protected async ValueTask<DbCommand> CreateCommandAsync(string command)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            await Connection.EnsureIsOpenAsync(cancellationToken);
            if (NormOptions.Value.Prepared || prepared)
            {
                if (cancellationToken.HasValue)
                {
                    await cmd.PrepareAsync(cancellationToken.Value);
                }
                else
                {
                    await cmd.PrepareAsync();
                }
                prepared = false;
            }
            ApplyOptions(cmd);
            return cmd;
        }

        protected DbCommand CreateCommand(string command, params object[] parameters)
        {
            var cmd = Connection.CreateCommand();
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            Connection.EnsureIsOpen();
            AddParametersInternal(cmd, parameters);
            if (NormOptions.Value.Prepared || prepared)
            {
                cmd.Prepare();
                prepared = false;
            }
            ApplyOptions(cmd);
            return cmd;
        }

        protected async ValueTask<DbCommand> CreateCommandAsync(string command, params object[] parameters)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            await Connection.EnsureIsOpenAsync(cancellationToken);
            AddParametersInternal(cmd, parameters);
            if (NormOptions.Value.Prepared || prepared)
            {
                if (cancellationToken.HasValue)
                {
                    await cmd.PrepareAsync(cancellationToken.Value);
                }
                else
                {
                    await cmd.PrepareAsync();
                }
                prepared = false;
            }
            ApplyOptions(cmd);
            return cmd;
        }

        protected DbCommand CreateCommand(FormattableString command)
        {
            var (commandString, parameters) = ParseFormattableCommand(command);
            return CreateCommand(commandString, parameters);
        }

        protected async ValueTask<DbCommand> CreateCommandAsync(FormattableString command)
        {
            var (commandString, parameters) = ParseFormattableCommand(command);
            return await CreateCommandAsync(commandString, parameters);
        }
    }
}
