using Autofac;
using Teronis.Data;
using Teronis.DependencyInjection;

namespace Teronis.EntityFrameworkCore
{
    public class RepositoryResolver : IRepositoryResolver
    {
        private readonly ILifetimeScope lifetimeScope;

        public RepositoryResolver(ILifetimeScope lifetimeScope)
            => this.lifetimeScope = lifetimeScope;

        public RepositoryType GetRepository<EntityType, RepositoryType>() where RepositoryType : IRepository<EntityType>
             => lifetimeScope.Resolve<RepositoryType>();
    }
}
