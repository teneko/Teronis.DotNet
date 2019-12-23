using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Teronis.EntityFrameworkCore
{
    public interface IDbUnitOfWork : IUnitOfWork
    {
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess);
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
        void AcceptAllChanges();
    }
}
