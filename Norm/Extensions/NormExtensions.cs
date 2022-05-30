using System;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace Norm
{
    public static partial class NormExtensions
    {
        /// <summary>
        /// Creates new Norm instance
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static Norm Norm(this DbConnection connection)
        {
            if (NormOptions.NormCtor == null)
            {
                return new Norm(connection);
            }
            return (Norm)NormOptions.NormCtor.Invoke(new object[] { connection });
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
            return connection.Norm().As(type);
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
            return connection.Norm().Timeout(timeout);
        }

        ///<summary>
        /// Sets the default cancellation token for all asynchronous commands for the connection.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="cancellationToken">The token to monitor for cancellation requests.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCancellationToken(this DbConnection connection, CancellationToken cancellationToken)
        {
            return connection.Norm().WithCancellationToken(cancellationToken);
        }

        ///<summary>
        /// Sets the next command in prepared mode by calling Prepare for the next command.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm Prepared(this DbConnection connection)
        {
            return connection.Norm().Prepared();
        }

        ///<summary>
        /// Sets the parameters for the next command.
        ///</summary>
        ///<param name="parameters">Parameters list. The parameter can be a simple value (mapped by position), DbParameter instance, or object instances where is each property is mapped to a named database parameter.</param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithParameters(this DbConnection connection, params object[] parameters)
        {
            return connection.Norm().WithParameters(parameters);
        }

        ///<summary>
        /// Set the command callback for the next command.
        ///</summary>
        ///<param name="dbCommandCallback">DbCommand callback</param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCommandCallback(this DbConnection connection, Action<DbCommand> dbCommandCallback)
        {
            return connection.Norm().WithCommandCallback(dbCommandCallback);
        }

        ///<summary>
        /// Set the database reader callback for the next command.
        ///</summary>
        ///<param name="readerCallback">Reader tuple value object (Name, Ordinal and DbDataReader)</param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithReaderCallback(this DbConnection connection, Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return connection.Norm().WithReaderCallback(readerCallback);
        }

        /// <summary>
        /// Sets the custom comment header for the next command.
        /// </summary>
        /// <param name="comment"></param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithComment(this DbConnection connection, string comment)
        {
            return connection.Norm().WithComment(comment);
        }

        /// <summary>
        /// Sets the comment header to include command parameters for the next command.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCommentParameters(this DbConnection connection)
        {
            return connection.Norm().WithCommentParameters();
        }

        /// <summary>
        /// Sets the comment header to include caller info (source method name, source code file path and line number) for the next command.
        /// </summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCommentCallerInfo(this DbConnection connection)
        {
            return connection.Norm().WithCommentCallerInfo();
        }

        /// <summary>
        /// Sets the comment header options for the next command.
        /// </summary>
        /// <param name="comment">Custom comment</param>
        /// <param name="includeParameters">Include command parameters</param>
        /// <param name="includeCommandAttributes">Include command attributes (command type and timeout)</param>
        /// <param name="includeCallerInfo">Include command caller info (source method name, source code file path and line number)</param>
        /// <param name="includeTimestamp">Include execution timestamp</param>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCommentHeader(this DbConnection connection,
            string comment = null,
            bool includeCommandAttributes = true,
            bool includeParameters = true,
            bool includeCallerInfo = true,
            bool includeTimestamp = false)
        {
            return connection.Norm().WithCommentHeader(
                comment: comment,
                includeCommandAttributes: includeCommandAttributes,
                includeParameters: includeParameters,
                includeCallerInfo: includeCallerInfo,
                includeTimestamp: includeTimestamp);
        }
    }
}
