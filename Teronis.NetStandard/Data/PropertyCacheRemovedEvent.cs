using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public delegate void PropertyCacheAddedEvent<TProperty>(object sender, PropertyCacheAddedEventArgs<TProperty> args);
}
