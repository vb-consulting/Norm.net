using System;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace Norm
{
    public partial class Norm
    {
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
        public Norm AsProcedure() => As(CommandType.StoredProcedure);

        ///<summary>
        ///     Set command type to Text for the connection commands and return Norm instance.
        ///</summary>
        ///<returns>Norm instance.</returns>
        public Norm AsText() => As(CommandType.Text);

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
    }
}
