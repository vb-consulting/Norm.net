using System.Threading.Tasks;

namespace Norm.Interfaces
{
    public interface INormExecuteAsync
    {
        ValueTask<INorm> ExecuteAsync(string command);
        ValueTask<INorm> ExecuteAsync(string command, params object[] parameters);
        ValueTask<INorm> ExecuteAsync(string command, params (string name, object value)[] parameters);
    }
}