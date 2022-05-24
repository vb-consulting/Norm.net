using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        /// Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAsync<T>(string command,
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
                return ReadToArrayInternalAsync(command).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayInternalAsync(command).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type));
        }

        ///<summary>
        /// Maps command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAsync<T>(string command,
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
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type, readerCallback));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadFormatAsync<T>(FormattableString command,
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
                return ReadToArrayInternalAsync(command).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayInternalAsync(command).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map command results to async enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadFormatAsync<T>(FormattableString command,
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
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternalAsync(command, readerCallback).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayWithSetInternalAsync(command, readerCallback).Map<T>(t1.type);
            }

            return ReadInternalAsync(command, async r => await GetFieldValueAsync<T>(r, 0, t1.type, readerCallback));
        }
    }
}