using System.Collections.Generic;
using System.Linq.Expressions;
using Teronis.EntityFrameworkCore.Query;
using Xunit;

namespace Test.NetStandard.Linq.Expressions.EntityFrameworkCore
{
    public class CollectionConstantPredicateBuilderTest
    {
        [Fact]
        public void Replaces_source_and_target_mapping_parameters_by_instance_ones()
        {
            var comparisonValueList = new[] { new ComparisonA() };

            var bodyExpression = CollectionConstantPredicateBuilder<ClassB>
                .FromComparisonList(comparisonValueList)
                .CreateBuilder(Expression.OrElse,
                    (source, value) => source.PropertyB1 == value.Property)
                .BuildBodyExpression<ClassA>(memberMapper => {
                    memberMapper.Map(b => b.PropertyB1, a => a.PropertyA1);
                }, out var targetParameter);

            var parameterCollector = new ParameterExpressionCollectorVisitor();
            parameterCollector.Visit(bodyExpression);

            Assert.Collection(parameterCollector.ParameterExpressions, parameter => {
                Assert.Equal(targetParameter, parameter);
            });
        }

        public class ClassA
        {
            public string? PropertyA1 { get; }
        }

        public class ComparisonA
        {
            public string? Property { get; }
        }

        public class ClassB
        {
            public string? PropertyB1 { get; }
        }

        public class ParameterExpressionCollectorVisitor : ExpressionVisitor
        {
            public List<ParameterExpression> ParameterExpressions { get; }

            public ParameterExpressionCollectorVisitor() =>
                ParameterExpressions = new List<ParameterExpression>();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpressions.Add(node);
                return node;
            }
        }
    }
}
