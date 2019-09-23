using System.Threading.Tasks;
using NoOrm.Extensions;

namespace NoOrm
{
    public partial class NoOrm
    {
        public async Task<INoOrm> ExecuteAsync(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return this;
            }
        }

        public async Task<INoOrm> ExecuteAsync(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                await cmd.AddParameters(parameters).ExecuteNonQueryAsync();
                return this;
            }
        }

        public async Task<INoOrm> ExecuteAsync(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                await cmd.AddParameters(parameters).ExecuteNonQueryAsync();
                return this;
            }
        }
    }
}
