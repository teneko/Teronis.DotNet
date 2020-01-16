using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Teronis.EntityFrameworkCore.Data
{
    public class DbUnitOfWork : IDbUnitOfWork
    {
        public bool IsDisposed { get; private set; }

        protected DbContext DbContext { get; }

        public DbUnitOfWork(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual int SaveChanges()
            => DbContext.SaveChanges();

        public virtual int SaveChanges(bool acceptAllChangesOnSuccess)
            => DbContext.SaveChanges(acceptAllChangesOnSuccess);

        public virtual Task<int> SaveChangesAsync()
            => DbContext.SaveChangesAsync();

        public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess)
            => DbContext.SaveChangesAsync(acceptAllChangesOnSuccess);

        public virtual void RejectChanges()
        {
            // TODO: implement here
            throw new NotImplementedException();
        }

        public IDbContextTransaction BeginTransaction()
            => DbContext.Database.BeginTransaction();

        public Task<IDbContextTransaction> BeginTransactionAsync()
            => DbContext.Database.BeginTransactionAsync();

        public void AcceptAllChanges()
            => DbContext.ChangeTracker.AcceptAllChanges();

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed) {
                if (disposing)
                    DbContext.Dispose();

                IsDisposed = true;
            }
        }

        public void Dispose()
            => Dispose(true);

        #endregion
    }
}
