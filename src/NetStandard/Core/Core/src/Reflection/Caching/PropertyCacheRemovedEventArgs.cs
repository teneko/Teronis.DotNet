

namespace Teronis.Reflection.Caching
{
    public class PropertyCacheRemovedEventArgs<TProperty> : PropertyCacheEventArgs<TProperty>
    {
        public PropertyCacheRemovedEventArgs(string propertyName, TProperty property)
            : base(propertyName, property) { }
    }
}
