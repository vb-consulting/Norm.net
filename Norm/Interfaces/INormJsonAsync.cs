using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormJsonAsync
    {
        IAsyncEnumerable<T> JsonAsync<T>(string command);
        IAsyncEnumerable<T> JsonAsync<T>(string command, params object[] parameters);
        IAsyncEnumerable<T> JsonAsync<T>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<T> JsonAsync<T>(string command, params (string name, object value, DbType type)[] parameters);
    }
}