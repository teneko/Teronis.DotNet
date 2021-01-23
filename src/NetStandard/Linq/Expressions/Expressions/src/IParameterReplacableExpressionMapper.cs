using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public interface IParameterReplacableExpressionMapper<SourceType, TargetType> : IParameterReplacingExpressionMapper<SourceType, TargetType>, IExpressionMapper
    { }
}
