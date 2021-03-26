// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public interface IParameterReplacingExpressionMapper<SourceType, TargetType>
    {
        void MapBodyAndParams<SourcePropertyType, TargetPropertyType>(Expression<Func<SourceType, SourcePropertyType>> subject, 
            Expression<Func<TargetType, TargetPropertyType>> replacment);
    }
}
