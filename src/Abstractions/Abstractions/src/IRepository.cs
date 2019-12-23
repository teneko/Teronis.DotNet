using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis
{
    public interface IRepository<EntityType>
    {
        EntityType Find(params object[] keyValues);
        IEnumerable<EntityType> All();
        IAsyncEnumerable<EntityType> AllAsync();
        IEnumerable<EntityType> All(Expression<Func<EntityType, bool>> predicate);
        IAsyncEnumerable<EntityType> AllAsync(Expression<Func<EntityType, bool>> predicate);
        IEnumerable<EntityType> OrderBy<RelatedType>(Expression<Func<EntityType, RelatedType>> keySelector);
        IAsyncEnumerable<EntityType> OrderByAsync<RelatedType>(Expression<Func<EntityType, RelatedType>> keySelector);
        void Add(EntityType entity);
        void AddRange(params EntityType[] entities);
        void Remove(EntityType entity);
        void RemoveRange(params EntityType[] entities);
    }
}
