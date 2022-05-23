using System;

namespace Norm
{
    public class CommandCommentHeader
    {
        /// <summary>
        /// Enable automatic comment headers that are added on each query command.
        /// </summary>
        public bool Enabled { get; set; } = false;
        /// <summary>
        /// Include current command atributes like command type and command timeout.
        /// </summary>
        public bool IncludeCommandAttributes { get; set; } = true;
        /// <summary>
        /// Include command parameters in comment headers.
        /// </summary>
        public bool IncludeParameters { get; set; } = true;
        /// <summary>
        /// Format string for parameters in comment headers. Placeholder 0 is name, 1 is parameter type and 2 is value. Used only when IncludeParameters is true.
        /// </summary>
        public string ParametersFormat { get; set; } = $"-- @{{0}} {{1}} = {{2}}{Environment.NewLine}";
        /// <summary>
        /// Include current timestamp  in comment headers.
        /// </summary>
        public bool IncludeTimestamp { get; set; } = false;
    }
}
