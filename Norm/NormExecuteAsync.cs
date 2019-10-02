using System.Threading.Tasks;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm
    {
        public async ValueTask<INorm> ExecuteAsync(string command)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            await cmd.ExecuteNonQueryAsync();
            OnCommandExecuted(cmd);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params object[] parameters)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            await cmd.AddParameters(parameters).ExecuteNonQueryAsync();
            OnCommandExecuted(cmd);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params (string name, object value)[] parameters)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync();
            await cmd.AddParameters(parameters).ExecuteNonQueryAsync();
            OnCommandExecuted(cmd);
            return this;
        }
    }
}
