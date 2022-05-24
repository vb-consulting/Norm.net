using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            return ReadToArrayInternalAsync(command);
        }

        ///<summary>
        ///     Maps command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return ReadToArrayInternalAsync(command, readerCallback);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadFormatAsync(FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            return ReadToArrayInternalAsync(command);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and map command results to async enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of name and value tuple arrays.</returns>
        public IAsyncEnumerable<(string name, object value)[]> ReadFormatAsync(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            return ReadToArrayInternalAsync(command, readerCallback);
        }
    }
}