using System;
using System.Text.Json;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm
    {
        public T SingleJson<T>(string command) => 
            JsonSerializer.Deserialize<T>(Single<string>(command), JsonOptions);

        public T SingleJson<T>(string command, params object[] parameters) => 
            JsonSerializer.Deserialize<T>(Single<string>(command, parameters), JsonOptions);

        public T SingleJson<T>(string command, params (string name, object value)[] parameters) => 
            JsonSerializer.Deserialize<T>(Single<string>(command, parameters), JsonOptions);
    }
}
