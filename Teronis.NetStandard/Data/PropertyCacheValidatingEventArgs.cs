using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class PropertyCacheValidatingEventArgs : PropertyCacheEventArgs
    {
        public bool IsPropertyCacheable { get; set; }

        public PropertyCacheValidatingEventArgs(string propertyName, object property)
            : base(propertyName, property) { }
    }
}
