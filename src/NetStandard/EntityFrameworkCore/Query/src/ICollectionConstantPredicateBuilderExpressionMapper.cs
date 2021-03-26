// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public interface ICollectionConstantPredicateBuilderExpressionMapper<SourceType, TargetType> : IParameterReplacableExpressionMapper<SourceType, TargetType>
    {}
}
