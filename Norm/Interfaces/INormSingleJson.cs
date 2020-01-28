using System.Data;

namespace Norm.Interfaces
{
    public interface INormSingleJson
    {
        T SingleJson<T>(string command);
        T SingleJson<T>(string command, params object[] parameters);
        T SingleJson<T>(string command, params (string name, object value)[] parameters);
        T SingleJson<T>(string command, params (string name, object value, DbType type)[] parameters);
    }
}