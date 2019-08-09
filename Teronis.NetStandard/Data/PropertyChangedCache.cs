using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Teronis.Extensions.NetStandard;
using Teronis.Reflection;

namespace Teronis.Data
{
    public class PropertyChangedCache<TProperty>
    {
        public event PropertyCacheAddingEvent<TProperty> PropertyCacheAdding;
        public event PropertyCacheAddedEvent<TProperty> PropertyCacheAdded;
        public event PropertyCacheRemovedEvent<TProperty> PropertyCacheRemoved;

        public Type PropertyType { get; set; }
        public INotifyPropertyChanged PropertyChangedNotifier { get; private set; }
        public object PropertyChangedRelayTarget { get; private set; }
        public Type PropertyChangedRelayTargetType { get; private set; }
        public EqualityComparer<TProperty> PropertyValueEqualityComparer { get; private set; }
        public IReadOnlyCollection<TProperty> CachedProperties => cachedProperties.Values;

        private Dictionary<string, TProperty> cachedProperties;

        public PropertyChangedCache(INotifyPropertyChanged propertyChangedNotifier, object propertyChangedRelayTarget, EqualityComparer<TProperty> propertyValueEqualityComparer)
        {
            propertyChangedNotifier = propertyChangedNotifier ?? throw new ArgumentNullException(nameof(propertyChangedNotifier));
            propertyChangedNotifier.PropertyChanged += PropertyChangedRelay_PropertyChanged;

            propertyChangedRelayTarget = propertyChangedRelayTarget
                ?? throw new ArgumentNullException(nameof(propertyChangedRelayTarget));

            cachedProperties = new Dictionary<string, TProperty>();
            PropertyType = typeof(TProperty);
            PropertyChangedNotifier = propertyChangedNotifier;
            PropertyChangedRelayTarget = propertyChangedRelayTarget;
            PropertyChangedRelayTargetType = propertyChangedRelayTarget.GetType();
            PropertyValueEqualityComparer = propertyValueEqualityComparer ?? EqualityComparer<TProperty>.Default;
        }

        public PropertyChangedCache(INotifyPropertyChanged propertyChangedNotifier, EqualityComparer<TProperty> propertyValueEqualityComparer)
            : this(propertyChangedNotifier, propertyChangedNotifier, propertyValueEqualityComparer) { }

        public PropertyChangedCache(INotifyPropertyChanged propertyChangedNotifier)
            : this(propertyChangedNotifier, default) { }

        protected void OnPriorPropertyCacheValidating(PropertyCacheAddingEventArgs<TProperty> args)
            => PropertyCacheAdding?.Invoke(this, args);

        protected void OnPropertyCacheAdded(PropertyCacheAddedEventArgs<TProperty> args)
            => PropertyCacheAdded?.Invoke(this, args);

        protected void OnPropertyCacheRemoved(PropertyCacheRemovedEventArgs<TProperty> args)
            => PropertyCacheRemoved?.Invoke(this, args);

        private void PropertyChangedRelay_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;

            var propertySettings = new VariableInfoSettings();
            propertySettings.Flags |= BindingFlags.NonPublic | BindingFlags.GetProperty;

            var propertyValue = PropertyChangedRelayTargetType
                .GetPropertyMember(propertyName, propertySettings)?
                .GetValue(PropertyChangedRelayTarget);

            var typedPropertyValue = default(TProperty);

            if (propertyValue != null && PropertyType.IsAssignableFrom(propertyValue.GetType()))
                typedPropertyValue = (TProperty)propertyValue;

            void cacheProperty(TProperty uncachedProperty)
            {
                var args = new PropertyCacheAddedEventArgs<TProperty>(propertyName, uncachedProperty);
                OnPropertyCacheAdded(args);
                cachedProperties.Add(propertyName, uncachedProperty);
            }

            void uncacheProperty(TProperty cachedProperty)
            {
                var args = new PropertyCacheRemovedEventArgs<TProperty>(propertyName, cachedProperty);
                OnPropertyCacheRemoved(args);
                cachedProperties.Remove(propertyName);
            }

            if (typedPropertyValue != null && !cachedProperties.ContainsKey(propertyName)) {
                var args = new PropertyCacheAddingEventArgs<TProperty>(propertyName, typedPropertyValue);
                OnPriorPropertyCacheValidating(args);

                if (!args.IsPropertyCacheable)
                    return;

                // Add new subscription
                cacheProperty(typedPropertyValue);
            } else if (cachedProperties.ContainsKey(propertyName)) {
                var cachedNotifier = cachedProperties[propertyName];

                if (typedPropertyValue == null)
                    uncacheProperty(cachedNotifier);
                else if (!PropertyValueEqualityComparer.Equals(typedPropertyValue, cachedNotifier)) {
                    uncacheProperty(cachedNotifier);
                    cacheProperty(typedPropertyValue);
                }
            }
        }
    }
}
