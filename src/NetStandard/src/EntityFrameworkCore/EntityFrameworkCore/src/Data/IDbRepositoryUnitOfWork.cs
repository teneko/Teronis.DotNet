using Teronis.Data;

namespace Teronis.EntityFrameworkCore.Data
{
    public interface IDbRepositoryUnitOfWork : IRepositoryUnitOfWork, IDbUnitOfWork
    { }
}
