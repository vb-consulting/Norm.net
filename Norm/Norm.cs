﻿using System;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace Norm
{
    public partial class Norm
    {
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
        }

        ///<summary>
        ///     returns DbConnection for this instance
        ///</summary>
        public DbConnection Connection { get; }

        ///<summary>
        ///     Set command type for the connection commands and return Norm instance.
        ///     Default command type for new instance is Text.
        ///</summary>
        ///<param name="type">
        ///     One of the System.Data.CommandType values.
        ///     Values are Text, StoredProcedure or TableDirect.
        ///</param>
        ///<returns>Norm instance.</returns>
        public Norm As(CommandType type)
        {
            commandType = type;
            return this;
        }

        ///<summary>
        ///     Set command type to StoredProcedure for the connection commands and return Norm instance.
        ///</summary>
        ///<returns>Norm instance.</returns>
        public Norm AsProcedure()
        {
            return As(CommandType.StoredProcedure);
        }

        ///<summary>
        ///     Set command type to Text for the connection commands and return Norm instance.
        ///</summary>
        ///<returns>Norm instance.</returns>
        public Norm AsText()
        {
            return As(CommandType.Text);
        }

        ///<summary>
        ///     Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        ///</summary>
        ///<param name="timeout">Wait time in seconds.</param>
        ///<returns>Norm instance.</returns>
        public Norm Timeout(int timeout)
        {
            commandTimeout = timeout;
            return this;
        }

        /// <summary>
        /// Set CancellationToken used in asynchronously operations in chained Norm instance.
        /// </summary>
        /// <param name="token">CancellationToken used in asynchronously operations</param>
        ///<returns>Norm instance.</returns>
        public Norm WithCancellationToken(CancellationToken token)
        {
            this.cancellationToken = token;
            return this;
        }

        ///<summary>
        ///     Sets the next command in prepared mode by calling Prepare for the next command.
        ///</summary>
        ///<returns>Norm instance.</returns>
        public Norm Prepared()
        {
            prepared = true;
            return this;
        }

        ///<summary>
        /// Adds parameters list to query
        ///</summary>
        ///<param name="parameters">Parameters list. The parameter can be a simple value (mapped by position), DbParameter instance, or object instances where is each property is mapped to a named database parameter.</param>
        ///<returns>Norm instance.</returns>
        public Norm WithParameters(params object[] parameters)
        {
            this.parameters = parameters;
            return this;
        }

        ///<summary>
        /// Set the command callback for the next command.
        ///</summary>
        ///<param name="dbCommandCallback">DbCommand callback</param>
        ///<returns>Norm instance.</returns>
        public Norm WithCommandCallback(Action<DbCommand> dbCommandCallback)
        {
            this.dbCommandCallback = dbCommandCallback;
            return this;
        }

        ///<summary>
        /// Set the database reader callback for the next command.
        ///</summary>
        ///<param name="readerCallback">DbCommand callback</param>
        ///<returns>Norm instance.</returns>
        public Norm WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            this.readerCallback = readerCallback;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        ///<returns>Norm instance.</returns>
        public Norm WithComment(string comment)
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
        public Norm WithCommentParameters()
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
        /// 
        /// </summary>
        ///<returns>Norm instance.</returns>
        public Norm WithCommentCallerInfo()
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
        /// 
        /// </summary>
        /// <param name="comment">Append this custom user comment to header</param>
        /// <param name="includeCommandAttributes"></param>
        /// <param name="includeParameters"></param>
        /// <param name="includeCallerInfo"></param>
        /// <param name="includeTimestamp"></param>
        ///<returns>Norm instance.</returns>
        public Norm WithCommentHeader(
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
    }
}
