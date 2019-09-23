using System.Linq;
using System.Threading.Tasks;
using NoOrm.Extensions;

namespace NoOrm
{
    public partial class NoOrm
    {
        public async Task<INoOrm> ReadAsync(string command, RowCallback results)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results(reader.GetValuesFromReader().ToArray());
                    }
                    return this;
                }
            }
        }

        public async Task<INoOrm> ReadAsync(string command, RowCallback results, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results(reader.GetValuesFromReader().ToArray());
                    }
                    return this;
                }
            }
        }

        public async Task<INoOrm> ReadAsync(string command, RowCallback results, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results(reader.GetValuesFromReader().ToArray());
                    }
                    return this;
                }
            }
        }

        public async Task<INoOrm> ReadAsync(string command, RowCallbackAsync results)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        await results(reader.GetValuesFromReader().ToArray());
                    }

                    return this;
                }
            }
        }

        public async Task<INoOrm> ReadAsync(string command, RowCallbackAsync results, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        await results(reader.GetValuesFromReader());
                    }

                    return this;
                }
            }
        }

        public async Task<INoOrm> ReadAsync(string command, RowCallbackAsync results, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                await Connection.EnsureIsOpenAsync();
                cmd.AddParameters(parameters);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        await results(reader.GetValuesFromReader());
                    }

                    return this;
                }
            }
        }

    }
}
