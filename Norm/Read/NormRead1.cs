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
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public virtual IEnumerable<T> Read<T>(string command,
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
                return ReadCallback(command, r => GetFieldValue<T>(r, 0, t1.type));
            }

            if (!t1.simple)
            {
                return ReadToArrayWithWithSetInternal(command).Map<T>(t1.type);
            }
            return ReadCallback(command, r => GetFieldValueWithCallback<T>(r, 0, t1.type));
        }


        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of single values of type T.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Database parameters object (anonymous object or SqlParameter array).</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public virtual IEnumerable<T> ReadFormat<T>(FormattableString command,
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
                return ReadCallback(command, r => GetFieldValue<T>(r, 0, t1.type));
            }

            if (!t1.simple)
            {
                return ReadToArrayWithSetInternal(command).Map<T>(t1.type);
            }
            return ReadCallback(command, r => GetFieldValueWithCallback<T>(r, 0, t1.type));
        }
    }
}