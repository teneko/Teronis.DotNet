using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class PropertyCacheEventArgs 
    {
        public string PropertyName { get; private set; }
        public object Property { get; private set; }

        public PropertyCacheAddedEventArgs(string propertyName, object property)
        {
            PropertyName = propertyName;
            Property = property;
        }
    }
}
