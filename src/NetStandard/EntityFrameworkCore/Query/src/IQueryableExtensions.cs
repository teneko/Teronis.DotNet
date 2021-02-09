using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Teronis.Collections.Generic;

namespace Teronis.EntityFrameworkCore.Query
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// This method is very complicated but listen: 
        /// <br/><paramref name="keyOneKeyManyEnumerablePairs"/> is going to be flattened down to be compatible with EF Core - a sub query will be made of it.
        /// <br/><typeparamref name="OneType"/> must be a part of <typeparamref name="EntityType"/>. <paramref name="getValue"/> is the "pointer" of such part
        /// and is used in expression tree for EF Core. Accessing anything other than <typeparamref name="EntityType"/> in scope of <paramref name="getValue"/>
        /// is strictly prohibited.
        /// <br/><typeparamref name="FlatManyType"/> must be a part of <typeparamref name="EntityType"/>. <paramref name="getFlattenedValue"/> is the "pointer" of such part
        /// and is used in expression tree for EF Core. Accessing anything other than <typeparamref name="EntityType"/> in scope of <paramref name="getValue"/>
        /// is strictly prohibited.
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        /// <typeparam name="OneType"></typeparam>
        /// <typeparam name="FlatManyType"></typeparam>
        /// <param name="query"></param>
        /// <param name="getValue"></param>
        /// <param name="getFlattenedValue"></param>
        /// <param name="keyOneKeyManyEnumerablePairs"></param>
        /// <param name="keyManyEnumerableBehvaiourFlags"></param>
        /// <returns></returns>
        public static IQueryable<EntityType> HasOneWithMany<EntityType, OneType, FlatManyType>(
            this IQueryable<EntityType> query,
            Expression<Func<EntityType, OneType>> getValue, Expression<Func<EntityType, FlatManyType>> getFlattenedValue,
            IEnumerable<ICovariantKeyValuePair<IYetNullable<OneType>, IEnumerable<FlatManyType>?>>? keyOneKeyManyEnumerablePairs,
            ComparisonItemsBehaviourFlags keyManyEnumerableBehvaiourFlags = ComparisonItemsBehaviourFlags.EmptyLeadsToFalse)
            where OneType : notnull
        {
            if (keyOneKeyManyEnumerablePairs is null) {
                return query;
            }

            var whereExpression = CollectionConstantPredicateBuilder<IMappableCompositeOneToManyKey<OneType, FlatManyType>>
                .CreateFromCollection(keyOneKeyManyEnumerablePairs!)
                .DefinePredicatePerItem(Expression.OrElse,
                    (source, comparisonValue) => !comparisonValue.Key.HasValue || Equals(comparisonValue.Key.Value, source.KeyOne))
                .ThenCreateFromCollection(Expression.AndAlso, x => x.Value,
                        comparisonItemsBehaviourFlags: keyManyEnumerableBehvaiourFlags)
                .ThenDefinePredicatePerItem(Expression.OrElse,
                    (source, value) => Equals(source.KeyMany, value))
                .BuildLambdaExpression<EntityType>(mapper => {
                    mapper.MapBodyAndParams(compositeKey => compositeKey.KeyOne, getValue);
                    mapper.MapBodyAndParams(compositeKey => compositeKey.KeyMany, getFlattenedValue);
                });

            return query.Where(whereExpression);
        }

        ///// <summary>
        ///// This is the negation of <see cref="ContainsCompositeOneToManyKeys"/>.
        ///// <br/>This method is very complicated but listen: 
        ///// <br/><paramref name="keyOneKeyManyEnumerablePairs"/> is going to be flattened down to be compatible with EF Core - a sub query will be made of it.
        ///// <br/><typeparamref name="TKeyOne"/> must be a part of <typeparamref name="EntityType"/>. <paramref name="getKeyOne"/> is the "pointer" of such part
        ///// and is used in expression tree for EF Core. Accessing anything other than <typeparamref name="EntityType"/> in scope of <paramref name="getKeyOne"/>
        ///// is strictly prohibited.
        ///// <br/><typeparamref name="TKeyMany"/> must be a part of <typeparamref name="EntityType"/>. <paramref name="getKeyMany"/> is the "pointer" of such part
        ///// and is used in expression tree for EF Core. Accessing anything other than <typeparamref name="EntityType"/> in scope of <paramref name="getKeyOne"/>
        ///// is strictly prohibited.
        ///// </summary>
        ///// <typeparam name="EntityType"></typeparam>
        ///// <typeparam name="TKeyOne"></typeparam>
        ///// <typeparam name="TKeyMany"></typeparam>
        ///// <param name="query"></param>
        ///// <param name="getKeyOne"></param>
        ///// <param name="getKeyMany"></param>
        ///// <param name="keyOneKeyManyEnumerablePairs"></param>
        ///// <param name="keyManyEnumerableBehvaiourFlags"></param>
        ///// <returns></returns>
        //public static IQueryable<EntityType> ContainsNotCompositeOneToManyKeys<EntityType, TKeyOne, TKeyMany>(
        //    this IQueryable<EntityType> query,
        //    Expression<Func<EntityType, TKeyOne>> getKeyOne, Expression<Func<EntityType, TKeyMany>> getKeyMany,
        //    IEnumerable<ICovariantKeyValuePair<IYetNullable<TKeyOne>, IEnumerable<TKeyMany>?>>? keyOneKeyManyEnumerablePairs = null,
        //    ComparisonItemsBehaviourFlags keyManyEnumerableBehvaiourFlags = ComparisonItemsBehaviourFlags.NullOrEmptyLeadsToSkip)
        //    where TKeyOne : notnull
        //{
        //    if (!keyOneKeyManyEnumerablePairs.IsNullOrEmpty()) {
        //        var whereExpression = CollectionConstantPredicateBuilder<IMappableCompositeOneToManyKey<TKeyOne, TKeyMany>>
        //            .CreateFromCollection(keyOneKeyManyEnumerablePairs!)
        //            .DefinePredicatePerItem(Expression.OrElse,
        //                (source, comparisonValue) => !comparisonValue.Key.HasValue || Equals(comparisonValue.Key.Value, source.KeyOne))
        //            .ThenCreateFromCollection(Expression.AndAlso, x => x.Value,
        //                 comparisonItemsBehaviourFlags: keyManyEnumerableBehvaiourFlags)
        //            .ThenDefinePredicatePerItem(Expression.AndAlso,
        //                (source, value) => !Equals(source.KeyMany, value))
        //            .BuildLambdaExpression<EntityType>(mapper => {
        //                mapper.MapBodyAndParams(compositeKey => compositeKey.KeyOne, getKeyOne);
        //                mapper.MapBodyAndParams(compositeKey => compositeKey.KeyMany, getKeyMany);
        //            });

        //        query = query.Where(whereExpression);
        //    }

        //    return query;
        //}

        private interface IMappableCompositeOneToManyKey<TKeyOne, TKeyMany>
        {
            TKeyOne KeyOne { get; }
            TKeyMany KeyMany { get; }
        }
    }
}
