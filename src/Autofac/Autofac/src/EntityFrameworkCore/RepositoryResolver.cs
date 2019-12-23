using Autofac;

namespace Teronis.EntityFrameworkCore
{
    public class RepositoryResolver : IRepositoryResolver
    {
        private ILifetimeScope lifetimeScope;

        public RepositoryResolver(ILifetimeScope lifetimeScope)
            => this.lifetimeScope = lifetimeScope;

        public RepositoryType GetRepository<EntityType, RepositoryType>() where RepositoryType : IRepository<EntityType>
             => lifetimeScope.Resolve<RepositoryType>();
    }
}
