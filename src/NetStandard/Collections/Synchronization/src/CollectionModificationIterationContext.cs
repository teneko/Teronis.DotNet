// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Synchronization
{
    public class CollectionModificationIterationContext
    {
        /// <summary>
        /// The item index in the scope of the modification.
        /// </summary>
        public int ModificationItemIndex {
            get => modificationItemIndex;

            internal set {
                modificationItemIndex = value;
                isCollectionIndexDirty = true;
            }
        }

        /// <summary>
        /// The first index of the collection that is affected by the modification. If you add the modification item index and the collection start
        /// index you get the actual item index of the collection. The collection the talk is about is the collection which modification you apply on.
        /// </summary>
        public int CollectionStartIndex { get; }

        public int CollectionItemIndex {
            get {
                if (isCollectionIndexDirty) {
                    collectionItemIndex = CollectionStartIndex + ModificationItemIndex;
                    isCollectionIndexDirty = false;
                }

                return collectionItemIndex;
            }
        }

        private bool isCollectionIndexDirty;
        private int collectionItemIndex;
        private int modificationItemIndex;

        public CollectionModificationIterationContext(int collectionStartIndex) =>
            CollectionStartIndex = collectionStartIndex;
    }
}
