using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public async Task<IDictionary<string, object>> SingleAsync(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                using (var reader = cmd.ExecuteReader())
                {
                    return await reader.ReadAsync() 
                        ? Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue) 
                        : new Dictionary<string, object>();
                }
            }
        }

        public async Task<IDictionary<string, object>> SingleAsync(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    return await reader.ReadAsync() 
                        ? Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue) 
                        : new Dictionary<string, object>();
                }
            }
        }

        public async Task<IDictionary<string, object>> SingleAsync(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    return await reader.ReadAsync()
                        ? Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)
                        : new Dictionary<string, object>();
                }
            }
        }
    }
}
