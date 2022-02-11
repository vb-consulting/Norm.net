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
        /// Maps command results to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> Read<T>(string command)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayInternal(command).Map<T>(t1.type);
            }

            return ReadInternal(command, r => GetFieldValue<T>(r, 0, t1.type));
        }

        ///<summary>
        /// Maps command results to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> Read<T>(string command, 
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback).Map<T>(t1.type);
            }

            return ReadInternal(command, r => GetFieldValue<T>(r, 0, t1.type, readerCallback));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> ReadFormat<T>(FormattableString command)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternal(command).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayInternal(command).Map<T>(t1.type);
            }

            return ReadInternal(command, r => GetFieldValue<T>(r, 0, t1.type));
        }

        ///<summary>
        /// Parse interpolated (formattable) command as database parameters and map results to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        /// <param name="readerCallback"></param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> ReadFormat<T>(FormattableString command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback).Map<T>(t1.type);
            }

            return ReadInternal(command, r => GetFieldValue<T>(r, 0, t1.type, readerCallback));
        }

        ///<summary>
        /// Maps command results with positional parameter values to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> Read<T>(string command, params object[] parameters)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternal(command, parameters).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayInternal(command, parameters).Map<T>(t1.type);
            }

            return ReadInternal(command, r => GetFieldValue<T>(r, 0, t1.type), parameters);
        }

        ///<summary>
        /// Maps command results with positional parameter values to enumerator of single values of type T.
        /// If type T is a class or a record, results will be mapped by name to a class or record instances by name.
        /// If type T is a named tuple, results will be mapped by name to a named tuple instances by position.
        /// Otherwise, single value is mapped.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        /// <param name="readerCallback"></param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>IEnumerable enumerator of single values of type T.</returns>
        public IEnumerable<T> Read<T>(string command,
            Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
            params object[] parameters)
        {
            var t1 = TypeCache<T>.GetMetadata();
            if (t1.valueTuple)
            {
                return ReadToArrayInternal(command, readerCallback, parameters).MapValueTuple<T>(t1.type);
            }
            if (!t1.simple)
            {
                return ReadToArrayWithSetInternal(command, readerCallback, parameters).Map<T>(t1.type);
            }

            return ReadInternal(command, r => GetFieldValue<T>(r, 0, t1.type, readerCallback), parameters);
        }
    }
}