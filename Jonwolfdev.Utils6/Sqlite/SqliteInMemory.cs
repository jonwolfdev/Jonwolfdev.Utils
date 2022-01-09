using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonwolfdev.Utils6.Sqlite
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqliteInMemory<T> : IDisposable where T : DbContext
    {
        static DbConnection CreateInMemoryDatabase()
        {
            var conn = new SqliteConnection("Filename=:memory:");
            conn.Open();
            return conn;
        }

        readonly DbConnection _conn;
        public readonly DbContextOptions<T> Options;

        public SqliteInMemory()
        {
            _conn = CreateInMemoryDatabase();
            Options = new DbContextOptionsBuilder<T>().UseSqlite(_conn).Options;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _conn?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
