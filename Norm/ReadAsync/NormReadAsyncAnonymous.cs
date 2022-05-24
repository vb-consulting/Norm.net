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
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAnonymousAsync<T>(T anonymousBlueprintInstance, string command) where T : class
        {
            return ReadToArrayInternalAsync(command).MapAnonymous<T>(anonymousBlueprintInstance.GetType());
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAnonymousAsync<T>(T anonymousBlueprintInstance, string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback) where T : class
        {
            return ReadToArrayInternalAsync(command, readerCallback).MapAnonymous<T>(anonymousBlueprintInstance.GetType());
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAnonymousFormatAsync<T>(T anonymousBlueprintInstance, FormattableString command) where T : class
        {
            return ReadToArrayInternalAsync(command).MapAnonymous<T>(anonymousBlueprintInstance.GetType());
        }

        ///<summary>
        ///Maps command results to async enumerator of single values of type T.
        ///</summary>
        ///<param name="anonymousBlueprintInstance">Anonymous instance used as blueprint to create new instances of same anonymous types</param>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<param name="readerCallback">A callback function, that is executed on each read iteration to provide an alternate mapping.</param>
        ///<returns>IAsyncEnumerable async enumerator of single values of type T.</returns>
        public IAsyncEnumerable<T> ReadAnonymousFormatAsync<T>(T anonymousBlueprintInstance, FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback) where T : class
        {
            return ReadToArrayInternalAsync(command, readerCallback).MapAnonymous<T>(anonymousBlueprintInstance.GetType());
        }
    }
}