using System;

namespace Teronis.EntityFrameworkCore
{
    public interface IDbRepositoryUnitOfWork : IRepositoryUnitOfWork, IDbUnitOfWork
    { }
}
