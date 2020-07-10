using System;
using Microsoft.EntityFrameworkCore;
using Teronis.Data;
using Teronis.DependencyInjection;

namespace Teronis.EntityFrameworkCore.Data
{
    public class DbRepositoryUnitOfWork : DbUnitOfWork, IDbRepositoryUnitOfWork
    {
        private readonly IRepositoryResolver repositoryResolver;

        public DbRepositoryUnitOfWork(DbContext dbContext, IRepositoryResolver repositoryResolver) : base(dbContext)
            => this.repositoryResolver = repositoryResolver ?? throw new ArgumentNullException(nameof(repositoryResolver));

        public virtual RepositoryType GetRepository<EntityType, RepositoryType>() where RepositoryType : IRepository<EntityType>
            => repositoryResolver.GetRepository<EntityType, RepositoryType>();
    }
}
