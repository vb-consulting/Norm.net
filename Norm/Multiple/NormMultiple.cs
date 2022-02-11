using System;
using System.Data;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader Multiple(string command)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters, execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader MultipleFormat(FormattableString command)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public async ValueTask<NormMultipleReader> MultipleAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters, execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public async ValueTask<NormMultipleReader> MultipleFormatAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command with positional parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader Multiple(string command, params object[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        ///<param name="parameters">Parameters objects array.</param>
        public async ValueTask<NormMultipleReader> MultipleAsync(string command, params object[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }
    }
}
