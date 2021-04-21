// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class ListQueue<T> : List<T>
    {
        public T Push(T input)
        {
            Add(input!);
            return input;
        }

        public T Peek() =>
            this[0];

        public T Pop()
        {
            if (Count == 0) {
                throw new InvalidOperationException("There are no items left to be popped.");
            }

            var output = this[0];
            RemoveAt(0);
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
