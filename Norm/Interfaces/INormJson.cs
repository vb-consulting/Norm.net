using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormJson
    {
        IEnumerable<T> Json<T>(string command);
        IEnumerable<T> Json<T>(string command, params object[] parameters);
        IEnumerable<T> Json<T>(string command, params (string name, object value)[] parameters);
        IEnumerable<T> Json<T>(string command, params (string name, object value, DbType type)[] parameters);
    }
}