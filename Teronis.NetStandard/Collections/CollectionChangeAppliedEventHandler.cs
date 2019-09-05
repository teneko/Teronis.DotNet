using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public delegate void CollectionChangeAppliedEventHandler<ItemType, ContentType>(object sender, CollectionChangeAppliedEventArgs<ItemType, ContentType> args);
}
