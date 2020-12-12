using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<T> Query<T>(string command)
        {
            return this.Read(command).Map<T>();
        }

        public IEnumerable<T> Query<T>(string command, params object[] parameters)
        {
            return this.Read(command, parameters).Map<T>();
        }

        public IEnumerable<T> Query<T>(string command, params (string name, object value)[] parameters)
        {
            return this.Read(command, parameters).Map<T>();
        }

        public IEnumerable<T> Query<T>(string command, params (string name, object value, DbType type)[] parameters)
        {
            return this.Read(command, parameters).Map<T>();
        }

        public IEnumerable<T> Query<T>(string command, params (string name, object value, object type)[] parameters)
        {
            return this.Read(command, parameters).Map<T>();
        }
    }
}
