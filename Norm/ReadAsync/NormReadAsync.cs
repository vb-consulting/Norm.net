using System;
using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command)
        {
            return ReadToArrayInternalAsync(command);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadFormatAsync(FormattableString command)
        {
            return ReadToArrayInternalAsync(command);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params object[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value)[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value, DbType type)[] parameters)
        {
            return ReadToArrayInternalAsync(command, parameters);
        }

        public IAsyncEnumerable<(string name, object value)[]> ReadAsync(string command,
            params (string name, object value, object type)[] parameters)
        {
            return ReadToArrayInternalUnknownParamsTypeAsync(command, parameters);
        }
    }
}