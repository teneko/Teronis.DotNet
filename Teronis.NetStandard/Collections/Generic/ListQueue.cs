using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class ListQueue<T> : List<T>
    {
        public T Push(T input)
        {
            Add(input);
            //
            return input;
        }

        public T Peek()
        {
            return this[0];
        }

        public T Pop()
        {
            T output = default;

            if (Count != 0) {
                output = this[0];
                RemoveAt(0);
            }
            //
            return output;
        }

        public ListQueue<T> PopAndPush()
        {
            if (Count != 0)
                Push(Pop());
            //
            return this;
        }
    }
}
