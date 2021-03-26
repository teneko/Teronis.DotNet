// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public interface IExpressionMapper 
    {
        void Map(Expression from, Expression to);
        void MapBody(LambdaExpression from, LambdaExpression to);
    }
}
