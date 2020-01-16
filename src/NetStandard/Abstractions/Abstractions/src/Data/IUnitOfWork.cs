using System;
using System.Threading.Tasks;

namespace Teronis.Data
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsDisposed { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void RejectChanges();
    }
}
