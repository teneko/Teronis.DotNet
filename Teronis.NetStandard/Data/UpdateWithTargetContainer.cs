using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    internal class UpdateWithTargetContainer<TUpdateItem, TItem>
    {
            public Update<TUpdateItem> Update { get; set; }
            public TItem Target { get; set; }
    }
}
