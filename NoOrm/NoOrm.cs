using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm : IDisposable, INoOrm
    {
        private enum DbType { Ms, Pg, Other }
    
        private CommandType commandType;
        private int? commandTimeout;
        private readonly bool convertsDbNull;
        private readonly DbType type;

        public DbConnection Connection { get; }

        public NoOrm(DbConnection connection, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            Connection = connection;
            this.commandType = commandType;
            this.commandTimeout = commandTimeout;
            var name = connection.GetType().Name;
            type = name switch { "SqlConnection" => DbType.Ms, "NpgsqlConnection" => DbType.Pg, _ => DbType.Other };
            convertsDbNull = type != DbType.Ms;
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

        private T GetFieldValue<T>(DbDataReader reader, int ordinal)
        {
            if (!convertsDbNull)
            {
                return reader.IsDBNull(ordinal) ? default : reader.GetFieldValue<T>(ordinal);
            }
            return reader.GetFieldValue<T>(ordinal);
        }

        private async Task<T> GetFieldValueAsync<T>(DbDataReader reader, int ordinal)
        {
            if (!convertsDbNull)
            {
                return await reader.IsDBNullAsync(ordinal) ? default : await reader.GetFieldValueAsync<T>(ordinal);
            }
            return await reader.GetFieldValueAsync<T>(ordinal);
        }
    }
}
