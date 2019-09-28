using System;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using System.Threading.Tasks;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm : IDisposable, INoOrm
    {
        private enum DbType { Ms, Pg, Other }
    
        private CommandType commandType;
        private int? commandTimeout;
        private JsonSerializerOptions jsonOptions;
        private readonly bool convertsDbNull;
        private readonly DbType dbType;
        private static readonly Type StringType = typeof(string);

        public DbConnection Connection { get; }

        public NoOrm(DbConnection connection, CommandType commandType = CommandType.Text, int? commandTimeout = null, JsonSerializerOptions jsonOptions = null)
        {
            Connection = connection;
            this.commandType = commandType;
            this.commandTimeout = commandTimeout;
            this.jsonOptions = jsonOptions;
            var name = connection.GetType().Name;
            dbType = name switch { "SqlConnection" => DbType.Ms, "NpgsqlConnection" => DbType.Pg, _ => DbType.Other };
            convertsDbNull = dbType != DbType.Ms;
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

        public INoOrm WithJsonOptions(JsonSerializerOptions jsonOptions)
        {
            this.jsonOptions = jsonOptions;
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

        private JsonSerializerOptions JsonOptions
        {
            get
            {
                if (this.jsonOptions != null)
                {
                    return this.jsonOptions;
                }
                return GlobalJsonSerializerOptions.Options;
            }
        }

        private void SetCommand(DbCommand cmd, string command)
            => cmd.SetCommandParameters(command, commandType, commandTimeout);

        private bool CheckDbNull<T>() => (!convertsDbNull || typeof(T) == StringType);

        private T GetFieldValue<T>(DbDataReader reader, int ordinal)
        {
            if (CheckDbNull<T>())
            {
                return reader.IsDBNull(ordinal) ? default : reader.GetFieldValue<T>(ordinal);
            }
            return reader.GetFieldValue<T>(ordinal);
        }

        private async Task<T> GetFieldValueAsync<T>(DbDataReader reader, int ordinal)
        {
            if (CheckDbNull<T>())
            {
                return await reader.IsDBNullAsync(ordinal) ? default : await reader.GetFieldValueAsync<T>(ordinal);
            }
            return await reader.GetFieldValueAsync<T>(ordinal);
        }
    }
}
