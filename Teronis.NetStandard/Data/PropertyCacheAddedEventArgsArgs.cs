using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class PropertyCacheAddedEventArgs : PropertyCacheEventArgs
    {
        public PropertyCacheAddedEventArgs(string propertyName, object property)
            : base(propertyName, property) { }
    }
}
