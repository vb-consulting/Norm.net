using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
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
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);

        IList<(string name, object value)> Single(string command, params (string name, object value, DbType type)[] parameters);
        T Single<T>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2) Single<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3) Single<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);

        IList<(string name, object value)> Single(string command, params (string name, object value, object type)[] parameters);
        T Single<T>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2) Single<T1, T2>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3) Single<T1, T2, T3>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4) Single<T1, T2, T3, T4>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4, T5) Single<T1, T2, T3, T4, T5>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4, T5, T6) Single<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7) Single<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8) Single<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, object type)[] parameters);
        (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) Single<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, object type)[] parameters);
    }
}