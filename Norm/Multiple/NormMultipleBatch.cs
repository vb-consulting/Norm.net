using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Norm
{

    public partial class NormMultipleBatch : IDisposable, IAsyncDisposable
    {
        private readonly Norm norm;
        private DbCommand dbCommand = null;
        private DbDataReader dbReader = null;
        private bool disposed = false;

        internal NormMultipleBatch(Norm norm)
        {
            this.norm = norm;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbCommand?.Dispose();
                dbCommand = null;
                dbReader?.Dispose();
                dbReader = null;
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (dbCommand != null)
            {
                await dbCommand.DisposeAsync().ConfigureAwait(false);
                dbCommand = null;
            }

            if (dbReader != null)
            {
                await dbReader.DisposeAsync().ConfigureAwait(false);
                dbReader = null;
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
    }
}
