using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public delegate void CollectionChangeAppliedEventHandler<T>(object sender, AspectedCollectionChange<T> aspectedChange);
}
