using System.Diagnostics.CodeAnalysis;

namespace Teronis.Reflection.Caching
{
    public class PropertyCachedEventArgs<TProperty> : PropertyCacheEventArgs<TProperty>
    {
        [MaybeNull, AllowNull]
        public TProperty RemovedPropertyValue { get; private set; }
        /// Recache means, that <see cref="RemovedPropertyValue"/> has been
        /// removed and <see cref="PropertyValue"/> has been added.
        public bool IsRecache { get; private set; }

        public PropertyCachedEventArgs(string propertyName, TProperty addedPropertyValue)
            : base(propertyName, addedPropertyValue) =>
            RemovedPropertyValue = default;

        public PropertyCachedEventArgs(string propertyName, TProperty addedPropertyValue, TProperty removedPropertyValue)
            : this(propertyName, addedPropertyValue)
        {
            RemovedPropertyValue = removedPropertyValue;
            IsRecache = true;
        }
    }
}
