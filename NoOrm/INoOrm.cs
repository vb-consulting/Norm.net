using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public interface INoOrm : INoOrmConnection,
        INoOrmExecute, INoOrmExecuteAsync,
        INoOrmSingle, INoOrmSingleAsync,
        INoOrmRead, INoOrmReadResults, INoOrmReadResultsConditional,
        INoOrmReadResultsAsync, INoOrmReadAsyncResultsAsync,
        INoOrmReadResultsConditionalAsync, INoOrmReadAsyncResultsConditionalAsync
    { }

    public interface INoOrmConnection
    {
        DbConnection Connection { get; }
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
        IDictionary<string, object> Single(string command);
        IDictionary<string, object> Single(string command, params object[] parameters);
        IDictionary<string, object> Single(string command, params (string name, object value)[] parameters);
    }

    public interface INoOrmSingleAsync
    {
        Task<IDictionary<string, object>> SingleAsync(string command);
        Task<IDictionary<string, object>> SingleAsync(string command, params object[] parameters);
        Task<IDictionary<string, object>> SingleAsync(string command, params (string name, object value)[] parameters);
    }

    public interface INoOrmRead
    {
        IEnumerable<IDictionary<string, object>> Read(string command);
        IEnumerable<IDictionary<string, object>> Read(string command, params object[] parameters);
        IEnumerable<IDictionary<string, object>> Read(string command, params (string name, object value)[] parameters);
    }

    public interface INoOrmReadResults
    {
        INoOrm Read(string command, Action<IDictionary<string, object>> results);
        INoOrm Read(string command, Action<IDictionary<string, object>> results, params object[] parameters);
        INoOrm Read(string command, Action<IDictionary<string, object>> results, params (string name, object value)[] parameters);
    }

    public interface INoOrmReadResultsConditional
    {
        INoOrm Read(string command, Func<IDictionary<string, object>, bool> results);
        INoOrm Read(string command, Func<IDictionary<string, object>, bool> results, params object[] parameters);
        INoOrm Read(string command, Func<IDictionary<string, object>, bool> results, params (string name, object value)[] parameters);
    }

    public interface INoOrmReadResultsAsync
    {
        Task<INoOrm> ReadAsync(string command, Action<IDictionary<string, object>> results);
        Task<INoOrm> ReadAsync(string command, Action<IDictionary<string, object>> results, params object[] parameters);
        Task<INoOrm> ReadAsync(string command, Action<IDictionary<string, object>> results, params (string name, object value)[] parameters);
    }

    public interface INoOrmReadAsyncResultsAsync
    {
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, Task> results);
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, Task> results, params object[] parameters);
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, Task> results, params (string name, object value)[] parameters);
    }


    public interface INoOrmReadResultsConditionalAsync
    {
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, bool> results);
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, bool> results, params object[] parameters);
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, bool> results, params (string name, object value)[] parameters);

    }

    public interface INoOrmReadAsyncResultsConditionalAsync
    {
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, Task<bool>> results);
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, Task<bool>> results, params object[] parameters);
        Task<INoOrm> ReadAsync(string command, Func<IDictionary<string, object>, Task<bool>> results, params (string name, object value)[] parameters);
    }
}
