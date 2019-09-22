using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public async Task<INoOrm> ReadAsync(string command, Action<IDictionary<string, object>> results)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                    }
                    return this;
                }
            }
        }

        public async Task<INoOrm> ReadAsync(string command, Action<IDictionary<string, object>> results, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                    }
                    return this;
                }
            }
        }

        public async Task<INoOrm> ReadAsync(string command, Action<IDictionary<string, object>> results, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                    }
                    return this;
                }
            }
        }
    }
}
