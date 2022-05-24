using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Execute SQL command asynchronously.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await cmd.ExecuteNonQueryAsync(cancellationToken.Value);
            }
            else
            {
                await cmd.ExecuteNonQueryAsync();
            }
            return this;
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and execute resulting SQL asynchronously.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteFormatAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                await cmd.ExecuteNonQueryAsync(cancellationToken.Value);
            }
            else
            {
                await cmd.ExecuteNonQueryAsync();
            }
            return this;
        }

        ///<summary>
        ///      Execute SQL command asynchronously with positional parameter values.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array. The parameter can be a simple value (mapped by position), DbParameter instance, or object instance where is each property is mapped to parameters.</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteAsync(string command, object parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                await cmd.ExecuteNonQueryAsync(cancellationToken.Value);
            }
            else
            {
                await cmd.ExecuteNonQueryAsync();
            }
            return this;
        }
    }
}
