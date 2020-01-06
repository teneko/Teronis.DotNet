using Teronis.Data;

namespace Teronis.DependencyInjection
{
    public interface IRepositoryResolver
    {
        RepositoryType GetRepository<EntityType, RepositoryType>() where RepositoryType : IRepository<EntityType>;
    }
}
