using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.Collections.ObjectModel
{
    public class IndexShiftedEventArgs : EventArgs
    {
        public ICollectionChange<object> CollectionChange { get; private set; }

        public IndexShiftedEventArgs(ICollectionChange<object> collectionChange)
            => CollectionChange = collectionChange;
    }
}
