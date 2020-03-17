using System.Data;
using System.Data.Common;

namespace Norm.Interfaces
{
    public interface INorm :
        INormExecute, 
        INormExecuteAsync,
        INormSingle, 
        INormSingleAsync,
        INormRead,
        INormReadAsync
    {
        DbConnection Connection { get; }
        INorm As(CommandType type);
        INorm AsProcedure();
        INorm AsText();
        INorm Timeout(int? timeout);
        INorm Prepared();
        INorm UsingPostgresFormatParamsMode();
    }
}
