using System.Data;
using System.Data.Common;
using System.Text.Json;

namespace Norm.Interfaces
{
    public interface INorm :
        INormExecute, 
        INormExecuteAsync,
        INormSingle, 
        INormSingleAsync,
        INormRead,
        INormReadAsync,
        INormSingleJson,
        INormSingleJsonAsync,
        INormJson,
        INormJsonAsync
    {
        DbConnection Connection { get; }
        INorm As(CommandType type);
        INorm AsProcedure();
        INorm AsText();
        INorm Timeout(int? timeout);
        INorm WithJsonOptions(JsonSerializerOptions options);
        INorm Prepared();
    }
}
