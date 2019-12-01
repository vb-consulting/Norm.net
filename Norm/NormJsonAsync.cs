using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm
    {
        public async IAsyncEnumerable<T> JsonAsync<T>(string command)
        {
            await foreach (var json in ReadInternalAsync(command, r => GetFieldValue<string>(r, 0)))
            {
                yield return JsonSerializer.Deserialize<T>(json, JsonOptions);
            }
        }

        public async IAsyncEnumerable<T> JsonAsync<T>(string command, params object[] parameters)
        {
            {
                await foreach (var json in ReadInternalAsync(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters)))
                {
                    yield return JsonSerializer.Deserialize<T>(json, JsonOptions);
                }
            }
        }

        public async IAsyncEnumerable<T> JsonAsync<T>(string command, params (string name, object value)[] parameters)
        {
            {
                await foreach (var json in ReadInternalAsync(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters)))
                {
                    yield return JsonSerializer.Deserialize<T>(json, JsonOptions);
                }
            }
        }
    }
}
