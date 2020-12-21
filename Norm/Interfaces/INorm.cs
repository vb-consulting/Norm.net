using System;
using System.Data;
using System.Data.Common;

namespace Norm.Interfaces
{
    public interface INorm :
        INormExecute, 
        INormExecuteAsync,
        INormRead,
        INormReadAsync,
        INormMultiple
    {
        ///<summary>
        ///     returns DbConnection for this instance
        ///</summary>
        DbConnection Connection { get; }
        ///<summary>
        ///     Set command type for the connection commands and return Norm instance.
        ///     Default command type for new instance is Text.
        ///</summary>
        ///<param name="type">
        ///     One of the System.Data.CommandType values.
        ///     Values are Text, StoredProcedure or TableDirect.
        ///</param>
        ///<returns>Norm instance.</returns>
        INorm As(CommandType type);
        ///<summary>
        ///     Set command type to StoredProcedure for the connection commands and return Norm instance.
        ///</summary>
        ///<returns>Norm instance.</returns>
        INorm AsProcedure();
        ///<summary>
        ///     Set command type to Text for the connection commands and return Norm instance.
        ///</summary>
        ///<returns>Norm instance.</returns>
        INorm AsText();
        ///<summary>
        ///     Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        ///</summary>
        ///<param name="timeout">Wait time in seconds.</param>
        ///<returns>Norm instance.</returns>
        INorm Timeout(int timeout);
        ///<summary>
        ///     Sets the next command in prepared mode by calling Prepare for the next command.
        ///</summary>
        ///<returns>Norm instance.</returns>
        INorm Prepared();
        ///<summary>
        ///     Next command will use PostgreSQL format function to parse parameter values.
        ///     This allows for parametrized PostgreSQL scripts execution.
        ///</summary>
        ///
        ///<returns>Norm instance.</returns>on.
        ///<exception cref="ArgumentException">Connection is not PostgreSQL connection or command is in prepared mode.</exception>.
        INorm UsingPostgresFormatParamsMode();
    }
}
