using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Norm
{
    public static partial class NormExtensions
    {
        private static readonly ConditionalWeakTable<DbConnection, Norm> Table = new ConditionalWeakTable<DbConnection, Norm>();

        private static Norm GetNoOrmInstance(this DbConnection connection)
        {
            if (Table.TryGetValue(connection, out var instance))
            {
                return instance;
            }
            if (NormOptions.NormCtor == null)
            {
                instance = new Norm(connection);
            }
            else
            {
                instance = (Norm)NormOptions.NormCtor.Invoke(new object[] { connection });
            }
            
            Table.AddOrUpdate(connection, instance);
            return instance;
        }
        ///<summary>
        /// Set command type for the connection commands and return new Norm instance.
        /// Default command type for new instance is Text.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="type">
        ///     One of the System.Data.CommandType values.
        ///     Values are Text, StoredProcedure or TableDirect.
        ///</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm As(this DbConnection connection, CommandType type)
        {
            return connection.GetNoOrmInstance().Clone().As(type);
        }

        ///<summary>
        /// Set command type to StoredProcedure for the connection commands and return new Norm instance.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm AsProcedure(this DbConnection connection)
        {
            return connection.As(CommandType.StoredProcedure);
        }

        ///<summary>
        /// Set command type to Text for the connection commands and return new Norm instance.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm AsText(this DbConnection connection)
        {
            return connection.As(CommandType.Text);
        }

        ///<summary>
        /// Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="timeout">Wait time in seconds.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm Timeout(this DbConnection connection, int timeout)
        {
            return connection.GetNoOrmInstance().Clone().Timeout(timeout);
        }

        ///<summary>
        /// Sets the default cancellation token for all asynchronous commands for the connection.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="cancellationToken">The token to monitor for cancellation requests.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCancellationToken(this DbConnection connection, CancellationToken cancellationToken)
        {
            return connection.GetNoOrmInstance().Clone().WithCancellationToken(cancellationToken);
        }

        ///<summary>
        /// Sets the next command in prepared mode by calling Prepare for the next command.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm Prepared(this DbConnection connection)
        {
            return connection.GetNoOrmInstance().Clone().Prepared();
        }

        ///<summary>
        /// Adds parameters list to query
        ///</summary>
        ///<param name="parameters">Parameters list. The parameter can be a simple value (mapped by position), DbParameter instance, or object instances where is each property is mapped to a named database parameter.</param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithParameters(this DbConnection connection, params object[] parameters)
        {
            return connection.GetNoOrmInstance().Clone().WithParameters(parameters);
        }

        ///<summary>
        /// Set the command callback for the next command.
        ///</summary>
        ///<param name="dbCommandCallback">DbCommand callback</param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCommandCallback(this DbConnection connection, Action<DbCommand> dbCommandCallback)
        {
            return connection.GetNoOrmInstance().Clone().WithCommandCallback(dbCommandCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithComment(this DbConnection connection, string comment)
        {
            return connection.GetNoOrmInstance().Clone().WithComment(comment);
        }

        /// <summary>
        /// 
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCommentParameters(this DbConnection connection)
        {
            return connection.GetNoOrmInstance().Clone().WithCommentParameters();
        }

        /// <summary>
        /// 
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCommentCallerInfo(this DbConnection connection)
        {
            return connection.GetNoOrmInstance().Clone().WithCommentCallerInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="includeParameters"></param>
        /// <param name="includeCommandAttributes"></param>
        /// <param name="includeCallerInfo"></param>
        /// <param name="includeTimestamp"></param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCommentHeader(this DbConnection connection,
            string comment = null,
            bool includeCommandAttributes = true,
            bool includeParameters = true,
            bool includeCallerInfo = true,
            bool includeTimestamp = false)
        {
            return connection.GetNoOrmInstance().Clone().WithCommentHeader(
                comment: comment,
                includeCommandAttributes: includeCommandAttributes,
                includeParameters: includeParameters,
                includeCallerInfo: includeCallerInfo,
                includeTimestamp: includeTimestamp);
        }
    }
}
