using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class PropertyCacheRemovedEventArgs : PropertyCacheEventArgs
    {
        public PropertyCacheRemovedEventArgs(string propertyName, object property)
            : base(propertyName, property) { }
    }
}
