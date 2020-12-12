using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        public IAsyncEnumerable<T> QueryAsync<T>(string command)
        {
            return this.ReadAsync(command).Map<T>();
        }

        public IAsyncEnumerable<T> QueryAsync<T>(string command, params object[] parameters)
        {
            return this.ReadAsync(command, parameters).Map<T>();
        }

        public IAsyncEnumerable<T> QueryAsync<T>(string command, params (string name, object value)[] parameters)
        {
            return this.ReadAsync(command, parameters).Map<T>();
        }

        public IAsyncEnumerable<T> QueryAsync<T>(string command, params (string name, object value, DbType type)[] parameters)
        {
            return this.ReadAsync(command, parameters).Map<T>();
        }

        public IAsyncEnumerable<T> QueryAsync<T>(string command, params (string name, object value, object type)[] parameters)
        {
            return this.ReadAsync(command, parameters).Map<T>();
        }
    }
}
