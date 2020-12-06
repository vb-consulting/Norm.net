using System.Collections.Generic;
using System.Data;

namespace Norm.Interfaces
{
    public interface INormRead
    {
        IEnumerable<(string name, object value)[]> Read(string command);
        IEnumerable<(string name, object value)[]> Read(string command, params object[] parameters);
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value)[] parameters);
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(string name, object value)[]> Read(string command, params (string name, object value, object type)[] parameters);

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
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params object[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value)[] parameters);
        
        IEnumerable<T> Read<T>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, DbType type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, DbType type)[] parameters);

        IEnumerable<T> Read<T>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command, params (string name, object value, object type)[] parameters);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command, params (string name, object value, object type)[] parameters);
    }
}