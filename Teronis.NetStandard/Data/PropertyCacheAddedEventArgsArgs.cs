using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class PropertyCacheAddedEventArgs<TProperty> 
    {
        public TProperty AddedProperty 
            => propertyCacheArgs.Property;

        public TProperty RemovedProperty { get; private set; }
        public bool IsRecache { get; private set; }

        private PropertyCacheEventArgs<TProperty> propertyCacheArgs;

        public PropertyCacheAddedEventArgs(string propertyName, TProperty property)
            => propertyCacheArgs = new PropertyCacheEventArgs<TProperty>(propertyName, property);

        public PropertyCacheAddedEventArgs(string propertyName, TProperty addedProperty, TProperty removedProperty)
            : this(propertyName, addedProperty)
        {
            RemovedProperty = removedProperty;
            IsRecache = true;
        }
    }
}
