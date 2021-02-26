using System;
using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// Represents the delegate for the handler that is called from <see cref="CollectionModificationIterationTools.IteratorBuilder.Iterate"/>.
    /// </summary>
    /// <param name="frontIndex">The modification item index without index offset.</param>
    /// <param name="backIndexOffset">The index offset that is when summed up with <param name="frontIndex"/> the index of the synchronized list.</param>
    public delegate void CollectionModificationIterationWithIndexDelegate(int frontIndex, int backIndexOffset);

    public delegate void CollectionModificationIterationDelegate();

    public class CollectionModificationIterationTools
    {
        /// <summary>
        /// Creates an iterator builder.
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
                backIndexOffset: modification.NewIndex,
                iteratesBackward: false);
        }

        /// <summary>
        /// Creates an iterator builder.
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
                backIndexOffset: modification.OldIndex,
                iteratesBackward: true);
        }

        /// <summary>
        /// Checks move action.
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
        /// Creates an iterator builder.
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
            private readonly int iterationCount;
            private readonly int backIndexOffset;

            public IteratorBuilder(int iterationCount, int backIndexOffset, bool iteratesBackward)
            {
                handlers = new List<MulticastDelegate>();
                this.iterationCount = iterationCount;
                this.backIndexOffset = backIndexOffset;
                IteratesBackward = iteratesBackward;
            }

            public IteratorBuilder Add(CollectionModificationIterationWithIndexDelegate actionHandler)
            {
                if (actionHandler is null) {
                    throw new ArgumentNullException(nameof(actionHandler));
                }

                handlers.Add(actionHandler);
                return this;
            }

            public IteratorBuilder Add(CollectionModificationIterationDelegate actionHandler)
            {
                if (actionHandler is null) {
                    throw new ArgumentNullException(nameof(actionHandler));
                }

                handlers.Add(actionHandler);
                return this;
            }

            public void Iterate()
            {
                void invokeIterationStep(int modificationItemIndex)
                {
                    foreach (var handler in handlers) {
                        if (handler is CollectionModificationIterationWithIndexDelegate iterationWithIndexHandler) {
                            iterationWithIndexHandler.Invoke(modificationItemIndex, backIndexOffset);
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
