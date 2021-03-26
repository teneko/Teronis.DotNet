// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Utils
{
    public static class LinkedListUtils
    {
        public static IEnumerable<LinkedListNode<T>> YieldNodesReversed<T>(LinkedListNode<T> node)
        {
            var currentNode = node;

            while (currentNode != null) {
                yield return currentNode;
                currentNode = currentNode.Previous;
            }
        }
    }
}
