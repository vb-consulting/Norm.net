using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoOrm.Extensions;

namespace NoOrm
{
    public partial class NoOrm
    {
        public async IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return reader.ToTuplesAsync();
            }
        }

        public async IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command, params object[] parameters)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            cmd.AddParameters(parameters);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return reader.ToTuplesAsync();
            }
        }

        public async IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command, params (string name, object value)[] parameters)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            cmd.AddParameters(parameters);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return reader.ToTuplesAsync();
            }
        }
    }
}
