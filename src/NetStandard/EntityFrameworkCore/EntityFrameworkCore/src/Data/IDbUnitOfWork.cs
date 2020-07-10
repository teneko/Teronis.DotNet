using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Teronis.Data;

namespace Teronis.EntityFrameworkCore.Data
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
