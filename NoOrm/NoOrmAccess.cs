using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NoOrm
{
    public partial class NoOrmAccess : IDisposable, INoOrm
    {
        public DbConnection Connection { get; }

        public NoOrmAccess(DbConnection connection)
        {
            Connection = connection;
        }

        public void Dispose()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
            Connection?.Dispose();
        }

        private void EnsureConnectionIsOpen()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        private async Task EnsureConnectionIsOpenAsync()
        {
            if (Connection.State != ConnectionState.Open)
            {
                await Connection.OpenAsync();
            }
        }
    }
}
