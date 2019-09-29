using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm
    {
        public IEnumerable<T> DeserializeJson<T>(string command) => 
            ReadInternal(command, r => GetFieldValue<string>(r, 0))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));

        public IEnumerable<T> DeserializeJson<T>(string command, params object[] parameters) =>
            ReadInternal(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));

        public IEnumerable<T> DeserializeJson<T>(string command, params (string name, object value)[] parameters) =>
            ReadInternal(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));
    }
}

