using System.Data;
using System.Data.Common;

namespace Norm.Interfaces
{
    public interface INorm :
        INormExecute, 
        INormExecuteAsync,
        INormSingle, 
        INormSingleAsync,
        INormRead,
        INormReadAsync,
        INormQuery,
        INormQueryAsync
    {
        /// Returns:
        ///     returns DbConnection for this instance
        DbConnection Connection { get; }
        ///
        /// Summary:
        ///     Set command type for the connection commands and return Norm instance.
        ///     Default command type for new instance is Text.
        ///
        /// Parameters:
        ///   type:
        ///     One of the System.Data.CommandType values.
        ///     Values are Text, StoredProcedure or TableDirect
        ///
        /// Returns:
        ///     Norm instance.
        INorm As(CommandType type);
        ///
        /// Summary:
        ///     Set command type to StoredProcedure for the connection commands and return Norm instance.
        ///
        /// Returns:
        ///     Norm instance.
        INorm AsProcedure();
        ///
        /// Summary:
        ///     Set command type to Text for the connection commands and return Norm instance.
        ///
        /// Returns:
        ///     Norm instance.
        INorm AsText();
        ///
        /// Summary:
        ///     Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        ///
        /// Parameters:
        ///   timeout:
        ///     Wait time in seconds.
        ///
        /// Returns:
        ///     Norm instance.
        INorm Timeout(int timeout);
        ///
        /// Summary:
        ///     Sets the next command in prepared mode by calling Prepare for the next command.
        ///
        /// Parameters:
        ///   connection:
        ///     DbConnection instance.
        ///
        /// Returns:
        ///     Norm instance.
        INorm Prepared();
        ///
        /// Summary:
        ///     Next command will use PostgreSQL format function to parse parameter values.
        ///     This allows for parametrized PostgreSQL scripts execution.
        ///
        /// Parameters:
        ///
        /// Returns:
        ///     Norm instance.on.
        ///
        /// Exceptions:
        ///   ArgumentException:
        ///     Connecntion is not PostgreSQL connection or command is in prepared mode.
        INorm UsingPostgresFormatParamsMode();
    }
}
