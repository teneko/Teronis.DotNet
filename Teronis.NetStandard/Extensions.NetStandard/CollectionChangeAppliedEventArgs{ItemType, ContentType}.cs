using System;
using System.Collections.Generic;
using System.Text;
using Teronis.Collections;

namespace Teronis.Extensions.NetStandard
{
    public static class CollectionChangeAppliedEventArgsExtensions
    {
        public static bool IsAsyncEvent<ItemType, ContentType>(this CollectionChangeAppliedEventArgs<ItemType, ContentType> args)
            => args.EventSequence != null;
    }
}
