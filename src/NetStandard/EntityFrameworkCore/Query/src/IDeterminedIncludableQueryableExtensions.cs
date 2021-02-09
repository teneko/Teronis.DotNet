using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Teronis.EntityFrameworkCore.Query;

namespace Teronis.EntityFrameworkCore.Query
{
    public static class IDeterminedIncludableQueryableExtensions
    {
        /// <summary>
        /// Specifies related entities to include in the query results. The navigation 
        /// property to be included is specified starting with the type of entity being
        /// queried (<typeparamref name="EntityType"/>). If you wish to include additional
        /// types based on the navigation properties of the type being included, then chain
        /// a call to <see cref="ThenIncludeIf{EntityType, PreviousPropertyType, PropertyType}(IDeterminedIncludableQueryable{EntityType, IEnumerable{PreviousPropertyType}}, Expression{Func{PreviousPropertyType, PropertyType}}, bool)"/>
        /// after this call.
        /// </summary>
        /// <typeparam name="EntityType">The type of entity being queried.</typeparam>
        /// <typeparam name="PropertyType">The type of related entity to be included.</typeparam>
        /// <param name="query">The source query.</param>
        /// <param name="includePath">A lambda expression representing the navigation property to be included</param>
        /// <param name="determinator">A boolean that determines the related entity being included or not.</param>
        /// <returns>A new includable query with related entity to be included if <paramref name="determinator"/> is true.</returns>
        /// <exception cref="ArgumentNullException">The parameter <paramref name="query"/> is null.</exception>
        public static IDeterminedIncludableQueryable<EntityType, PropertyType> IncludeIf<EntityType, PropertyType>(
            this IQueryable<EntityType> query, Expression<Func<EntityType, PropertyType>> includePath, bool determinator)
            where EntityType : class
        {
            query = query ?? throw new ArgumentNullException(nameof(query));

            IIncludableQueryable<EntityType, PropertyType> getIncludedQuery() =>
                query.Include(includePath);

            return new DeterminedIncludableQuery<EntityType, PropertyType>(query, getIncludedQuery, determinator);
        }

        /// <summary>
        /// Specifies related entities to include in the query results. The navigation 
        /// property to be included is specified starting with the type of entity being
        /// queried (<typeparamref name="EntityType"/>). If you wish to include additional
        /// types based on the navigation properties of the type being included, then chain
        /// a call to <see cref="ThenIncludeIf{EntityType, PreviousPropertyType, PropertyType}(IDeterminedIncludableQueryable{EntityType, IEnumerable{PreviousPropertyType}}, Expression{Func{PreviousPropertyType, PropertyType}}, bool)"/>
        /// after this call.
        /// </summary>
        /// <typeparam name="EntityType">The type of entity being queried.</typeparam>
        /// <typeparam name="PropertyType">The type of related entity to be included.</typeparam>
        /// <param name="determinedQuery">The source query.</param>
        /// <param name="includePath">A lambda expression representing the navigation property to be included</param>
        /// <param name="determinator">A boolean that determines the related entity being included or not.</param>
        /// <returns>A new includable query with related entity to be included if <paramref name="determinator"/> is true.</returns>
        /// <exception cref="ArgumentNullException">The parameter <paramref name="determinedQuery"/> is null.</exception>
        public static IDeterminedIncludableQueryable<EntityType, PropertyType> IncludeIf<EntityType, PreviousPropertyType, PropertyType>(
            this IDeterminedIncludableQueryable<EntityType, PreviousPropertyType> determinedQuery,
            Expression<Func<EntityType, PropertyType>> includePath, bool determinator)
            where EntityType : class =>
            determinedQuery.DeterminedQuery.IncludeIf(includePath, determinator);

        /// <summary>
        /// Specifies related entities to include in the query results. The navigation 
        /// property to be included is specified starting with the type of entity being
        /// queried (<typeparamref name="EntityType"/>). If you wish to include additional
        /// types based on the navigation properties of the type being included, then chain
        /// a call to <see cref="ThenIncludeIf{EntityType, PreviousPropertyType, PropertyType}(IDeterminedIncludableQueryable{EntityType, IEnumerable{PreviousPropertyType}}, Expression{Func{PreviousPropertyType, PropertyType}}, bool)"/>
        /// after this call.
        /// </summary>
        /// <typeparam name="EntityType">The type of entity being queried.</typeparam>
        /// <typeparam name="PropertyType">The type of related entity to be included.</typeparam>
        /// <param name="determinedQuery">The source query.</param>
        /// <param name="includePath">A lambda expression representing the navigation property to be included</param>
        /// <returns>A new includable query with related entity to be included.</returns>
        /// <exception cref="ArgumentNullException">The parameter <paramref name="determinedQuery"/> is null.</exception>
        public static IIncludableQueryable<EntityType, PropertyType> Include<EntityType, PreviousPropertyType, PropertyType>(
            this IDeterminedIncludableQueryable<EntityType, PreviousPropertyType> determinedQuery,
            Expression<Func<EntityType, PropertyType>> includePath)
            where EntityType : class =>
            determinedQuery.DeterminedQuery.Include(includePath);

