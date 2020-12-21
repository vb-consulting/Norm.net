using System.Data;
using System.Threading.Tasks;
using Norm.Interfaces;

namespace Norm
{
    public partial class Norm
    {
        public async ValueTask<INorm> ExecuteAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params object[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params (string name, object value)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }
    }
}
