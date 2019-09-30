using System;
using System.Text.Json;
using System.Threading.Tasks;


namespace NoOrm
{
    public partial class NoOrm
    {
        public async ValueTask<T> SingleJsonAsync<T>(string command) => 
            JsonSerializer.Deserialize<T>(await SingleAsync<string>(command), JsonOptions);

        public async ValueTask<T> SingleJsonAsync<T>(string command, params object[] parameters) => 
            JsonSerializer.Deserialize<T>(await SingleAsync<string>(command, parameters), JsonOptions);

        public async ValueTask<T> SingleJsonAsync<T>(string command, params (string name, object value)[] parameters) => 
            JsonSerializer.Deserialize<T>(await SingleAsync<string>(command, parameters), JsonOptions);
    }
}
