using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Norm.Interfaces
{
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
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);

        ValueTask<IList<(string name, object value)>> SingleAsync(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<T> SingleAsync<T>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2)> SingleAsync<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3)> SingleAsync<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4)> SingleAsync<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5)> SingleAsync<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6)> SingleAsync<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7)> SingleAsync<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        ValueTask<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> SingleAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);
    }
}