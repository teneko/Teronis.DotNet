using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace Teronis.EntityFrameworkCore.Query
{
    internal readonly struct DeterminedIncludableQuery<EntityType, PreviousPropertyType> : IDeterminedIncludableQueryable<EntityType, PreviousPropertyType>
        where EntityType : class
    {
        public readonly Func<IIncludableQueryable<EntityType, PreviousPropertyType>> IncludedQueryFactory { get; }
        public readonly bool Determinator { get; }

        public readonly IQueryable<EntityType> DeterminedQuery {
            get {
                if (Determinator) {
                    return IncludedQueryFactory();
                }

                return query;
            }
        }

        private readonly IQueryable<EntityType> query;

        public DeterminedIncludableQuery(IQueryable<EntityType> query, Func<IIncludableQueryable<EntityType, PreviousPropertyType>> includedQueryFactory, bool determinator)
        {
            this.query = query;
            IncludedQueryFactory = includedQueryFactory;
            Determinator = determinator;
        }
    }
}
