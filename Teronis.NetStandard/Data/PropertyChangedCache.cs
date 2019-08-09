using System;
using System.Collections.Generic;
using System.ComponentModel;
using Teronis.Extensions.NetStandard;

namespace Teronis.Data
{
    public class PropertyChangedCache
    {
        public event PriorPropertyCacheValidatingEvent PriorPropertyCacheValidating;
        public event PropertyCacheAddedEvent PropertyCacheAdded;
        public event PropertyCacheRemovedEvent PropertyCacheRemoved;

        public PropertyChangedRelay PropertyChangedRelay { get; private set; }
        public object PropertyChangedRelayTarget { get; private set; }
        public Type PropertyChangedRelayTargetType { get; private set; }

        private Dictionary<string, object> cachedProperties;

        public PropertyChangedCache(PropertyChangedRelay propertyChangedRelay, object propertyChangedRelayTarget)
        {
            cachedProperties = new Dictionary<string, object>();
            propertyChangedRelay.PropertyChanged += PropertyChangedRelay_PropertyChanged;
            PropertyChangedRelay = propertyChangedRelay;

            PropertyChangedRelayTarget = propertyChangedRelayTarget 
                ?? propertyChangedRelay.CommonPropertiesContainerNotifier
                ?? throw new ArgumentNullException(nameof(propertyChangedRelayTarget), $"{nameof(propertyChangedRelay)} was null and {nameof(propertyChangedRelay)}.{nameof(PropertyChangedRelay.CommonPropertiesContainerNotifier)} (backup) was null too.");

            PropertyChangedRelayTargetType = PropertyChangedRelayTarget.GetType();
        }

        protected void OnPriorPropertyCacheValidating(PropertyCacheValidatingEventArgs args)
            => PriorPropertyCacheValidating?.Invoke(this, args);

        protected void OnPropertyCacheAdded(PropertyCacheAddedEventArgs args)
            => PropertyCacheAdded?.Invoke(this, args);

        protected void OnPropertyCacheRemoved(PropertyCacheRemovedEventArgs args)
            => PropertyCacheRemoved?.Invoke(this, args);

        private void PropertyChangedRelay_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            var property = PropertyChangedRelayTargetType.GetVariableMember(propertyName).GetValue(this);
            bool isPropertyCacheable;

            if (!cachedProperties.ContainsKey(propertyName))
            {
                var args = new PropertyCacheValidatingEventArgs(propertyName, property);
                OnPriorPropertyCacheValidating(args);
                isPropertyCacheable = args.IsPropertyCacheable;
            }
            else
                isPropertyCacheable = true;

            if (!isPropertyCacheable)
                return;

            void cacheProperty(object uncachedProperty)
            {
                var args = new PropertyCacheAddedEventArgs(propertyName, uncachedProperty);
                OnPropertyCacheAdded(args);
                cachedProperties.Add(propertyName, uncachedProperty);
            }

            void uncacheProperty(object cachedProperty)
            {
                var args = new PropertyCacheRemovedEventArgs(propertyName, cachedProperty);
                OnPropertyCacheRemoved(args);
                cachedProperties.Remove(propertyName);
            }

            if (property != null && !cachedProperties.ContainsKey(propertyName))
            {
                // Add new subscription
                cacheProperty(property);
            }
            else if (cachedProperties.ContainsKey(propertyName))
            {
                var cachedNotifier = cachedProperties[propertyName];

                if (property == null)
                    uncacheProperty(cachedNotifier);
                else if (property != cachedNotifier)
                {
                    uncacheProperty(cachedNotifier);
                    cacheProperty(property);
                }
            }
        }
    }
}
