using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///Maps command results to enumerator of single values of type T.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> Read<T>(string command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            var t1 = TypeCache<T>.GetMetadata();
            
            if (t1.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T>(t1.type);
            }

            if (this.readerCallback == null)
            {
                if (!t1.simple)
                {
                    return ReadToArrayInternal(command).Map<T>(t1.type);
                }
                return ReadInternal(command, r => GetFieldValue<T>(r, 0, t1.type));
            }

            if (!t1.simple)
            {
                return ReadToArrayWithCallbackInternal(command).Map<T>(t1.type);
            }
            return ReadInternal(command, r => GetFieldValueWithCallback<T>(r, 0, t1.type));
        }


        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of single values of type T.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> ReadFormat<T>(FormattableString command,
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
        {
            this.memberName = memberName;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T>(t1.type);
            }
            if (this.readerCallback == null)
            {
                if (!t1.simple)
                {
                    return ReadToArrayInternal(command).Map<T>(t1.type);
                }
                return ReadInternal(command, r => GetFieldValue<T>(r, 0, t1.type));
            }

            if (!t1.simple)
            {
                return ReadToArrayWithCallbackInternal(command).Map<T>(t1.type);
            }
            return ReadInternal(command, r => GetFieldValueWithCallback<T>(r, 0, t1.type));
        }
    }
}