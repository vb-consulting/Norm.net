using System;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using Norm.Interfaces;

namespace Norm
{
    public static partial class NormExtensions
    {
        private static readonly ConditionalWeakTable<DbConnection, Norm> Table;

        static NormExtensions()
        {
            Table = new ConditionalWeakTable<DbConnection, Norm>();
        }

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
        //
        // Summary:
        //     Set command type for the connection commands and return new Norm instance.
        //     Default command type for new instance is Text.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        //   type:
        //     One of the System.Data.CommandType values.
        //     Values are Text, StoredProcedure or TableDirect
        //
        // Returns:
        //     Norm instance that encapsulates the connection.
        public static INorm As(this DbConnection connection, CommandType type) => 
            connection.GetNoOrmInstance().Clone().As(type);
        //
        // Summary:
        //     Set command type to StoredProcedure for the connection commands and return new Norm instance.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        // Returns:
        //     Norm instance that encapsulates the connection.
        public static INorm AsProcedure(this DbConnection connection) => 
            connection.As(CommandType.StoredProcedure);
        //
        // Summary:
        //     Set command type to Text for the connection commands and return new Norm instance.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        // Returns:
        //     Norm instance that encapsulates the connection.
        public static INorm AsText(this DbConnection connection) => 
            connection.As(CommandType.Text);
        //
        // Summary:
        //     Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        //   timeout:
        //     Wait time in seconds.
        //
        // Returns:
        //     Norm instance that encapsulates the connection.
        public static INorm Timeout(this DbConnection connection, int timeout) => 
            connection.GetNoOrmInstance().Clone().Timeout(timeout);
        //
        // Summary:
        //     Sets the default cancellation token for all asynchronous commands for the connection.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        //   cancellationToken:
        //     The token to monitor for cancellation requests.
        //
        // Returns:
        //     Norm instance that encapsulates the connection.
        public static INorm WithCancellationToken(this DbConnection connection, CancellationToken cancellationToken) =>
            connection.GetNoOrmInstance().Clone().WithCancellationToken(cancellationToken);
        //
        // Summary:
        //     Sets the next command in prepared mode by calling Prepare for the next command.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        // Returns:
        //     Norm instance that encapsulates the connection.
        public static INorm Prepared(this DbConnection connection) =>
            connection.GetNoOrmInstance().Clone().Prepared();
        //
        // Summary:
        //     Next command will use PostgreSQL format function to parse parameter values.
        //     This allows for parametrized PostgreSQL scripts execution.
        //
        // Parameters:
        //   connection:
        //     DbConnection instance.
        //
        // Returns:
        //     Norm instance that encapsulates the connection.
        //
        // Exceptions:
        //   ArgumentException:
        //     Connecntion is not PostgreSQL connection or command is in prepared mode.
        public static INorm UsingPostgresFormatParamsMode(this DbConnection connection) =>
            connection.GetNoOrmInstance().Clone().UsingPostgresFormatParamsMode();
    }
}
