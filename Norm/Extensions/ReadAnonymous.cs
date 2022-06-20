using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Norm
{
    public static partial class NormExtensions
    {
        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public static IEnumerable<T> Read<T>(this DbConnection connection, T anonymousBlueprintInstance, string command,
            object parameters = null,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where T : class
        {
            return connection.Instance<Norm>().Read(anonymousBlueprintInstance, command, parameters, memberName, sourceFilePath, sourceLineNumber);
        }

        ///<summary>
        ///Parse interpolated (formattable) command as database parameters and map command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="connection">DbConnection instance.</param>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public static IEnumerable<T> ReadFormat<T>(this DbConnection connection, T anonymousBlueprintInstance, FormattableString command,
            object parameters = null,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where T : class
        {
            return connection.Instance<Norm>().ReadFormat<T>(anonymousBlueprintInstance, command, parameters, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}