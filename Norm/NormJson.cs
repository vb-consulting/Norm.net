using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<T> Json<T>(string command) => 
            ReadInternal(command, r => GetFieldValue<string>(r, 0))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));

        public IEnumerable<T> Json<T>(string command, params object[] parameters) =>
            ReadInternal(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));

        public IEnumerable<T> Json<T>(string command, params (string name, object value)[] parameters) =>
            ReadInternal(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));

        public IEnumerable<T> Json<T>(string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command, r => GetFieldValue<string>(r, 0), cmd => cmd.AddParameters(parameters))
                .Select(json => JsonSerializer.Deserialize<T>(json, JsonOptions));
    }
}

