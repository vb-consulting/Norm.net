using System.Data;
using System.Threading.Tasks;

namespace Norm.Interfaces
{
    public interface INormSingleJsonAsync
    {
        ValueTask<T> SingleJsonAsync<T>(string command);
        ValueTask<T> SingleJsonAsync<T>(string command, params object[] parameters);
        ValueTask<T> SingleJsonAsync<T>(string command, params (string name, object value, DbType type)[] parameters);
    }
}