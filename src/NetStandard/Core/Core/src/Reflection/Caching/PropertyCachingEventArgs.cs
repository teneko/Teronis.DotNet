

using System.Diagnostics.CodeAnalysis;

namespace Teronis.Reflection.Caching
{
    public class PropertyCachingEventArgs<TProperty> : PropertyCacheEventArgs<TProperty>
    {
        public bool CanTrackProperty { get; set; }

        public PropertyCachingEventArgs(string propertyName, [AllowNull]TProperty property)
            : base(propertyName, property)
            => CanTrackProperty = true;
    }
}
