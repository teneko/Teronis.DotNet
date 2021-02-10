using System;
using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// Represents the delegate for the handler that is called from <see cref="CollectionModificationIterationTools.IteratorBuilder.Iterate"/>.
    /// </summary>
    /// <param name="itemIndex">The index of item without index offset.</param>
    /// <param name="indexOffset">The index offset.</param>
    public delegate void CollectionModificationIterationDelegate(int itemIndex, int indexOffset);

    internal class CollectionModificationIterationTools
    {
        /// <summary>
        /// Creates an iterator builder.
        /// </summary>
        /// <param name="modification"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when member <see cref="ICollectionModificationParameters.NewItemsCount"/> is null.</exception>
        public static IteratorBuilder BeginInsert(ICollectionModificationParameters modification)
        {
            if (modification.NewItemsCount is null) {
                throw new ArgumentException("New items were not given while inserting.");
            }

            return new IteratorBuilder(
                iterationCount: modification.NewItemsCount.Value,
                indexOffset: modification.NewIndex,
                iteratesBackward: false);
        }

        /// <summary>
        /// Creates an iterator builder.
        /// </summary>
        /// <param name="modification"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when member <see cref="ICollectionModificationParameters.OldItemsCount"/> is null.</exception>
        public static IteratorBuilder BeginRemove(ICollectionModificationParameters modification)
        {
            if (modification.OldItemsCount is null) {
                throw new ArgumentException("Old items were not given while removing.");
            }

            return new IteratorBuilder(
                iterationCount: modification.OldItemsCount.Value,
                indexOffset: modification.OldIndex,
                iteratesBackward: true);
        }

        /// <summary>
        /// Checks move action.
        /// </summary>
        /// <param name="modification"></param>
        /// <exception cref="ArgumentException">Thrown when <see cref="ICollectionModificationParameters.OldItemsCount"/> is null.</exception>
        public static void CheckMove(ICollectionModificationParameters modification) {
            if (modification.OldItemsCount is null) {
                throw new ArgumentException("Old items were not given while moving.");
            }
        }

        public static IteratorBuilder BeginReplace(ICollectionModificationParameters modification) {
            if (modification.NewItemsCount is null) {
                throw new ArgumentNullException("New items were not given while replacing");
            }

            return new IteratorBuilder(
                modification.NewItemsCount.Value, 
                modification.NewIndex, 
                iteratesBackward: false);
        }

        public class IteratorBuilder {

            public bool IteratesBackward { get; }

            private List<CollectionModificationIterationDelegate> handlers;
            private readonly int iterationCount;
            private readonly int indexOffset;

            public IteratorBuilder(int iterationCount, int indexOffset, bool iteratesBackward)
            {
                handlers = new List<CollectionModificationIterationDelegate>();
                this.iterationCount = iterationCount;
                this.indexOffset = indexOffset;
                IteratesBackward = iteratesBackward;
            }

            public IteratorBuilder Add(CollectionModificationIterationDelegate actionHandler)
            {
                if (actionHandler is null) {
                    throw new ArgumentNullException(nameof(actionHandler));
                }

                handlers.Add(actionHandler);
                return this;
            }

            public void Iterate() {
                if (IteratesBackward) {
                    for (var index = iterationCount - 1; index >= 0; index--) {
                        foreach (var handler in handlers) {
                            handler.Invoke(index, indexOffset);
                        }
                    }
                } else {
                    for (var index = 0; index < iterationCount; index++) {
                        foreach (var handler in handlers) {
                            handler.Invoke(index, indexOffset);
                        }
                    }
                }
            }
        }
    }
}
