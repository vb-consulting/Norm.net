using System;
using System.Data.Common;
using System.Reflection;

namespace Norm
{
    public class NormOptions
    {
        /// <summary>
        /// Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error
        /// </summary>
        public int? CommandTimeout { get; set; } = null;
        /// <summary>
        /// Callback that, if set will be called before every execution. Useful for lgging commands.
        /// </summary>
        public Action<DbCommand> DbCommandCallback { get; set; } = null;
        /// <summary>
        /// Autmoatic command comment header options.
        /// </summary>
        public CommandCommentHeader CommandCommentHeader { get; private set; } = new CommandCommentHeader();
        /// <summary>
        /// Escape sequence, when using parameters via string interpolation formats, use this escape to skip parametr parsing and use values as is.
        /// For example: command ReadFormat($"select from {"table":raw}"), that "table" will be interpreted as normal interpolation string.
        /// </summary>
        public string RawInterpolationParameterEscape { get; set; } = "raw";
        /// <summary>
        /// Set to true to run all commands in prepared mode every time by calling Prepare() method before execution.
        /// </summary>
        public bool Prepared { get; set; } = false;
        /// <summary>
        /// Norm instance type, used internally for Norm extensions. Must inherit Norm type. Set to null for default behavior.
        /// </summary>
        public Type NormInstanceType { get; set; } = null;

        public static NormOptions Value { get; internal set; } = new NormOptions();

        internal static ConstructorInfo NormCtor = null;

        public static void Configure(Action<NormOptions> options)
        {
            Value = new NormOptions();
            NormCtor = null;
            options(Value);
            AssignExtensionCtor();
        }

        public static void Configure<T>(Action<T> options) where T : NormOptions, new()
        {
            Value = new T();
            NormCtor = null;
            options(Value as T);
            AssignExtensionCtor();
        }

        private static void AssignExtensionCtor()
        {
            if (Value.NormInstanceType != null)
            {
                if (!Value.NormInstanceType.IsSubclassOf(typeof(Norm)))
                {
                    throw new ArgumentException($"Type \"{Value.NormInstanceType.FullName}\" does not inherits type \"{typeof(Norm).FullName} type");
                }
                var ctors = Value.NormInstanceType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                if (ctors.Length == 0)
                {
                    throw new ArgumentException($"Type \"{Value.NormInstanceType.FullName}\" does not have any public constructors available.");
                }
                NormCtor = ctors[0];
            }
        }
    }
}
