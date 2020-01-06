using Teronis.Data;

namespace Teronis.EntityFrameworkCore
{
    public interface IDbRepositoryUnitOfWork : IRepositoryUnitOfWork, IDbUnitOfWork
    { }
}
