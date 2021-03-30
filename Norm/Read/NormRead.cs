using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<(string name, object value)[]> Read(string command)
        {
            return ReadToArrayInternal(command);
        }

        public IEnumerable<(string name, object value)[]> ReadFormat(FormattableString command)
        {
            return ReadToArrayInternal(command);
        }

        public IEnumerable<(string name, object value)[]> Read(string command, params object[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value)[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadToArrayInternal(command, parameters);
        }

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadToArrayInternalUnknowParamsType(command, parameters);
        }
    }
}