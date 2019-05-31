using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.Collections
{
    public class AspectedCollectionChange<TItem>
    {
        public CollectionChange<TItem> Change { get; private set; }
        public CollectionChangeReplaceAspect<TItem> ReplaceAspect { get; private set; }
        public CollectionChangeResetAspect<TItem> ResetAspect { get; private set; }

        public AspectedCollectionChange(CollectionChange<TItem> change)
            => Change = change;

        public AspectedCollectionChange(CollectionChange<TItem> change, CollectionChangeReplaceAspect<TItem> replaceAspect)
            : this(change)
            => ReplaceAspect = replaceAspect;

        public AspectedCollectionChange(CollectionChange<TItem> change, CollectionChangeResetAspect<TItem> resetAspect)
             : this(change)
             => ResetAspect = resetAspect;
    }
}
