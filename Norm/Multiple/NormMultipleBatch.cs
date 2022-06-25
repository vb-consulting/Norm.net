using System;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace Norm
{

    public partial class NormMultipleBatch : IDisposable, IAsyncDisposable
    {
        private readonly Norm norm;
        private object dbBatch = null;
        private DbCommand dbCommand = null;
        private DbDataReader dbReader = null;
        private bool disposed = false;

        internal NormMultipleBatch(Norm norm)
        {
            this.norm = norm;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (dbBatch != null)
                    {
                        ExecuteMethod<object>(dbBatch, "Dispose");
                        //dbBatch.Dispose();
                    }
                    if (dbCommand != null)
                    {
                        dbCommand.Dispose();
                    }
                    if (dbReader != null)
                    {
                        dbReader.Dispose();
                    }
                }
                disposed = true;
            }
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (dbBatch != null)
                    {
                        await ExecuteMethod<ValueTask>(dbBatch, "DisposeAsync");
                        //await dbBatch.DisposeAsync();
                    }
                    if (dbCommand != null)
                    {
                        dbCommand.Dispose();
                    }
                    if (dbReader != null)
                    {
                        await dbReader.DisposeAsync();
                    }
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>true if there are more result sets; otherwise, false.</returns>
        public bool Next()
        {
            return dbReader.NextResult();
        }

        /// <summary>
        /// Asynchronously advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>A value task representing the asynchronous operation returning true if there are more result sets; otherwise, false.</returns>
        public async ValueTask<bool> NextAsync()
        {
            if (norm.CancellationToken.HasValue)
            {
                return await dbReader.NextResultAsync(norm.CancellationToken.Value);
            }
            return await dbReader.NextResultAsync();
        }

        internal NormMultipleBatch Init(string command, FormattableString formattable)
        {
            /*
            if (NormOptions.Value.UseBatchIfAvailable)
            {
                dbBatch = ExecuteMethod<object>(this.norm.Connection, "CreateBatch");
                var commands = GetProperty<object>(dbBatch, "BatchCommands");
            }
            */
            if (formattable != null)
            {
                dbCommand = norm.CreateCommand(formattable);
            }
            else
            {
                dbCommand = norm.CreateCommand(command);
            }
            dbReader = norm.ExecuteReader(dbCommand);
            return this;
        }

        internal async ValueTask<NormMultipleBatch> InitAsync(string command, FormattableString formattable)
        {
            if (formattable != null)
            {
                dbCommand = await norm.CreateCommandAsync(formattable);
            }
            else
            {
                dbCommand = await norm.CreateCommandAsync(command);
            }
            dbReader = await norm.ExecuteReaderAsync(dbCommand);
            return this;
        }

        private T ExecuteMethod<T>(object instance, string name, params object[] paramaters)
        {
            var method = instance.GetType().GetMethod(name);
            if (method != null)
            {
                return (T)method.Invoke(instance, paramaters);
            }
            return default;
        }

        private T GetProperty<T>(object instance, string name)
            where T : class
        {
            var prop = instance.GetType().GetProperty(name, BindingFlags.Public);
            if (prop != null)
            {
                var value = prop.GetValue(instance);
                return value as T;
            }
            return default;
        }
    }
}
