using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChangeAppliedEventArgs<TItem> : EventArgs
    {
        public AsyncableEventSequence EventSequence { get; private set; }
        public AspectedCollectionChange<TItem> AspectedCollectionChange { get; private set; }

        public CollectionChangeAppliedEventArgs(AspectedCollectionChange<TItem> aspectedCollectionChange, AsyncableEventSequence eventSequence)
        {
            EventSequence = eventSequence;
            AspectedCollectionChange = aspectedCollectionChange;
        }

        public CollectionChangeAppliedEventArgs(AspectedCollectionChange<TItem> aspectedCollectionChange)
            : this(aspectedCollectionChange, default) { }
    }
}
