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
        /// Command callback that will be called before every execution.
        /// </summary>
        public Action<DbCommand> DbCommandCallback { get; set; } = null;
        /// <summary>
        /// Automatic command comment header options.
        /// </summary>
        public CommandCommentHeader CommandCommentHeader { get; private set; } = new CommandCommentHeader();
        /// <summary>
        /// Escape sequence, when using parameters via string interpolation formats, use this escape to skip parameter parsing and use values as is.
        /// For example: command ReadFormat($"select from {"table":raw}"), that "table" will be interpreted as normal interpolation string.
        /// </summary>
        public string RawInterpolationParameterEscape { get; set; } = "raw";
        /// <summary>
        /// Set to true to run all commands in prepared mode every time by calling `Prepare()` method before execution.
        /// </summary>
        public bool Prepared { get; set; } = false;
        /// <summary>
        /// Set to true to map instances properties that have private or protected setters.
        /// </summary>
        public bool MapPrivateSetters { get; set; } = false;
        /// <summary>
        /// Enables SQL rewriting for Npgsql driver by setting global Npgsql.EnableSqlRewriting switch
        /// Only for Npgsql version 6 and above.
        /// Default is null, Npgsql driver default.
        /// When SQL rewriting is disabled, only positional parameters can be used ($1, $2, 3, etc), but it has performance benefits.
        /// see https://github.com/npgsql/npgsql/blob/main/src/Npgsql/NpgsqlCommand.cs#L94
        /// This option only have impact before any of the commands are executed.
        /// </summary>
        public bool? NpgsqlEnableSqlRewriting { get; set; } = null;
        /// <summary>
        /// Automatically sets mapped instances to null if all properties are null.
        /// </summary>
        public bool NullableInstances { get; set; } = false;
        /// <summary>
        /// By default all database field names are stripped of underscores and @ at characters.
        /// This is done to make it easier to map to C# properties (field_name to FieldName).
        /// Set this option to true to skip this behavior and keep original names.
        /// </summary>
        public bool KeepOriginalNames { get; set; } = false;

        /// <summary>
        /// Norm instance type, used internally for Norm extensions. Must inherit Norm type. Set to null for default behavior.
        /// </summary>
        protected virtual Type NormInstanceType { get; set; } = null;

        protected virtual void OnConfigured()
        {
        }

        public static NormOptions Value { get; internal set; } = new NormOptions();

        internal static ConstructorInfo NormCtor = null;

        /// <summary>
        /// Configuration of global settings
        /// </summary>
        /// <param name="options"></param>
        public static void Configure(Action<NormOptions> options)
        {
            Value = new NormOptions();
            NormCtor = null;
            options(Value);
            if (Value.NpgsqlEnableSqlRewriting.HasValue)
            {
                AppContext.SetSwitch("Npgsql.EnableSqlRewriting", Value.NpgsqlEnableSqlRewriting.Value);
            }
            Value.OnConfigured();
        }

        /// <summary>
        /// Configuration of global settings for extensions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        public static void Configure<T>(Action<T> options) where T : NormOptions, new()
        {
            Value = new T();
            NormCtor = null;
            options(Value as T);
            AssignExtensionCtor();
            if (Value.NpgsqlEnableSqlRewriting.HasValue)
            {
                AppContext.SetSwitch("Npgsql.EnableSqlRewriting", Value.NpgsqlEnableSqlRewriting.Value);
            }
            Value.OnConfigured();
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
