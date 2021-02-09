using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace Teronis.EntityFrameworkCore.Query
{
    public interface IDeterminedIncludableQueryable<out EntityType, out PreviousPropertyType>
        where EntityType : class
    {
        bool Determinator { get; }
        Func<IIncludableQueryable<EntityType, PreviousPropertyType>> IncludedQueryFactory { get; }

        IQueryable<EntityType> DeterminedQuery { get; }
    }
}
