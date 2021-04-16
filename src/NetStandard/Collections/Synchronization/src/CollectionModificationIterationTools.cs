// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// Represents the delegate for the handler that is called from <see cref="CollectionModificationIterationTools.IteratorBuilder.Iterate"/>.
    /// </summary>
    /// <param name="modificationItemIndex">The item index in the scope of the modification.</param>
    /// <param name="collectionStartIndex">
    /// The first index of the collection that is affected by the modification. If you add the modification item index and the collection start
    /// index you get the actual item index of the collection. The collection the talk is about is the collection which modification you apply on.
    /// </param>
    public delegate void CollectionModificationIterationWithIndexDelegate(int modificationItemIndex, int collectionStartIndex);

    public delegate void CollectionModificationIterationDelegate();

    public static class CollectionModificationIterationTools
    {
        /// <summary>
        /// Creates an iterator builder for a modification that has items to be inserted to a collection.
        /// </summary>
        /// <param name="modification"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Member <see cref="ICollectionModificationParameters.NewItemsCount"/> is null.</exception>
        public static IteratorBuilder BeginInsert(ICollectionModificationParameters modification)
        {
            if (modification.NewItemsCount is null) {
                throw new ArgumentException("New items were not given while inserting.");
            }

            return new IteratorBuilder(
                iterationCount: modification.NewItemsCount.Value,
                collectionStartIndex: modification.NewIndex,
                iteratesBackward: false);
        }

        /// <summary>
        /// Creates an iterator builder for a modification that has items to be rmeoved from a collection.
        /// </summary>
        /// <param name="modification"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Member <see cref="ICollectionModificationParameters.OldItemsCount"/> is null.</exception>
        public static IteratorBuilder BeginRemove(ICollectionModificationParameters modification)
        {
            if (modification.OldItemsCount is null) {
                throw new ArgumentException("Old items were not given while removing.");
            }

            return new IteratorBuilder(
                iterationCount: modification.OldItemsCount.Value,
                collectionStartIndex: modification.OldIndex,
                iteratesBackward: true);
        }

        /// <summary>
        /// Checks a modification that is about to move items existing items. Thus it throws an exception if no old items are present.
        /// </summary>
        /// <param name="modification"></param>
        /// <exception cref="ArgumentException">Member <see cref="ICollectionModificationParameters.OldItemsCount"/> is null.</exception>
        public static void CheckMove(ICollectionModificationParameters modification)
        {
            if (modification.OldItemsCount is null) {
                throw new ArgumentException("Old items were not given while moving.");
            }
        }

        /// <summary>
        /// Creates an iterator builder for a modification that has the instruction to replace items.
        /// </summary>
        /// <param name="modification"></param>
        /// <exception cref="ArgumentException">Member <see cref="ICollectionModificationParameters.NewItemsCount"/> is null.</exception>
        public static IteratorBuilder BeginReplace(ICollectionModificationParameters modification)
        {
            if (modification.NewItemsCount is null) {
                throw new ArgumentNullException("New items were not given while replacing");
            }

            return new IteratorBuilder(
                modification.NewItemsCount.Value,
                modification.NewIndex,
                iteratesBackward: false);
        }

        public class IteratorBuilder
        {

            public bool IteratesBackward { get; }

            private List<MulticastDelegate> handlers;
            /// <summary>
            /// Equal to the amount of items.
            /// </summary>
            private readonly int iterationCount;
            /// <summary>
            /// The first index of the collection that is affected by the modification. If you add the modification item index and the collection start
            /// index you get the actual item index of the collection. The collection the talk is about is the collection which modification you apply on.
            /// </summary>
            private readonly int collectionStartIndex;

            public IteratorBuilder(int iterationCount, int collectionStartIndex, bool iteratesBackward)
            {
                handlers = new List<MulticastDelegate>();
                this.iterationCount = iterationCount;
                this.collectionStartIndex = collectionStartIndex;
                IteratesBackward = iteratesBackward;
            }

            /// <summary>
            /// Adds a function handler to the per-item-iteration pipeline.
            /// </summary>
            /// <param name="actionHandler"></param>
            /// <returns></returns>
            public IteratorBuilder Add(CollectionModificationIterationWithIndexDelegate actionHandler)
            {
                if (actionHandler is null) {
                    throw new ArgumentNullException(nameof(actionHandler));
                }

                handlers.Add(actionHandler);
                return this;
            }

            /// <summary>
            /// Adds a non-parameterized function handler to the per-item-iteration pipeline.
            /// </summary>
            /// <param name="actionHandler"></param>
            /// <returns></returns>
            public IteratorBuilder Add(CollectionModificationIterationDelegate actionHandler)
            {
                if (actionHandler is null) {
                    throw new ArgumentNullException(nameof(actionHandler));
                }

                handlers.Add(actionHandler);
                return this;
            }

            /// <summary>
            /// Iterates each previously added action handler per modification item.
            /// </summary>
            public void Iterate()
            {
                void invokeIterationStep(int modificationItemIndex)
                {
                    foreach (var handler in handlers) {
                        if (handler is CollectionModificationIterationWithIndexDelegate iterationWithIndexHandler) {
                            iterationWithIndexHandler.Invoke(modificationItemIndex, collectionStartIndex);
                        } else if (handler is CollectionModificationIterationDelegate iterationHandler) {
                            iterationHandler.Invoke();
                        } else {
                            throw new NotSupportedException();
                        }
                    }
                }

                if (IteratesBackward) {
                    for (var modificationItemIndex = iterationCount - 1; modificationItemIndex >= 0; modificationItemIndex--) {
                        invokeIterationStep(modificationItemIndex);
                    }
                } else {
                    for (var modificationItemIndex = 0; modificationItemIndex < iterationCount; modificationItemIndex++) {
                        invokeIterationStep(modificationItemIndex);
                    }
                }
            }
        }
    }
}
