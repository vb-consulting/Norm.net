using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public async Task<INoOrm> ExecuteAsync(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return this;
            }
        }

        public async Task<INoOrm> ExecuteAsync(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                await cmd.AddParameters(parameters).ExecuteNonQueryAsync();
                return this;
            }
        }

        public async Task<INoOrm> ExecuteAsync(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await EnsureConnectionIsOpenAsync();
                await cmd.AddParameters(parameters).ExecuteNonQueryAsync();
                return this;
            }
        }
    }
}
