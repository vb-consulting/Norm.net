using System.Data;
using System.Threading.Tasks;
using Norm.Extensions;
using Norm.Interfaces;

namespace Norm
{
    public partial class Norm
    {
        public async ValueTask<INorm> ExecuteAsync(string command)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await (await PrepareAsync(cmd)).ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params object[] parameters)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await (await AddParametersAsync(cmd, parameters)).ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params (string name, object value)[] parameters)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await (await AddParametersAsync(cmd, parameters)).ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params (string name, object value, DbType type)[] parameters)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await (await AddParametersAsync(cmd, parameters)).ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        public async ValueTask<INorm> ExecuteAsync(string command, params (string name, object value, object type)[] parameters)
        {
            await using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            await Connection.EnsureIsOpenAsync(cancellationToken);
            await (await AddParametersUnknownTypeAsync(cmd, parameters)).ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }
    }
}
