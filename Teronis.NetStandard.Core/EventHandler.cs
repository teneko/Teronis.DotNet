using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard
{
    public delegate void EventHandler<in TSender, in TArgs>(TSender sender, TArgs args);
}
