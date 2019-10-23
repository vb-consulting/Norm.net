using System;
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
        IList<(string name, object value)> Single(string command);
        IList<(string name, object value)> Single(string command, params object[] parameters);
        IList<(string name, object value)> Single(string command, params (string name, object value)[] parameters);
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
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command);
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command);
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
    }

    public interface INormSingleAsync
    {
        ValueTask<IList<(string name, object value)>> SingleAsync(string command);
        ValueTask<IList<(string name, object value)>> SingleAsync(string command, params object[] parameters);
        ValueTask<IList<(string name, object value)>> SingleAsync(string command, params (string name, object value)[] parameters);
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

        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command);
        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
    }

    public interface INormRead
    {
        IEnumerable<IList<(string name, object value)>> Read(string command);
        IEnumerable<IList<(string name, object value)>> Read(string command, params object[] parameters);
        IEnumerable<IList<(string name, object value)>> Read(string command, params (string name, object value)[] parameters);
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
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command);
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
    }

    public interface INormReadAsync
    {
        IAsyncEnumerable<IList<(string name, object value)>> ReadAsync(string command);
        IAsyncEnumerable<IList<(string name, object value)>> ReadAsync(string command, params object[] parameters);
        IAsyncEnumerable<IList<(string name, object value)>> ReadAsync(string command, params (string name, object value)[] parameters);
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
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> ReadAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ReadAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value)[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params object[] parameters);
        IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value)[] parameters);
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
