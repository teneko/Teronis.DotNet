

namespace Teronis.Reflection.Caching
{
    public class PropertyCacheRemovedEventArgs<TProperty> : PropertyCacheEventArgs<TProperty>
    {
        public TProperty OldPropertyValue { get; private set; }

        public PropertyCacheRemovedEventArgs(string propertyName, TProperty property)
            : base(propertyName, property) { }
    }
}
