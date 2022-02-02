using System;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Norm
{
    public static partial class NormExtensions
    {
        private static readonly ConditionalWeakTable<DbConnection, Norm> Table = new ConditionalWeakTable<DbConnection, Norm>();

        internal static Norm GetNoOrmInstance(this DbConnection connection)
        {
            if (Table.TryGetValue(connection, out var instance))
            {
                return instance;
            }
            instance = new Norm(connection);
            Table.AddOrUpdate(connection, instance);
            return instance;
        }
        ///<summary>
        ///     Set command type for the connection commands and return new Norm instance.
        ///     Default command type for new instance is Text.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="type">
        ///     One of the System.Data.CommandType values.
        ///     Values are Text, StoredProcedure or TableDirect.
        ///</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm As(this DbConnection connection, CommandType type) => 
            connection.GetNoOrmInstance().Clone().As(type);
        ///<summary>
        ///     Set command type to StoredProcedure for the connection commands and return new Norm instance.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm AsProcedure(this DbConnection connection) => 
            connection.As(CommandType.StoredProcedure);
        ///<summary>
        ///     Set command type to Text for the connection commands and return new Norm instance.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm AsText(this DbConnection connection) => 
            connection.As(CommandType.Text);
        ///<summary>
        ///     Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="timeout">Wait time in seconds.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm Timeout(this DbConnection connection, int timeout) => 
            connection.GetNoOrmInstance().Clone().Timeout(timeout);
        ///<summary>
        ///     Sets the default cancellation token for all asynchronous commands for the connection.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="cancellationToken">The token to monitor for cancellation requests.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm WithCancellationToken(this DbConnection connection, CancellationToken cancellationToken) =>
            connection.GetNoOrmInstance().Clone().WithCancellationToken(cancellationToken);
        ///<summary>
        ///     Sets the next command in prepared mode by calling Prepare for the next command.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        public static Norm Prepared(this DbConnection connection) =>
            connection.GetNoOrmInstance().Clone().Prepared();
        ///<summary>
        ///     Next command will use PostgreSQL format function to parse parameter values.
        ///     This allows for parametrized PostgreSQL scripts execution.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<returns>Norm instance that encapsulates the connection.</returns>
        ///<exception cref="ArgumentException">Connection is not PostgreSQL connection or command is in prepared mode.</exception>
        public static Norm UsingPostgresFormatParamsMode(this DbConnection connection) =>
            connection.GetNoOrmInstance().Clone().UsingPostgresFormatParamsMode();
    }
}
