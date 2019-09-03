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
        public TProperty DefaultPropertyValue { get; set; }
        public INotifyPropertyChanged PropertyChangedNotifier { get; private set; }
        public object PropertyChangedRelayTarget { get; private set; }
        public Type PropertyChangedRelayTargetType { get; private set; }
        public IEqualityComparer<TProperty> PropertyValueEqualityComparer { get; private set; }
        public ReadOnlyDictionary<string, TProperty> CachedProperties { get; private set; }
        public bool CanSkipRemovedEventInvocationWhenRecaching { get; set; }

        private Dictionary<string, TProperty> cachedProperties;
        private PropertyComparisonType propertyComparisonType;

        public PropertyChangedCache(INotifyPropertyChanged propertyChangedNotifier, object propertyChangedRelayTarget, IEqualityComparer<TProperty> propertyValueEqualityComparer)
        {
            propertyChangedNotifier = propertyChangedNotifier
                ?? throw new ArgumentNullException(nameof(propertyChangedNotifier));

            propertyChangedRelayTarget = propertyChangedRelayTarget
                ?? throw new ArgumentNullException(nameof(propertyChangedRelayTarget));

            propertyValueEqualityComparer = propertyValueEqualityComparer
                ?? EqualityComparer<TProperty>.Default;

            PropertyType = typeof(TProperty);
            var propertyTypeInfo = PropertyType.GetTypeInfo();

            if (propertyTypeInfo.IsValueType)
                propertyComparisonType = PropertyComparisonType.ValueType;
            else
                propertyComparisonType = PropertyComparisonType.ReferenceType;

            cachedProperties = new Dictionary<string, TProperty>();
            CachedProperties = new ReadOnlyDictionary<string, TProperty>(cachedProperties);
            DefaultPropertyValue = default;
            PropertyChangedNotifier = propertyChangedNotifier;
            PropertyChangedNotifier.PropertyChanged += PropertyChangedNotifier_PropertyChanged;
            PropertyChangedRelayTarget = propertyChangedRelayTarget;
            PropertyChangedRelayTargetType = propertyChangedRelayTarget.GetType();
            PropertyValueEqualityComparer = propertyValueEqualityComparer;
        }

        public PropertyChangedCache(INotifyPropertyChanged propertyChangedNotifier, IEqualityComparer<TProperty> propertyValueEqualityComparer)
            : this(propertyChangedNotifier, propertyChangedNotifier, propertyValueEqualityComparer) { }

        public PropertyChangedCache(INotifyPropertyChanged propertyChangedNotifier)
            : this(propertyChangedNotifier, default) { }

        protected void OnPropertyCacheAdding(PropertyCacheAddingEventArgs<TProperty> args)
            => PropertyCacheAdding?.Invoke(this, args);

        protected void OnPropertyCacheAdded(PropertyCacheAddedEventArgs<TProperty> args)
            => PropertyCacheAdded?.Invoke(this, args);

        protected void OnPropertyCacheRemoved(PropertyCacheRemovedEventArgs<TProperty> args)
            => PropertyCacheRemoved?.Invoke(this, args);

        public void OnPropertyChangedNotifierPropertyChanged(string propertyName)
        {
            var propertySettings = new VariableInfoSettings();
            propertySettings.Flags |= BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.GetField;

            var propertyMember = PropertyChangedRelayTargetType
                .GetVariableMember(propertyName, propertySettings);

            // There is no member that can be handled, so we return
            if (propertyMember == null)
                return;

            var propertyValue = propertyMember
                .GetValue(PropertyChangedRelayTarget);

            TProperty typedPropertyValue;

            var propertyVariableType = propertyMember.GetVariableType();

            if ((propertyComparisonType == PropertyComparisonType.ValueType && PropertyType == propertyVariableType)
                || (propertyComparisonType == PropertyComparisonType.ReferenceType && PropertyType.IsAssignableFrom(propertyVariableType)))
                typedPropertyValue = (TProperty)propertyValue;
            else
                typedPropertyValue = DefaultPropertyValue;

            void cachePropertyViaArgs(PropertyCacheAddedEventArgs<TProperty> args)
            {
                cachedProperties.Add(propertyName, args.AddedProperty);
                OnPropertyCacheAdded(args);
            }

            void cacheProperty(TProperty uncachedProperty)
            {
                var args = new PropertyCacheAddedEventArgs<TProperty>(propertyName, uncachedProperty);
                cachePropertyViaArgs(args);
            }

            void uncacheProperty(TProperty cachedProperty, bool isRecache)
            {
                var args = new PropertyCacheRemovedEventArgs<TProperty>(propertyName, cachedProperty);
                cachedProperties.Remove(propertyName);

                if (isRecache && CanSkipRemovedEventInvocationWhenRecaching)
                    OnPropertyCacheRemoved(args);
            }

            void recacheProperty(TProperty cachedProperty, TProperty uncachedProperty)
            {
                uncacheProperty(cachedProperty, true);
                var args = new PropertyCacheAddedEventArgs<TProperty>(propertyName, uncachedProperty, cachedProperty);
                cachePropertyViaArgs(args);
            }

            if (!PropertyValueEqualityComparer.Equals(typedPropertyValue, DefaultPropertyValue) && !cachedProperties.ContainsKey(propertyName))
            {
                var args = new PropertyCacheAddingEventArgs<TProperty>(propertyName, typedPropertyValue);
                OnPropertyCacheAdding(args);

                if (!args.IsPropertyCacheable)
                    return;

                // Add new subscription
                cacheProperty(typedPropertyValue);
            }
            else if (cachedProperties.ContainsKey(propertyName))
            {
                var cachedProperty = cachedProperties[propertyName];

                if (PropertyValueEqualityComparer.Equals(typedPropertyValue, DefaultPropertyValue))
                    uncacheProperty(cachedProperty, false);
                else if (!PropertyValueEqualityComparer.Equals(typedPropertyValue, cachedProperty))
                    recacheProperty(cachedProperty, typedPropertyValue);
            }
        }

        private void PropertyChangedNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => OnPropertyChangedNotifierPropertyChanged(e.PropertyName);
    }
}
