using System.Collections.Generic;

namespace Norm.Interfaces
{
    public interface INormJson
    {
        IEnumerable<T> Json<T>(string command);
        IEnumerable<T> Json<T>(string command, params object[] parameters);
        IEnumerable<T> Json<T>(string command, params (string name, object value)[] parameters);
    }
}