using System.Text.Json;

namespace Norm
{
    public static class GlobalJsonSerializerOptions
    {
        public static JsonSerializerOptions Options { get; private set; } = null;
        public static void SetGlobalOptions(JsonSerializerOptions options)
        {
            Options = options;
        }
    }
}
