using Microsoft.EntityFrameworkCore;
using System;

namespace Teronis.EntityFrameworkCore
{
    public class DbRepositoryUnitOfWork : DbUnitOfWork, IDbRepositoryUnitOfWork
    {
        private IRepositoryResolver repositoryResolver;

        public DbRepositoryUnitOfWork(DbContext dbContext, IRepositoryResolver repositoryResolver) : base(dbContext)
            => this.repositoryResolver = repositoryResolver ?? throw new ArgumentNullException(nameof(repositoryResolver));

        public virtual RepositoryType GetRepository<EntityType, RepositoryType>() where RepositoryType : IRepository<EntityType>
            => repositoryResolver.GetRepository<EntityType, RepositoryType>();
    }
}