using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class PropertyCacheRemovedEventArgs<TProperty> : PropertyCacheEventArgs<TProperty>
    {
        public PropertyCacheRemovedEventArgs(string propertyName, TProperty property)
            : base(propertyName, property) { }
    }
}
