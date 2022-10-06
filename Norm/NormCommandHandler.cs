using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        protected virtual void ApplyOptions(DbCommand cmd)
        {
            if (NormOptions.Value.CommandTimeout.HasValue)
            {
                cmd.CommandTimeout = NormOptions.Value.CommandTimeout.Value;
            }
            if (commandTimeout.HasValue)
            {
                cmd.CommandTimeout = commandTimeout.Value;
            }

            ApllyCommentHeader(cmd);

            NormOptions.Value.DbCommandCallback?.Invoke(cmd);
            dbCommandCallback?.Invoke(cmd);

            if (cmd.CommandType == CommandType.StoredProcedure
                && ((this.dbType | NormOptions.Value.CommandCommentHeader.OmitStoredProcCommandCommentHeaderForDbTypes) == NormOptions.Value.CommandCommentHeader.OmitStoredProcCommandCommentHeaderForDbTypes))            {
                if (this.commentHeader != null)
                {
                    cmd.CommandText = cmd.CommandText.Replace(this.commentHeader, "");
                }
            }
        }

        protected virtual void ApllyCommentHeader(DbCommand cmd)
        {
            commandText = cmd.CommandText;
            commentHeader = null;

            if (!NormOptions.Value.CommandCommentHeader.Enabled && !this.commandCommentHeaderEnabled)
            {
                return;
            }
            
            var sb = new StringBuilder();

            if (this.commandCommentHeaderEnabled && this.comment != null)
            {
                sb.Append($"{this.comment}\n");
            }

            if ((NormOptions.Value.CommandCommentHeader.Enabled && NormOptions.Value.CommandCommentHeader.IncludeCommandAttributes) ||
                (this.commandCommentHeaderEnabled && this.includeCommandAttributes))
            {
                sb.Append($"{(this.dbType == DatabaseType.Other ? "" : $"{this.dbType} ")}{cmd.CommandType} Command. Timeout: {cmd.CommandTimeout} seconds.\n");
            }

            if ((NormOptions.Value.CommandCommentHeader.Enabled && NormOptions.Value.CommandCommentHeader.IncludeCallerInfo) ||
                (this.commandCommentHeaderEnabled && this.includeCallerInfo))
            {
                sb.Append($"at {memberName} in {sourceFilePath}#{sourceLineNumber}\n");
            }

            if ((NormOptions.Value.CommandCommentHeader.Enabled && NormOptions.Value.CommandCommentHeader.IncludeTimestamp) ||
                (this.commandCommentHeaderEnabled && this.includeTimestamp))
            {
                sb.Append($"Timestamp: {DateTime.Now:o}\n");
            }

            if ((NormOptions.Value.CommandCommentHeader.Enabled && NormOptions.Value.CommandCommentHeader.IncludeParameters) ||
                (this.commandCommentHeaderEnabled && this.includeParameters))
            {
                int paramIndex = 0;
                foreach (DbParameter p in cmd.Parameters)
                {
                    paramIndex++;
                    string paramType;
                    if (this.dbType == DatabaseType.Other)
                    {
                        paramType = p.DbType.ToString().ToLowerInvariant();
                    }
                    else
                    {
                        var prop = p.GetType().GetProperty($"{this.dbType}DbType");
                        if (prop != null)
                        {
                            paramType = prop.GetValue(p).ToString().ToLowerInvariant();
                        }
                        else
                        {
                            paramType = this.dbType.ToString().ToLowerInvariant();
                        }
                        if (int.TryParse(paramType, out _))
                        {
                            paramType = "";
                        }
                    }
                    object value = p.Value is DateTime time ? time.ToString("o") : p.Value;
                    if (value is string)
                    {
                        value = $"\"{value}\"".Replace("/*", "??").Replace("*/", "??");
                    }
                    else if (value is bool)
                    {
                        value = value.ToString().ToLowerInvariant();
                    }
                    else if (value is System.Collections.ICollection)
                    {
                        var array = new List<string>();
                        var enumerator = (value as System.Collections.IEnumerable).GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            array.Add(enumerator.Current.ToString());
                        }
                        value = $"{{{string.Join(", ", array)}}}";
                    }
                    var name = string.IsNullOrEmpty(p.ParameterName) ? $"${paramIndex}" : $"@{p.ParameterName}";
                    sb.Append(string.Format(NormOptions.Value.CommandCommentHeader.ParametersFormat, name, paramType, value));
                }
            }

            if (sb.Length > 0)
            {
                commandText = cmd.CommandText;
                commentHeader = $"/*\n{sb}*/\n";
                cmd.CommandText = string.Concat(commentHeader, commandText);
            }
        }

        protected void EnsureIsOpen(DbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        protected async Task EnsureIsOpenAsync(DbConnection connection, CancellationToken? cancellationToken = null)
        {
            if (connection.State == ConnectionState.Open)
            {
                return;
            }
            cancellationToken?.ThrowIfCancellationRequested();
            if (cancellationToken.HasValue)
            {
                await connection.OpenAsync(cancellationToken.Value);
            }
            else
            {
                await connection.OpenAsync();
            }
            return;
        }

        protected void MergeParameters(object[] parameters)
        {
            if (parameters.Length == 1 && parameters[0].GetType().IsArray)
            {
                if (parameters[0] is DbParameter[] p)
                {
                    parameters = p;
                }
            }
            if (this.parameters == null)
            {
                this.parameters = parameters;
            }
            else
            {
                this.parameters = this.parameters.Union(parameters).ToArray();
            }
        }

        protected void ApplyUnknownResultTypes(DbCommand cmd)
        {
            if (allResultTypesAreUnknown)
            {
                var prop = cmd.GetType().GetProperties().First(p => string.Equals(p.Name, "AllResultTypesAreUnknown", StringComparison.InvariantCulture));
                prop.SetValue(cmd, true);
            }
            if (unknownResultTypeList != null)
            {
                var prop = cmd.GetType().GetProperties().First(p => string.Equals(p.Name, "UnknownResultTypeList", StringComparison.InvariantCulture));
                prop.SetValue(cmd, unknownResultTypeList);
            }
        }
    }
}
