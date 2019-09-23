using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoOrm.Extensions;

namespace NoOrm
{
    public partial class NoOrm
    {
        public async Task<IEnumerable<(string name, object value)>> SingleAsync(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                using (var reader = cmd.ExecuteReader())
                {
                    return await reader.ReadAsync()
                        ? reader.GetTuplesFromReader().ToList()
                        : new List<(string name, object value)>();
                }
            }
        }

        public async Task<IEnumerable<(string name, object value)>> SingleAsync(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    return await reader.ReadAsync() 
                        ? reader.GetTuplesFromReader().ToList()
                        : new List<(string name, object value)>();
                }
            }
        }

        public async Task<IEnumerable<(string name, object value)>> SingleAsync(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    return await reader.ReadAsync()
                        ? reader.GetTuplesFromReader().ToList()
                        : new List<(string name, object value)>();
                }
            }
        }
    }
}
