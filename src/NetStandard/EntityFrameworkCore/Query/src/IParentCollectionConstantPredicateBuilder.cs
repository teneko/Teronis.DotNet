// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    internal interface IParentCollectionConstantPredicateBuilder
    {
        bool IsRoot { get; }
        ParameterExpression SourceParameterExpression { get; }
        /// <summary>
        /// Puts <paramref name="builder"/> on top of parent builder.
        /// </summary>
        /// <param name="builder"></param>
        void StackBuilder(IChildCollectionConstantPredicateBuilder builder);
        bool TryPopBuilder([MaybeNullWhen(false)] out IChildCollectionConstantPredicateBuilder builder);
        void AppendExpression(Expression expression, Func<Expression, Expression, BinaryExpression> binaryExpression);
    }
}
