using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command)
        {
            return ReadToArrayInternal(command);
        }

        ///<summary>
        ///     Maps command results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> Read(string command, 
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return ReadToArrayInternal(command, readerCallback);
        }

        ///<summary>
        ///      Parse interpolated (formattable) command as database parameters and map results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> ReadFormat(FormattableString command)
        {
            return ReadToArrayInternal(command);
        }

        ///<summary>
        ///      Parse interpolated (formattable) command as database parameters and map results to enumerator of name and value tuple arrays.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IEnumerable enumerator of name and value tuple arrays.</returns>
        public IEnumerable<(string name, object value)[]> ReadFormat(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            return ReadToArrayInternal(command, readerCallback);
        }
    }
}