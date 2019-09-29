using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm
    {
        public IAsyncEnumerable<T> DeserializeJsonAsync<T>(string command) =>
            ReadInternalAsync(command, r => GetFieldValue<string>(r, 0))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));

        public IAsyncEnumerable<T> DeserializeJsonAsync<T>(string command, params object[] parameters) =>
            ReadInternalAsync(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));

        public IAsyncEnumerable<T> DeserializeJsonAsync<T>(string command, params (string name, object value)[] parameters) =>
            ReadInternalAsync(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));
    }
}
