using System;
using System.Data;
using System.Runtime.CompilerServices;
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
        public async ValueTask<Norm> ExecuteAsync(string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
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
        public async ValueTask<Norm> ExecuteFormatAsync(FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
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
    }
}
