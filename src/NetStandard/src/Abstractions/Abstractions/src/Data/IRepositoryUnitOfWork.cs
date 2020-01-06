using Teronis.DependencyInjection;

namespace Teronis.Data
{
    public interface IRepositoryUnitOfWork : IUnitOfWork, IRepositoryResolver
    { }
}
