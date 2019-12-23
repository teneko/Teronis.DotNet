using System;

namespace Teronis
{
    public interface IRepositoryUnitOfWork : IUnitOfWork, IRepositoryResolver
    { }
}
