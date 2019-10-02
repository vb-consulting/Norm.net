using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using System.Threading.Tasks;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm : IDisposable, INorm
    {
        private enum DbType
        {
            Ms,
            Pg,
            Other
        }

        private CommandType commandType;
        private int? commandTimeout;
        private JsonSerializerOptions jsonOptions;
        private readonly bool convertsDbNull;
        private readonly DbType dbType;
        private static readonly Type StringType = typeof(string);
        private readonly IList<(string name, ParameterDirection direction, object value)> outputParams;
        private readonly IDictionary<string, object> outputParamValues;

        public DbConnection Connection { get; }

        public Norm(DbConnection connection, CommandType commandType = CommandType.Text, int? commandTimeout = null,
            JsonSerializerOptions jsonOptions = null)
        {
            Connection = connection;
            this.commandType = commandType;
            this.commandTimeout = commandTimeout;
            this.jsonOptions = jsonOptions;
            var name = connection.GetType().Name;
            dbType = name switch {"SqlConnection" => DbType.Ms, "NpgsqlConnection" => DbType.Pg, _ => DbType.Other};
            convertsDbNull = dbType != DbType.Ms;
            outputParams = new List<(string name, ParameterDirection direction, object value)>();
            outputParamValues = new Dictionary<string, object>();
        }

        public INorm As(CommandType type)
        {
            commandType = type;
            return this;
        }

        public INorm AsProcedure() => As(CommandType.StoredProcedure);

        public INorm AsText() => As(CommandType.Text);

        public INorm Timeout(int? timeout)
        {
            commandTimeout = timeout;
            return this;
        }

        public INorm WithJsonOptions(JsonSerializerOptions options)
        {
            this.jsonOptions = options;
            return this;
        }

        public INorm WithOutParameter(string name)
        {
            outputParams.Add((name, ParameterDirection.Output, null));
            return this;
        }

        public INorm WithOutParameter(string name, object value)
        {
            outputParams.Add((name, ParameterDirection.InputOutput, value));
            return this;
        }

        public object GetOutParameterValue(string name) => outputParamValues[name];

        public void Dispose()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
            Connection?.Dispose();
        }

        private JsonSerializerOptions JsonOptions => 
            this.jsonOptions ?? GlobalJsonSerializerOptions.Options;

        private void SetCommand(DbCommand cmd, string command)
        {
            cmd.SetCommandParameters(command, commandType, commandTimeout);
            outputParamValues.Clear();
            foreach (var (name, direction, value) in outputParams)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = name;
                param.Direction = direction;
                if (dbType == DbType.Ms && (value == null || value.GetType() == StringType))
                {
                    param.DbType = System.Data.DbType.AnsiString;
                    param.Size = int.MaxValue;
                }
                if (direction == ParameterDirection.InputOutput)
                {
                    param.Value = value;
                }
                cmd.Parameters.Add(param);
            }
        }

        private void OnCommandExecuted(DbCommand cmd)
        {
            foreach (var (name, _, _) in outputParams)
            {
                outputParamValues.Add(name, cmd.Parameters[name].Value); 
            }
            outputParams.Clear();
        }

        private bool CheckDbNull<T>() => (!convertsDbNull || typeof(T) == StringType);

        private T GetFieldValue<T>(DbDataReader reader, int ordinal)
        {
            if (CheckDbNull<T>())
            {
                return reader.IsDBNull(ordinal) ? default : reader.GetFieldValue<T>(ordinal);
            }
            return reader.GetFieldValue<T>(ordinal);
        }

        private async ValueTask<T> GetFieldValueAsync<T>(DbDataReader reader, int ordinal)
        {
            if (CheckDbNull<T>())
            {
                return await reader.IsDBNullAsync(ordinal) ? default : await reader.GetFieldValueAsync<T>(ordinal);
            }
            return await reader.GetFieldValueAsync<T>(ordinal);
        }
    }
}
