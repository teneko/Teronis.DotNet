using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Teronis.Data;

namespace Teronis.EntityFrameworkCore.Data
{
    public class DbRepository<EntityType> : IRepository<EntityType>
        where EntityType : class
    {
        public bool IsDisposed { get; private set; }

        protected DbContext DbContext { get; }
        protected DbSet<EntityType> DbSet { get; }

        public DbRepository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<EntityType>();
        }

        private IQueryable<EntityType> queryAsNoTracking()
            => DbSet.AsNoTracking();

        public virtual EntityType Find(params object[] keyValues)
        {
            var entity = DbSet.Find(keyValues);
            //var entry = DbContext.Entry(entity);

            //if (entry.State == EntityState.Modified)
            //    entry.Reload();

            return entity;
        }

        public virtual IEnumerable<EntityType> All()
            => DbSet;

        public virtual IAsyncEnumerable<EntityType> AllAsync()
            => DbSet;

        public virtual IEnumerable<EntityType> All(Expression<Func<EntityType, bool>> predicate)
            => queryAsNoTracking().Where(predicate);

        public virtual IAsyncEnumerable<EntityType> AllAsync(Expression<Func<EntityType, bool>> predicate)
            => queryAsNoTracking().Where(predicate).AsAsyncEnumerable();

        public virtual IEnumerable<EntityType> OrderBy<RelatedType>(Expression<Func<EntityType, RelatedType>> keySelector)
            => queryAsNoTracking().OrderBy(keySelector);

        public virtual IAsyncEnumerable<EntityType> OrderByAsync<RelatedType>(Expression<Func<EntityType, RelatedType>> keySelector)
            => queryAsNoTracking().OrderBy(keySelector).AsAsyncEnumerable();

        public virtual void Add(EntityType entity)
            => DbSet.Add(entity);

        public virtual void AddRange(params EntityType[] entities)
            => DbSet.AddRange(entities);

        private void attachEntity(EntityType entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached) {
                DbSet.Attach(entity);
            }
        }

        public virtual void Remove(EntityType entity)
        {
            attachEntity(entity);
            DbSet.Remove(entity);
        }

        public virtual void RemoveRange(params EntityType[] entities)
        {
            foreach (var entity in entities) {
                attachEntity(entity);
            }

            DbSet.RemoveRange(entities);
        }
    }
}
