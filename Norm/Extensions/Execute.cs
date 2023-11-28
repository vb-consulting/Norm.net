using System;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///     Execute SQL command.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>Same DbConnection instance.</returns>
        public static Norm Execute(this DbConnection connection, string command,
            object parameters = null,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return connection.Instance<Norm>().Execute(command, parameters, memberName, sourceFilePath, sourceLineNumber);
        }
        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and execute resulting SQL.
        ///</summary>
        ///<param name="connection">DbConnection instance</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>Same DbConnection instance.</returns>
        public static Norm ExecuteFormat(this DbConnection connection, FormattableString command,
            object parameters = null,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            return connection.Instance<Norm>().ExecuteFormat(command, parameters, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}
