using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using System.Threading.Tasks;

namespace Norm
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
        INorm WithOutParameter(string name);
        INorm WithOutParameter(string name, object value);
        object GetOutParameterValue(string name);
    }

    public interface INormExecute
    {
        INorm Execute(string command);
        INorm Execute(string command, params object[] parameters);
        INorm Execute(string command, params (string name, object value)[] parameters);
    }

    public interface INormExecuteAsync
    {
        ValueTask<INorm> ExecuteAsync(string command);
        ValueTask<INorm> ExecuteAsync(string command, params object[] parameters);
        ValueTask<INorm> ExecuteAsync(string command, params (string name, object value)[] parameters);
    }

    public interface INormSingle
    {
        IEnumerable<(string name, object value)> Single(string command);
        IEnumerable<(string name, object value)> Single(string command, params object[] parameters);
        IEnumerable<(string name, object value)> Single(string command, params (string name, object value)[] parameters);
        T Single<T>(string command);
        T Single<T>(string command, params object[] parameters);
        T Single<T>(string command, params (string name, object value)[] parameters);
        (T1, T2) Single<T1, T2>(string command);
        (T1, T2) Single<T1, T2>(string command, params object[] parameters);
        (T1, T2) Single<T1, T2>(string command, params (string name, object value)[] parameters);
        (T1, T2, T3) Single<T1, T2, T3>(string command);
        (T1, T2, T3) Single<T1, T2, T3>(string command, params object[] parameters);
        (T1, T2, T3) Single<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command);
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params object[] parameters);
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command);
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);
    }

    public interface INormSingleAsync
    {
        IAsyncEnumerable<(string name, object value)> SingleAsync(string command);
        IAsyncEnumerable<(string name, object value)> SingleAsync(string command, params object[] parameters);
        IAsyncEnumerable<(string name, object value)> SingleAsync(string command, params (string name, object value)[] parameters);
        ValueTask<T> SingleAsync<T>(string command);
        ValueTask<T> SingleAsync<T>(string command, params object[] parameters);
        ValueTask<T> SingleAsync<T>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command);
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params object[] parameters);
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command);
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command);
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command);
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);
    }

    public interface INormRead
    {
        IEnumerable<IEnumerable<(string name, object value)>> Read(string command);
        IEnumerable<IEnumerable<(string name, object value)>> Read(string command, params object[] parameters);
        IEnumerable<IEnumerable<(string name, object value)>> Read(string command, params (string name, object value)[] parameters);
        IEnumerable<T> Read<T>(string command);
        IEnumerable<T> Read<T>(string command, params object[] parameters);
        IEnumerable<T> Read<T>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2)> Read<T1, T2>(string command);
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters);
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command);
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command);
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command);
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);

    }

    public interface INormReadAsync
    {
        IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command);
        IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command, params object[] parameters);
        IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<T> ReadAsync<T>(string command);
        IAsyncEnumerable<T> ReadAsync<T>(string command, params object[] parameters);
        IAsyncEnumerable<T> ReadAsync<T>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command);
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command);
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2, T3)> ReadAsync<T1, T2, T3>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command);
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4)> ReadAsync<T1, T2, T3, T4>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command);
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5)> ReadAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value)[] parameters);

    }

    public interface INormSingleJson
    {
        T SingleJson<T>(string command);
        T SingleJson<T>(string command, params object[] parameters);
        T SingleJson<T>(string command, params (string name, object value)[] parameters);
    }

    public interface INormSingleJsonAsync
    {
        ValueTask<T> SingleJsonAsync<T>(string command);
        ValueTask<T> SingleJsonAsync<T>(string command, params object[] parameters);
        ValueTask<T> SingleJsonAsync<T>(string command, params (string name, object value)[] parameters);
    }

    public interface INormJson
    {
        IEnumerable<T> Json<T>(string command);
        IEnumerable<T> Json<T>(string command, params object[] parameters);
        IEnumerable<T> Json<T>(string command, params (string name, object value)[] parameters);
    }

    public interface INormJsonAsync
    {
        IAsyncEnumerable<T> JsonAsync<T>(string command);
        IAsyncEnumerable<T> JsonAsync<T>(string command, params object[] parameters);
        IAsyncEnumerable<T> JsonAsync<T>(string command, params (string name, object value)[] parameters);
    }
}
