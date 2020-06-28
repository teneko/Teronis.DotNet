using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public class ListQueue<T> : List<T>
    {
        [return: MaybeNull]
        public T Push([AllowNull] T input)
        {
            Add(input!);
            return input;
        }

        public T Peek() =>
            this[0];

        [return: MaybeNull]
        public T Pop()
        {
            T output = default;

            if (Count != 0) {
                output = this[0];
                RemoveAt(0);
            }

            return output;
        }

        public ListQueue<T> PopAndPush()
        {
            if (Count != 0) {
                Push(Pop());
            }

            return this;
        }
    }
}
