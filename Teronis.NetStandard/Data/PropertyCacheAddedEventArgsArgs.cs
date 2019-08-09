using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class PropertyCacheAddedEventArgs<TProperty> : PropertyCacheEventArgs<TProperty>
    {
        public PropertyCacheAddedEventArgs(string propertyName, TProperty property)
            : base(propertyName, property) { }
    }
}
