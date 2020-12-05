using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Norm.Extensions
{
    public static partial class ConnectionExtensions
    {
        private static readonly ConcurrentDictionary<int, (string name, string[] fields, string select)> CommandCache = 
            new ConcurrentDictionary<int, (string name, string[] fields, string select)>();

        public static IEnumerable<T> Get<T>(this DbConnection connection)
        {
            var (_, _, select) = GetCommandData<T>();
            return connection.GetNoOrmInstance().Read(select).Select<T>();
        }

        public static IAsyncEnumerable<T> GetAsync<T>(this DbConnection connection)
        {
            var (_, _, select) = GetCommandData<T>();
            return connection.GetNoOrmInstance().ReadAsync(select).Select<T>();
        }

        public static IEnumerable<T> Get<T>(this DbConnection connection, params object[] parameters)
        {
            var (_, fields, select) = GetCommandData<T>();
            return connection.GetNoOrmInstance().Read(string.Concat(select, GetWhere(fields, parameters)), parameters).Select<T>();
        }

        public static IAsyncEnumerable<T> GetAsync<T>(this DbConnection connection, params object[] parameters)
        {
            var (_, fields, select) = GetCommandData<T>();
            return connection.GetNoOrmInstance().ReadAsync(string.Concat(select, GetWhere(fields, parameters)), parameters).Select<T>();
        }

        public static IEnumerable<T> Get<T>(this DbConnection connection, params (string name, object value)[] parameters)
        {
            var (_, _, select) = GetCommandData<T>();
            return connection.GetNoOrmInstance().Read(string.Concat(select, GetWhere(parameters)), parameters).Select<T>();
        }

        public static IAsyncEnumerable<T> GetAsync<T>(this DbConnection connection, params (string name, object value)[] parameters)
        {
            var (_, _, select) = GetCommandData<T>();
            return connection.GetNoOrmInstance().ReadAsync(string.Concat(select, GetWhere(parameters)), parameters).Select<T>();
        }

        private static string GetWhere(string[] fields, params object[] parameters)
        {
            var exp = parameters.Select((_, idx) => {
                var n = fields[idx];
                return $"{n}=@{n}";
            });
            return $" where {string.Join(" and ", exp)}";
        }

        private static string GetWhere(params (string name, object value)[] parameters)
        {
            return $" where {string.Join(" and ", parameters.Select(p => $"{p.name}=@{p.name}"))}";
        }

        private static (string name, string[] fields, string select) GetCommandData<T>()
        {
            var type = typeof(T);
            var hashCode = type.GetHashCode();
            return CommandCache.GetOrAdd(hashCode, _ =>
            {
                var name = type.Name.ToLower();
                var fields = type.GetProperties().Select(p => p.Name.ToLower()).ToArray();
                var select = $"select {string.Join(',', fields)} from {name}";
                return (name, fields, select);
            });
        }
    }
}
