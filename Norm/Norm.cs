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
        protected string commandText;
        protected string commentHeader;
        protected readonly DatabaseType dbType;
        protected object[] parameters = null;

        protected CommandType commandType = CommandType.Text;
        protected CommandBehavior behavior = CommandBehavior.Default;
        protected int? commandTimeout = null;
        protected CancellationToken? cancellationToken = null;
        protected bool prepared = false;
        protected DbTransaction transaction = null;

        protected bool allResultTypesAreUnknown = false;
        protected bool[] unknownResultTypeList = null;

        protected Action<DbCommand> dbCommandCallback = null;
        protected Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback = null;
        protected bool commandCommentHeaderEnabled = false;
        protected string comment = null;
        protected bool includeCommandAttributes = true;
        protected bool includeParameters = false;
        protected bool includeCallerInfo = false;
        protected bool includeTimestamp = false;
        protected string memberName = null;
        protected string sourceFilePath = null;
        protected int sourceLineNumber = 0;
        private string[] names = null;

        public DatabaseType DbType => dbType;

        public Norm(DbConnection connection)
        {
            Connection = connection;
            dbType = connection.GetType().Name switch
            {
                "SqlConnection" => DatabaseType.Sql,
                "NpgsqlConnection" => DatabaseType.Npgsql,
                "MySqlConnection" => DatabaseType.MySql,
                _ => DatabaseType.Other
            };
            if (NormOptions.Value.DbReaderCallback != null)
            {
                this.readerCallback = NormOptions.Value.DbReaderCallback;
            }
        }

        ///<summary>
        ///returns DbConnection for this instance
        ///</summary>
        public DbConnection Connection { get; }

        ///<summary>
        ///returns CancellationToken for this instance or null if CancellationToken is not set
        ///</summary>
        public CancellationToken? CancellationToken { get => cancellationToken; }

        ///<summary>
        ///returns ReaderCallback for this instance or null of ReaderCallback is not set
        ///</summary>
        public Func<(string Name, int Ordinal, DbDataReader Reader), object> ReaderCallback { get => readerCallback; }

        ///<summary>
        /// Set command type for the connection commands and return Norm instance.
        /// Default command type for new instance is Text.
        ///</summary>
        ///<param name="type">
        ///     One of the System.Data.CommandType values.
        ///     Values are Text, StoredProcedure or TableDirect.
        ///</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm As(CommandType type)
        {
            commandType = type;
            return this;
        }

        ///<summary>
        /// Set command type to StoredProcedure for the connection commands and return Norm instance.
        ///</summary>
        ///<returns>Norm instance.</returns>
        public virtual Norm AsProcedure()
        {
            return As(CommandType.StoredProcedure);
        }

        ///<summary>
        /// Set command type to Text for the connection commands and return Norm instance.
        ///</summary>
        ///<returns>Norm instance.</returns>
        public virtual Norm AsText()
        {
            return As(CommandType.Text);
        }

        ///<summary>
        /// Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        ///</summary>
        ///<param name="timeout">Wait time in seconds.</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm Timeout(int timeout)
        {
            commandTimeout = timeout;
            return this;
        }

        ///<summary>
        /// Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        ///</summary>
        ///<param name="timeout">Wait time in seconds.</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithTimeout(int timeout)
        {
            commandTimeout = timeout;
            return this;
        }

        ///<summary>
        /// Sets the transaction object for the current database command.
        ///</summary>
        ///<param name="transaction">Transaction object</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithTransaction(DbTransaction transaction)
        {
            this.transaction = transaction;
            return this;
        }

        /// <summary>
        /// Set CancellationToken used in asynchronously operations in chained Norm instance.
        /// </summary>
        /// <param name="token">CancellationToken used in asynchronously operations</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithCancellationToken(CancellationToken token)
        {
            this.cancellationToken = token;
            return this;
        }

        ///<summary>
        ///     Sets the next command in prepared mode by calling Prepare for the next command.
        ///</summary>
        ///<returns>Norm instance.</returns>
        public virtual Norm Prepared()
        {
            prepared = true;
            return this;
        }

        ///<summary>
        /// Adds parameters list to query
        ///</summary>
        ///<param name="parameters">Parameters list. The parameter can be a simple value (mapped by position), DbParameter instance, or object instances where is each property is mapped to a named database parameter.</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithParameters(params object[] parameters)
        {
            MergeParameters(parameters);
            return this;
        }

        ///<summary>
        /// Set the command callback for the next command.
        ///</summary>
        ///<param name="dbCommandCallback">DbCommand callback</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithCommandCallback(Action<DbCommand> dbCommandCallback)
        {
            this.dbCommandCallback = dbCommandCallback;
            return this;
        }

        ///<summary>
        /// Set the database reader callback for the next command.
        ///</summary>
        ///<param name="readerCallback">Reader tuple value object (Name, Ordinal and DbDataReader)</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            if (this.readerCallback != null)
            {
                throw new NormReaderAlreadyAssignedException();
            }
            this.readerCallback = readerCallback;
            return this;
        }

        /// <summary>
        /// Sets the custom comment header for the next command.
        /// </summary>
        /// <param name="comment"></param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithComment(string comment)
        {
            if (!this.commandCommentHeaderEnabled)
            {
                this.commandCommentHeaderEnabled = true;
                this.includeCommandAttributes = false;
                this.includeParameters = false;
                this.includeCallerInfo = false;
                this.includeTimestamp = false;
            }
            
            this.comment = comment;
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithCommentParameters()
        {
            if (!this.commandCommentHeaderEnabled)
            {
                this.commandCommentHeaderEnabled = true;
                this.comment = null;
                this.includeCommandAttributes = false;
                this.includeCallerInfo = false;
                this.includeTimestamp = false;
            }

            this.includeParameters = true;
            return this;
        }

        /// <summary>
        /// Sets the comment header to include caller info (source method name, source code file path and line number) for the next command.
        /// </summary>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithCommentCallerInfo()
        {
            if (!this.commandCommentHeaderEnabled)
            {
                this.commandCommentHeaderEnabled = true;
                this.comment = null;
                this.includeCommandAttributes = false;
                this.includeParameters = false;
                this.includeTimestamp = false;
            }

            this.includeCallerInfo = true;
            return this;
        }

        /// <summary>
        /// Sets the comment header options for the next command.
        /// </summary>
        /// <param name="comment">Custom comment</param>
        /// <param name="includeParameters">Include command parameters</param>
        /// <param name="includeCommandAttributes">Include command attributes (command type and timeout)</param>
        /// <param name="includeCallerInfo">Include command caller info (source method name, source code file path and line number)</param>
        /// <param name="includeTimestamp">Include execution timestamp</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithCommentHeader(
            string comment = null,
            bool includeCommandAttributes = true,
            bool includeParameters = true,
            bool includeCallerInfo = true,
            bool includeTimestamp = false)
        {
            this.commandCommentHeaderEnabled = true;
            this.comment = comment;
            this.includeCommandAttributes = includeCommandAttributes;
            this.includeParameters = includeParameters;
            this.includeCallerInfo = includeCallerInfo;
            this.includeTimestamp = includeTimestamp;
            return this;
        }

        ///<summary>
        /// Sets the database reader command behavior.
        ///</summary>
        ///<param name="behavior">CommandBehavior enum value</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithCommandBehavior(CommandBehavior behavior)
        {
            this.behavior = behavior;
            return this;
        }

        ///<summary>
        /// Sets PostgreSQL results behavior.
        /// Call WithUnknownResultType() to set all results as unknown or add true to result position to set type to unknown.
        /// Unkown result type is serialized as raw string and there is no type matching. Useful for fast json retreival.
        ///</summary>
        ///<param name="list">List booleans matching rsult poistion or empty for all</param>
        ///<returns>Norm instance.</returns>
        public virtual Norm WithUnknownResultType(params bool[] list)
        {
            if (dbType != DatabaseType.Npgsql)
            {
                throw new NotSupportedException("WithUnknownResultType is only available on PostgreSQL");
            }
            if (list == null || list.Length == 0)
            {
                allResultTypesAreUnknown = true;
                unknownResultTypeList = null;
            }
            else
            {
                allResultTypesAreUnknown = false;
                unknownResultTypeList = list;
            }
            return this;
        }

        /// <summary>
        /// Creates a DbCommand object
        /// </summary>
        /// <param name="command">SQL Command</param>
        /// <returns>DbCommand</returns>
        public DbCommand CreateCommand(string command)
        {
            var cmd = Connection.CreateCommand();
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }
            EnsureIsOpen(this.Connection);
            if (this.parameters != null)
            {
                NormParameterParser.AddParameters(this, cmd, this.parameters);
            }
            if (NormOptions.Value.Prepared || prepared)
            {
                cmd.Prepare();
            }
            if ((allResultTypesAreUnknown || unknownResultTypeList != null) && dbType == DatabaseType.Npgsql)
            {
                ApplyUnknownResultTypes(cmd);
            }
            ApplyOptions(cmd);
            return cmd;
        }

        /// <summary>
        /// Creates a DbCommand object
        /// </summary>
        /// <param name="command">SQL Command</param>
        /// <returns>DbCommand</returns>
        public async ValueTask<DbCommand> CreateCommandAsync(string command)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }
            await EnsureIsOpenAsync(this.Connection, cancellationToken);
            if (this.parameters != null)
            {
                NormParameterParser.AddParameters(this, cmd, this.parameters);
            }
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
            if ((allResultTypesAreUnknown || unknownResultTypeList != null) && dbType == DatabaseType.Npgsql)
            {
                ApplyUnknownResultTypes(cmd);
            }
            ApplyOptions(cmd);
            return cmd;
        }

        /// <summary>
        /// Creates a DbCommand object
        /// </summary>
        /// <param name="command">SQL Command</param>
        /// <returns>DbCommand</returns>
        public DbCommand CreateCommand(FormattableString command)
        {
            var (commandString, parameters) = NormParameterParser.ParseFormattableCommand(command);
            MergeParameters(parameters);
            return CreateCommand(commandString);
        }

        /// <summary>
        /// Creates a DbCommand object
        /// </summary>
        /// <param name="command">SQL Command</param>
        /// <returns>DbCommand</returns>
        public async ValueTask<DbCommand> CreateCommandAsync(FormattableString command)
        {
            var (commandString, parameters) = NormParameterParser.ParseFormattableCommand(command);
            MergeParameters(parameters);
            return await CreateCommandAsync(commandString);
        }

        /// <summary>
        /// Executes DbReader
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns>DbDataReader</returns>
        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            return cmd.ExecuteReader(this.behavior);
        }

        /// <summary>
        /// Executes DbReader
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns>DbDataReader</returns>
        public async ValueTask<DbDataReader> ExecuteReaderAsync(DbCommand cmd)
        {
            if (this.cancellationToken.HasValue)
            {
                return await cmd.ExecuteReaderAsync(this.behavior, this.cancellationToken.Value);
            }
            return await cmd.ExecuteReaderAsync(this.behavior);
        }
    }
}
