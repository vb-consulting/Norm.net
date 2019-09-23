using System;
using System.Data;
using System.Data.Common;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm : IDisposable, INoOrm
    {
        private CommandType commandType;
        private int? commandTimeout;

        public DbConnection Connection { get; }

        public NoOrm(DbConnection connection, CommandType commandType = CommandType.Text, int? commandTimeout = null)
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

        private void SetCommand(DbCommand cmd, string command)
            => cmd.SetCommandParameters(command, commandType, commandTimeout);
    }
}
