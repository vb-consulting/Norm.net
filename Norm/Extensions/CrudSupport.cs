using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Norm
{
    public static partial class NormExtensions
    {
        public static IEnumerable<T> Select<T>(this DbConnection connection)
        {
            var type = typeof(T);
            int hash = type.TypeHash();
            var (name, fields) = GetCommandData<T>(type, hash);
            return connection.GetNoOrmInstance().Read(Select(name, fields)).Map<T>(type, hash);
        }

        public static IAsyncEnumerable<T> SelectAsync<T>(this DbConnection connection)
        {
            var type = typeof(T);
            int hash = type.TypeHash();
            var (name, fields) = GetCommandData<T>(type, hash);
            return connection.GetNoOrmInstance().ReadAsync(Select(name, fields)).Map<T>();
        }

        public static IEnumerable<T> Select<T>(this DbConnection connection, params object[] parameters)
        {
            var type = typeof(T);
            int hash = type.TypeHash();
            var (name, fields) = GetCommandData<T>(type, hash);
            return connection.GetNoOrmInstance().Read(string.Concat(Select(name, fields), GetWhere(fields, parameters)), parameters).Map<T>();
        }

        public static IAsyncEnumerable<T> SelectAsync<T>(this DbConnection connection, params object[] parameters)
        {
            var type = typeof(T);
            int hash = type.TypeHash();
            var (name, fields) = GetCommandData<T>(type, hash);
            return connection.GetNoOrmInstance().ReadAsync(string.Concat(Select(name, fields), GetWhere(fields, parameters)), parameters).Map<T>();
        }

        public static IEnumerable<T> Select<T>(this DbConnection connection, params (string name, object value)[] parameters)
        {
            var type = typeof(T);
            int hash = type.TypeHash();
            var (name, fields) = GetCommandData<T>(type, hash);
            return connection.GetNoOrmInstance().Read(string.Concat(Select(name, fields), GetWhere(parameters)), parameters).Map<T>();
        }

        public static IAsyncEnumerable<T> SelectAsync<T>(this DbConnection connection, params (string name, object value)[] parameters)
        {
            var type = typeof(T);
            int hash = type.TypeHash();
            var (name, fields) = GetCommandData<T>(type, hash);
            return connection.GetNoOrmInstance().ReadAsync(string.Concat(Select(name, fields), GetWhere(parameters)), parameters).Map<T>();
        }

        public static InsertValuesBuilder<T> Insert<T>(this DbConnection connection)
        {
            return new InsertValuesBuilder<T>(connection);
        }

        public static InsertValuesBuilder<T> Insert<T>(this DbConnection connection, params string[] fields)
        {
            return new InsertValuesBuilder<T>(connection, fields);
        }

        internal static string GetWhere(string[] fields, params object[] parameters)
        {
            var exp = parameters.Select((_, idx) => {
                var n = fields[idx];
                return $"{n}=@{n}";
            });
            return $" where {string.Join(" and ", exp)}";
        }

        internal static string GetWhere(params (string name, object value)[] parameters)
        {
            return $" where {string.Join(" and ", parameters.Select(p => $"{p.name}=@{p.name}"))}";
        }

        internal static string Select(string name, string[] fields)
        {
            return $"select {string.Join(", ", fields)} from {name}";
        }
    }

    public class InsertValuesBuilder<T>
    {
        private readonly Norm _norm;
        private readonly Type _type;
        private readonly int _hash;
        private readonly string _name;
        private readonly string[] _fields;
        private readonly HashSet<string> _fieldsHash;
        private readonly List<(string name, object value)> _parameters;
        private string _command;
        private List<string> _values;
        private ushort _index;

        internal InsertValuesBuilder(DbConnection connection, string[] fields = null)
        {
            _norm = connection.GetNoOrmInstance();
            _type = typeof(T);
            _hash = _type.TypeHash();
            (_name, _fields) = NormExtensions.GetCommandData<T>(_type, _hash);
            if (fields != null)
            {
                _fields = fields;
            }
            _fieldsHash = new HashSet<string>(_fields);
            _command = $"insert into {_name} ";
            _parameters = new List<(string name, object value)>();
            _values = new List<string>();
            _index = 0;
        }

        public InsertValuesBuilder<T> Values(params object[] values)
        {
            var fields = values.Select((v, i) => _fields[i]);
            if (_index == 0)
            {
                _command = string.Concat(_command, "(", string.Join(", ", fields), ") values ");
            }
            _values.Add(string.Concat("(", string.Join(", ", fields.Select((f, i) =>
            {
                var name = string.Concat(f, _index);
                this._parameters.Add((name, values[i]));
                return string.Concat("@", name);
            })), ")"));
            _index++;
            return this;
        }

        public InsertValuesBuilder<T> Values(params (string name, object value)[] parameters)
        {
            if (_index == 0)
            {
                _command = string.Concat(_command, "(", string.Join(", ", _fields), ") values ");
            }
            var dict = parameters.ToDictionary(p => p.name.ToLower(), p => p.value);
            _values.Add(string.Concat("(", string.Join(", ", _fields.Select((f, i) =>
            {
                var name = string.Concat(f, _index);
                _parameters.Add((name, dict[f]));
                return string.Concat("@", name);
            })), ")"));
            _index++;
            return this;
        }

        public InsertValuesBuilder<T> Values(T instance)
        {
            var properties = NormExtensions.GetProperties(_hash, _type);
            if (_index == 0)
            {
                _command = string.Concat(_command, "(", string.Join(", ", _fields), ") values ");
            }
            _values.Add(string.Concat( "(", string.Join(", ", properties.Select((p, i) =>
            {
                var lower = p.Name.ToLower();
                if (_fieldsHash.Contains(lower))
                {
                    var name = string.Concat(lower, _index);
                    _parameters.Add((name, p.GetValue(instance)));
                    return string.Concat("@", name);
                }
                return null;
            }).Where(p => p != null)), ")"));
            _index++;
            return this;
        }

        public void Execute()
        {
            var cmd = string.Concat(_command, string.Join(", ", _values));
            _norm.Execute(cmd, _parameters.ToArray());
        }

        public async ValueTask ExecuteAsync()
        {
            var cmd = string.Concat(_command, string.Join(", ", _values));
            await _norm.ExecuteAsync(cmd, _parameters.ToArray());
        }

        public IEnumerable<T> Returning()
        {
            var cmd = string.Concat(_command, string.Join(", ", _values), " returning *");
            return _norm.Read(cmd, _parameters.ToArray()).Map<T>(_type, _hash);
        }

        public IAsyncEnumerable<T> ReturningAsync()
        {
            var cmd = string.Concat(_command, string.Join(", ", _values), " returning *");
            return _norm.ReadAsync(cmd, _parameters.ToArray()).Map<T>(_type, _hash);
        }
    }
}
