using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NoOrm
{
    public partial class NoOrmAccess : IDisposable, INoOrm
    {
        private CommandType commandType;
        private int? commandTimeout;

        public DbConnection Connection { get; }

        public NoOrmAccess(DbConnection connection, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            Connection = connection;
            this.commandType = commandType;
            this.commandTimeout = commandTimeout;
        }

        public INoOrm As(CommandType type)
        {
            commandType = type;
            return this;
        }

        public INoOrm Timeout(int? timeout)
        {
            commandTimeout = timeout;
            return this;
        }

        public void Dispose()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
            Connection?.Dispose();
        }

        private void EnsureConnectionIsOpen()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        private async Task EnsureConnectionIsOpenAsync()
        {
            if (Connection.State != ConnectionState.Open)
            {
                await Connection.OpenAsync();
            }
        }

        private void SetCommand(DbCommand cmd, string command)
        {
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            if (commandTimeout != null)
            {
                cmd.CommandTimeout = commandTimeout.Value;
            }
        }
    }
}
