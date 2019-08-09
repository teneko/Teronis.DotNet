using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public delegate void PropertyCacheAddingEvent<TProperty>(object sender, PropertyCacheAddingEventArgs<TProperty> args);
}
