using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public delegate void PropertyCacheRemovedEvent<TProperty>(object sender, PropertyCacheRemovedEventArgs<TProperty> args);
}