        private static IDeterminedIncludableQueryable<EntityType, PropertyType> thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(
            this IQueryable<EntityType> query, Func<IIncludableQueryable<EntityType, PropertyType>> getIncludedQuery, bool determinator)
            where EntityType : class =>
            new DeterminedIncludableQuery<EntityType, PropertyType>(query, getIncludedQuery, determinator);

        public static IDeterminedIncludableQueryable<EntityType, PropertyType> ThenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(
            this IIncludableQueryable<EntityType, PreviousPropertyType> query, Expression<Func<PreviousPropertyType, PropertyType>> includePath,
            bool determinator)
            where EntityType : class
        {
            IIncludableQueryable<EntityType, PropertyType> getIncludedQuery() =>
                 query.ThenInclude(includePath);


/* Unmerged change from project 'Teronis.NetStandard.EntityFrameworkCore.Query (net5.0)'
Before:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(query,
                getIncludedQuery, determinator);
After:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(getIncludedQuery, determinator);
*/

/* Unmerged change from project 'Teronis.NetStandard.EntityFrameworkCore.Query (netstandard2.1)'
Before:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(query,
                getIncludedQuery, determinator);
After:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(getIncludedQuery, determinator);
*/
            return query.thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(                getIncludedQuery, determinator);
        }

        public static IDeterminedIncludableQueryable<EntityType, PropertyType> ThenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(
            this IIncludableQueryable<EntityType, IEnumerable<PreviousPropertyType>> query,
            Expression<Func<PreviousPropertyType, PropertyType>> includePath, bool determinator)
            where EntityType : class
        {
            IIncludableQueryable<EntityType, PropertyType> getIncludedQuery() =>
                 query.ThenInclude(includePath);


/* Unmerged change from project 'Teronis.NetStandard.EntityFrameworkCore.Query (net5.0)'
Before:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(query,
                getIncludedQuery, determinator);
After:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(getIncludedQuery, determinator);
*/

/* Unmerged change from project 'Teronis.NetStandard.EntityFrameworkCore.Query (netstandard2.1)'
Before:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(query,
                getIncludedQuery, determinator);
After:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(getIncludedQuery, determinator);
*/
            return query.thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(                getIncludedQuery, determinator);
        }

        public static IDeterminedIncludableQueryable<EntityType, PropertyType> ThenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(
            this IDeterminedIncludableQueryable<EntityType, PreviousPropertyType> determinedQuery,
            Expression<Func<PreviousPropertyType, PropertyType>> includePath, bool determinator)
            where EntityType : class
        {
            IIncludableQueryable<EntityType, PropertyType> getIncludedQuery() =>
                 determinedQuery.IncludedQueryFactory().ThenInclude(includePath);


/* Unmerged change from project 'Teronis.NetStandard.EntityFrameworkCore.Query (net5.0)'
Before:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(determinedQuery.DeterminedQuery,
                getIncludedQuery, determinedQuery.Determinator & determinator);
After:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(getIncludedQuery, determinedQuery.Determinator & determinator);
*/

/* Unmerged change from project 'Teronis.NetStandard.EntityFrameworkCore.Query (netstandard2.1)'
Before:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(determinedQuery.DeterminedQuery,
                getIncludedQuery, determinedQuery.Determinator & determinator);
After:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(getIncludedQuery, determinedQuery.Determinator & determinator);
*/
            return determinedQuery.DeterminedQuery.thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(                getIncludedQuery, determinedQuery.Determinator & determinator);
        }

        public static IDeterminedIncludableQueryable<EntityType, PropertyType> ThenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(
            this IDeterminedIncludableQueryable<EntityType, IEnumerable<PreviousPropertyType>> determinedQuery,
            Expression<Func<PreviousPropertyType, PropertyType>> includePath, bool determinator)
            where EntityType : class
        {
            IIncludableQueryable<EntityType, PropertyType> getIncludedQuery() =>
                determinedQuery.IncludedQueryFactory().ThenInclude(includePath);


/* Unmerged change from project 'Teronis.NetStandard.EntityFrameworkCore.Query (net5.0)'
Before:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(determinedQuery.DeterminedQuery,
                getIncludedQuery, determinedQuery.Determinator & determinator);
After:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(getIncludedQuery, determinedQuery.Determinator & determinator);
*/

/* Unmerged change from project 'Teronis.NetStandard.EntityFrameworkCore.Query (netstandard2.1)'
Before:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(determinedQuery.DeterminedQuery,
                getIncludedQuery, determinedQuery.Determinator & determinator);
After:
            return thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(getIncludedQuery, determinedQuery.Determinator & determinator);
*/
            return determinedQuery.DeterminedQuery.thenIncludeIf<EntityType, PreviousPropertyType, PropertyType>(                getIncludedQuery, determinedQuery.Determinator & determinator);
        }
    }
}
