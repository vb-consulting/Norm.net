using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public static NormMultipleReader Multiple(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().Multiple(command);
        }
        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters, execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public static NormMultipleReader MultipleFormat(this DbConnection connection, FormattableString command)
        {
            return connection.GetNoOrmInstance().MultipleFormat(command);
        }
        ///<summary>
        ///     Execute SQL command with positional parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public static NormMultipleReader Multiple(this DbConnection connection, string command, object parameters)
        {
            return connection.GetNoOrmInstance().Multiple(command, parameters);
        }
        ///<summary>
        ///     Execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public static ValueTask<NormMultipleReader> MultipleAsync(this DbConnection connection, string command)
        {
            return connection.GetNoOrmInstance().MultipleAsync(command);
        }
        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters, execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public static ValueTask<NormMultipleReader> MultipleFormatAsync(this DbConnection connection, FormattableString command)
        {
            return connection.GetNoOrmInstance().MultipleFormatAsync(command);
        }
        ///<summary>
        ///     Execute SQL command asynchronously with positional parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public static ValueTask<NormMultipleReader> MultipleAsync(this DbConnection connection, string command, object parameters)
        {
            return connection.GetNoOrmInstance().MultipleAsync(command, parameters);
        }
    }
}
