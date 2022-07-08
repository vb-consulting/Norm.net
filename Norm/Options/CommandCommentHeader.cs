using System;

namespace Norm
{
    public class CommandCommentHeader
    {
        /// <summary>
        /// Enable automatic comment headers that are added to each query command.
        /// </summary>
        public bool Enabled { get; set; } = false;
        /// <summary>
        /// Include current command attributes like database provider, command type, and command timeout.
        /// </summary>
        public bool IncludeCommandAttributes { get; set; } = true;
        /// <summary>
        /// Include command caller info like calling method member name, source code file path, and source code line number.
        /// </summary>
        public bool IncludeCallerInfo { get; set; } = true;
        /// <summary>
        /// Include command parameters in comment headers.
        /// </summary>
        public bool IncludeParameters { get; set; } = true;
        /// <summary>
        /// Format string for parameters in comment headers. Placeholder 0 is the parameter name, 1 is the parameter type and 2 is the parameter value. Used only when `IncludeParameters` is `true`.
        /// </summary>
        public string ParametersFormat { get; set; } = $"{{0}} {{1}} = {{2}}\n";
        /// <summary>
        /// Include command execution timestamp in comment headers.
        /// </summary>
        public bool IncludeTimestamp { get; set; } = false;
        /// <summary>
        /// Omits comment headers if enabled from a command text when command type is Stored Procedure for the database providers that do not support comments in a Stored Procedure calls (SQL Server and MySQL)
        /// </summary>
        public DatabaseType OmitStoredProcCommandCommentHeaderForDbTypes { get; set; } = DatabaseType.Sql | DatabaseType.MySql;
    }
}
