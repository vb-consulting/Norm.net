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
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteAsync(string command,
            object parameters = null,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            if (parameters != null)
            {
                this.WithParameters(parameters);
            }
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                this.recordsAffected = await cmd.ExecuteNonQueryAsync(cancellationToken.Value);
            }
            else
            {
                this.recordsAffected = await cmd.ExecuteNonQueryAsync();
            }
            return this;
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and execute resulting SQL asynchronously.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteFormatAsync(FormattableString command,
            object parameters = null,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            if (parameters != null)
            {
                this.WithParameters(parameters);
            }
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                this.recordsAffected = await cmd.ExecuteNonQueryAsync(cancellationToken.Value);
            }
            else
            {
                this.recordsAffected = await cmd.ExecuteNonQueryAsync();
            }
            return this;
        }
    }
}
