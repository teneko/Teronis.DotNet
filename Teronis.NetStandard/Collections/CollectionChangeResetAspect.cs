using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Teronis.Collections
{
   public class CollectionChangeReplaceAspect<TItem>
    {
        /// <summary>
        /// Each value represents the referenced value before it got replaced.
        /// </summary>
        public IReadOnlyDictionary<int, TItem> ReferencedReplacedOldItemByIndexDictionary { get; private set; }

        private Dictionary<int, TItem> referencedReplacedOldItemByIndexDictionary;

        public CollectionChangeReplaceAspect() {
            referencedReplacedOldItemByIndexDictionary = new Dictionary<int, TItem>();
            ReferencedReplacedOldItemByIndexDictionary = new ReadOnlyDictionary<int, TItem>(referencedReplacedOldItemByIndexDictionary);
        }

        public void RegisterReferenceReplacedOldItem(int oldItemIndex, TItem replacedOldItem)
            => referencedReplacedOldItemByIndexDictionary.Add(oldItemIndex, replacedOldItem);
    }
}
