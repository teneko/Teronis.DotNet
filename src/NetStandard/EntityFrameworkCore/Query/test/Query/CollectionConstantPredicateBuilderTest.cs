using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Teronis.EntityFrameworkCore.Query;
using Xunit;

namespace Test.NetStandard.EntityFrameworkCore.Query
{
    public class CollectionConstantPredicateBuilderTest
    {
        [Fact]
        public void Replaces_source_and_target_mapping_parameters_by_instance_ones()
        {
            var comparisonValueList = new[] { new ComparisonA() { Properties = new string[] { "" } } };

            var bodyExpression = CollectionConstantPredicateBuilder<ClassB>
                .CreateFromCollection(comparisonValueList)
                .DefinePredicatePerItem(Expression.OrElse,
                    (b, value) => b.PropertyB1 != null && b.PropertyB1 == value.Property)
                .ThenCreateFromCollection(Expression.AndAlso, value => value.Properties)
                .DefinePredicatePerItem(Expression.OrElse,
                    (b, value) => b.PropertiesCB1.Contains(default))
                .BuildBodyExpression<ClassA>(memberMapper => {
                    memberMapper.Map(b => b.PropertyB1, a => a.PropertyA1);
                    memberMapper.Map(b => b.PropertiesCB1, a => a.PropertiesA1);
                }, out var targetParameter);

            var parameterCollector = new ParameterExpressionCollectorVisitor();
            parameterCollector.Visit(bodyExpression);

            Assert.All(parameterCollector.ParameterExpressions, parameter => {
                Assert.Equal(targetParameter, parameter);
            });
        }

        public class ClassA
        {
            public string? PropertyA1 { get; set; }
            public string[]? PropertiesA1 { get; set; }
        }

        public class ComparisonA
        {
            public string? Property { get; set; }
            public string[]? Properties { get; set; }
        }

        public class ClassB
        {
            public string? PropertyB1 { get; set; }
            public string[]? PropertiesCB1 { get; set; }
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
