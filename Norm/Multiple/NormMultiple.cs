using System;
using System.Data;
using System.Runtime.CompilerServices;
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
        public NormMultipleReader Multiple(string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters, execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader MultipleFormat(FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public async ValueTask<NormMultipleReader> MultipleAsync(string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
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
        public async ValueTask<NormMultipleReader> MultipleFormatAsync(FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }
    }
}
