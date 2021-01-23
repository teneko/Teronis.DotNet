using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Teronis.Linq.Expressions
{
    public class ExpressionComparerTests
    {
        [Theory]
        [ClassData(typeof(ExpressionGenerator))]
        public void Compare_expressions(Expression x, Expression y, bool shouldBeEqual)
        {
            var equal = ExpressionEqualityComparer.Default.Equals(x, y);

            if (shouldBeEqual) {
                Assert.True(equal);
            } else {
                Assert.False(equal);
            }
        }

        public class ExpressionGenerator : IEnumerable<object[]>
        {
            private object[] array(params object[] values) =>
                values;

            private Expression expression<T>(Expression<Func<T>> expression) =>
                expression;

            private Expression expression<P, T>(Expression<Func<P, T>> expression) =>
                expression;

            public IEnumerator<object[]> GetEnumerator()
            {
                var twoFactory = expression(() => 2);
                yield return array(twoFactory, twoFactory, true);
                yield return array(twoFactory, expression(() => 3), false);
                var listExpression = expression(((int i, List<int> list) x) => x.list);
                yield return array(listExpression, listExpression, true);
                yield return array(listExpression, expression(((int i, List<bool> list) x) => x.list), false);
            }

            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();
        }
    }
}
