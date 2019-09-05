using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Teronis.Collections
{
    public class CollectionChangeReplaceAspect<ItemType>
    {
        /// <summary>
        /// Each value represents the referenced value before it got replaced.
        /// </summary>
        //public IReadOnlyDictionary<int, ItemType> ReferencedReplacedOldItemByIndexDictionary { get; private set; }

        //private Dictionary<int, ItemType> referencedReplacedOldItemByIndexDictionary;

        public CollectionChangeReplaceAspect()
        {
            //referencedReplacedOldItemByIndexDictionary = new Dictionary<int, ItemType>();
            //ReferencedReplacedOldItemByIndexDictionary = new ReadOnlyDictionary<int, ItemType>(referencedReplacedOldItemByIndexDictionary);
        }

        //public void RegisterReferenceReplacedOldItem(int oldItemIndex, ItemType replacedOldItem)
        //    => referencedReplacedOldItemByIndexDictionary.Add(oldItemIndex, replacedOldItem);
    }
}
