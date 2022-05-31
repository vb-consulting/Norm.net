using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public virtual IEnumerable<T> Read<T>(T anonymousBlueprintInstance, string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where T : class
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            return ReadToArrayInternal(command).MapAnonymous<T>(anonymousBlueprintInstance.GetType());
        }

        ///<summary>
        ///Maps command results to enumerator of anonymous values.
        ///</summary>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public virtual IEnumerable<T> ReadFormat<T>(T anonymousBlueprintInstance, FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            where T : class
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            return ReadToArrayInternal(command).MapAnonymous<T>(anonymousBlueprintInstance.GetType());
        }
    }
}