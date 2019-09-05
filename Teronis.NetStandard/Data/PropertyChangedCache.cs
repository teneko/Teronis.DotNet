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

        /// <summary>
        /// If true, then only properties are added to the cache whose property 
        /// values are not equals <see cref="PropertyDefaultValue"/>, and cached 
        /// properties are removed and not recached whose property values 
        /// are equals <see cref="PropertyDefaultValue"/>.
        /// </summary>
        public bool CanHandleDefaultValue { get; set; }

        public TProperty PropertyDefaultValue { get; set; }
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
            PropertyDefaultValue = default;
            CanHandleDefaultValue = true;
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

            var propertyType = propertyMember.GetVariableType();

            if ((propertyComparisonType == PropertyComparisonType.ValueType && PropertyType == propertyType)
                || (propertyComparisonType == PropertyComparisonType.ReferenceType && PropertyType.IsAssignableFrom(propertyType)))
                typedPropertyValue = (TProperty)propertyValue;
            else
                return; // The type of the property does meet the requirements, so we skip the property

            var doesCachedPropertiesContainPropertyName = cachedProperties.ContainsKey(propertyName);

            if (!doesCachedPropertiesContainPropertyName
                && (CanHandleDefaultValue || false) && PropertyValueEqualityComparer.Equals(typedPropertyValue, PropertyDefaultValue))
                return;

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
                cachedProperties.Remove(propertyName);

                if (isRecache && CanSkipRemovedEventInvocationWhenRecaching)
                    return;

                var args = new PropertyCacheRemovedEventArgs<TProperty>(propertyName, cachedProperty);
                OnPropertyCacheRemoved(args);
            }

            void recacheProperty(TProperty cachedProperty, TProperty uncachedProperty)
            {
                uncacheProperty(cachedProperty, true);
                var args = new PropertyCacheAddedEventArgs<TProperty>(propertyName, uncachedProperty, cachedProperty);
                cachePropertyViaArgs(args);
            }

            if (!doesCachedPropertiesContainPropertyName)
            {
                var args = new PropertyCacheAddingEventArgs<TProperty>(propertyName, typedPropertyValue);
                OnPropertyCacheAdding(args);

                if (!args.IsPropertyCacheable)
                    return;

                // Add new subscription
                cacheProperty(typedPropertyValue);
            }
            else
            {
                var cachedProperty = cachedProperties[propertyName];

                if (CanHandleDefaultValue && PropertyValueEqualityComparer.Equals(typedPropertyValue, PropertyDefaultValue))
                    uncacheProperty(cachedProperty, false);
                if (!PropertyValueEqualityComparer.Equals(typedPropertyValue, cachedProperty))
                    recacheProperty(cachedProperty, typedPropertyValue);
                else
                    // Both values are equal, so we don't need to uncache
                    return;
            }
        }

        private void PropertyChangedNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => OnPropertyChangedNotifierPropertyChanged(e.PropertyName);
    }
}
