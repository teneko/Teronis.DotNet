using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class PropertyCacheAddingEventArgs<TProperty> : PropertyCacheEventArgs<TProperty>
    {
        public bool IsPropertyCacheable { get; set; }

        public PropertyCacheAddingEventArgs(string propertyName, TProperty property)
            : base(propertyName, property)
            => IsPropertyCacheable = true;
    }
}
