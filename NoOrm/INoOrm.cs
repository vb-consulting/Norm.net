using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public interface INoOrm :
        INoOrmExecute, 
        INoOrmExecuteAsync,
        INoOrmSingle, 
        INoOrmSingleAsync,
        INoOrmRead,
        INoOrmReadAsync
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

    public interface INoOrmReadAsync
    {
        IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command);
        IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command, params object[] parameters);
        IAsyncEnumerable<IAsyncEnumerable<(string name, object value)>> ReadAsync(string command, params (string name, object value)[] parameters);
    }
}
