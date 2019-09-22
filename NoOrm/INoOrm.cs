using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace NoOrm
{
    public delegate void RowCallback(object[] rows);
    //public delegate bool RowCallback<T1>(T1 value1);
    //public delegate bool RowCallback<T1, T2>(T1 value1, T2 value2);
    //public delegate bool RowCallback<T1, T2, T3>(T1 value1, T2 value2, T3 value3);
    public delegate Task RowCallbackAsync(params object[] rows);

    public interface INoOrm :
        INoOrmExecute, 
        INoOrmExecuteAsync,
        INoOrmSingle, 
        INoOrmSingleAsync,
        INoOrmRead, 
        INoOrmReadRows,
        INoOrmReadRowsAsync
    {
        DbConnection Connection { get; }
        INoOrm As(CommandType type);
        INoOrm Timeout(int? timeout);
    }
    
    public interface INoOrmExecute
    {
        INoOrm Execute(string command);
        INoOrm Execute(string command, params object[] parameters);
        INoOrm Execute(string command, params (string name, object value)[] parameters);
    }

    public interface INoOrmExecuteAsync
    {
        Task<INoOrm> ExecuteAsync(string command);
        Task<INoOrm> ExecuteAsync(string command, params object[] parameters);
        Task<INoOrm> ExecuteAsync(string command, params (string name, object value)[] parameters);
    }

    public interface INoOrmSingle
    {
        IEnumerable<(string name, object value)> Single(string command);
        IEnumerable<(string name, object value)> Single(string command, params object[] parameters);
        IEnumerable<(string name, object value)> Single(string command, params (string name, object value)[] parameters);
    }

    public interface INoOrmSingleAsync
    {
        Task<IEnumerable<(string name, object value)>> SingleAsync(string command);
        Task<IEnumerable<(string name, object value)>> SingleAsync(string command, params object[] parameters);
        Task<IEnumerable<(string name, object value)>> SingleAsync(string command, params (string name, object value)[] parameters);
    }

    public interface INoOrmRead
    {
        IEnumerable<IEnumerable<(string name, object value)>> Read(string command);
        IEnumerable<IEnumerable<(string name, object value)>> Read(string command, params object[] parameters);
        IEnumerable<IEnumerable<(string name, object value)>> Read(string command, params (string name, object value)[] parameters);
    }

    public interface INoOrmReadRows
    {
        INoOrm Read(string command, RowCallback results);
        INoOrm Read(string command, RowCallback results, params object[] parameters);
        INoOrm Read(string command, RowCallback results, params (string name, object value)[] parameters);
    }

    public interface INoOrmReadRowsAsync
    {
        Task<INoOrm> ReadAsync(string command, RowCallback results);
        Task<INoOrm> ReadAsync(string command, RowCallback results, params object[] parameters);
        Task<INoOrm> ReadAsync(string command, RowCallback results, params (string name, object value)[] parameters);
        Task<INoOrm> ReadAsync(string command, RowCallbackAsync results);
        Task<INoOrm> ReadAsync(string command, RowCallbackAsync results, params object[] parameters);
        Task<INoOrm> ReadAsync(string command, RowCallbackAsync results, params (string name, object value)[] parameters);
    }
}
