using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class UnwindedExpressionQueue : IReadOnlyCollection<Expression>, IEnumerable<Expression>
    {
        public int Count => expressionQueue.Count;

        private readonly Queue<Expression> expressionQueue;

        public UnwindedExpressionQueue(Expression expression)
        {
            var visitor = new ExpressionEnumeratorVisitor();
            visitor.Visit(expression);
            expressionQueue = visitor.Expressions;
        }

        public Expression Peek() =>
            expressionQueue.Peek();

        public Expression Dequeue() =>
            expressionQueue.Dequeue();

        public bool TryDequeue([MaybeNullWhen(false)] out Expression expression)
        {
            if (expressionQueue.Count == 0) {
                expression = null;
                return false;
            }

            expression = expressionQueue.Dequeue();
            return true;
        }

        public IEnumerator<Expression> GetEnumerator() =>
            expressionQueue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        private class ExpressionEnumeratorVisitor : ExpressionVisitor
        {
            public Queue<Expression> Expressions { get; }

            public ExpressionEnumeratorVisitor()
            {
                Expressions = new Queue<Expression>();
            }

            public override Expression? Visit(Expression? node)
            {
                if (node is null) {
                    return null;
                }

                Expressions.Enqueue(node);
                return base.Visit(node);
            }
        }
    }
}
