using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Teronis.Extensions;

namespace Teronis.Reflection.Caching
{
    /// <summary>
    /// This class does provide a cache of values of type <see cref="PropertyType"/> 
    /// which are owned by <see cref="SingleTypedPropertiesOwner"/>.
    /// </summary>
    /// <typeparam name="PropertyType"></typeparam>
    public class SingleTypePropertyCache<PropertyType>
    {
        public event PropertyCachingEvent<PropertyType>? PropertyAdding;
        public event PropertyCachedEvent<PropertyType>? PropertyAdded;
        public event PropertyCacheRemovedEvent<PropertyType>? PropertyRemoved;

        public Type TrackingPropertyType { get; set; }
        /// <summary>
        /// If true, then only properties are added to the cache whose property
        /// values are not equals <see cref="TrackingPropertyDefaultValue"/> and
        /// cached properties are removed and not recached whose property values 
        /// are equals <see cref="TrackingPropertyDefaultValue"/>.
        /// </summary>
        public bool CanHandleDefaultValue { get; set; }
        [AllowNull, MaybeNull]
        public PropertyType TrackingPropertyDefaultValue { get; set; }
        public INotifyPropertyChanged SingleTypedPropertyNotifier { get; private set; }
        public object SingleTypedPropertiesOwner { get; private set; }
        public Type SingleTypedPropertiesOwnerType { get; private set; }
        public IEqualityComparer<PropertyType> PropertyValueEqualityComparer { get; private set; }
        public ReadOnlyDictionary<string, PropertyType> CachedPropertyValues { get; private set; }
        /// <summary>
        /// Used when it is tried to get a variable member of <see cref="SingleTypedPropertiesOwnerType"/> by its name.
        /// </summary>
        public VariableInfoDescriptor VariableInfoDescriptor { get; }
        /// <summary>
        /// It does skip the invocation of the property removed event. It may help
        /// if you can handle the retrack in the property added event.
        /// </summary>
        public bool CanSkipPropertyRemovedEventInvocationWhenRetracking { get; set; }

        private readonly Dictionary<string, PropertyType> cachedPropertyValues;
        private readonly PropertyComparisonMode propertyComparisonMode;

        public SingleTypePropertyCache(INotifyPropertyChanged singleTypedPropertyNotifier, object singleTypedPropertiesOwner, IEqualityComparer<PropertyType>? propertyValueEqualityComparer)
        {
            singleTypedPropertyNotifier = singleTypedPropertyNotifier
                ?? throw new ArgumentNullException(nameof(singleTypedPropertyNotifier));

            singleTypedPropertiesOwner = singleTypedPropertiesOwner
                ?? throw new ArgumentNullException(nameof(singleTypedPropertiesOwner));

            propertyValueEqualityComparer ??= EqualityComparer<PropertyType>.Default;

            TrackingPropertyType = typeof(PropertyType);
            var propertyTypeInfo = TrackingPropertyType.GetTypeInfo();

            if (propertyTypeInfo.IsValueType) {
                propertyComparisonMode = PropertyComparisonMode.ValueType;
            } else {
                propertyComparisonMode = PropertyComparisonMode.ReferenceType;
            }

            cachedPropertyValues = new Dictionary<string, PropertyType>();
            CachedPropertyValues = new ReadOnlyDictionary<string, PropertyType>(cachedPropertyValues);
            VariableInfoDescriptor = new VariableInfoDescriptor();
            VariableInfoDescriptor.Flags |= BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.GetField;
            TrackingPropertyDefaultValue = default;
            CanHandleDefaultValue = true;

            SingleTypedPropertyNotifier = singleTypedPropertyNotifier;
            SingleTypedPropertyNotifier.PropertyChanged += SingleTypePropertyNotifier_PropertyChanged;
            SingleTypedPropertiesOwner = singleTypedPropertiesOwner;
            SingleTypedPropertiesOwnerType = singleTypedPropertiesOwner.GetType();
            PropertyValueEqualityComparer = propertyValueEqualityComparer;
        }

        public SingleTypePropertyCache(INotifyPropertyChanged singleTypedPropertyNotifier, object singleTypedPropertiesOwner)
            : this(singleTypedPropertyNotifier, singleTypedPropertiesOwner, default) { }

        public SingleTypePropertyCache(INotifyPropertyChanged singleTypedPropertyNotifierAndOwner, IEqualityComparer<PropertyType>? propertyValueEqualityComparer)
            : this(singleTypedPropertyNotifierAndOwner, singleTypedPropertyNotifierAndOwner, propertyValueEqualityComparer) { }

        public SingleTypePropertyCache(INotifyPropertyChanged singleTypedPropertyNotifierAndOwner)
            : this(singleTypedPropertyNotifierAndOwner, default(IEqualityComparer<PropertyType>)) { }

        protected void OnPropertyCacheAdding(PropertyCachingEventArgs<PropertyType> args)
            => PropertyAdding?.Invoke(this, args);

        protected void OnPropertyCacheAdded(PropertyCachedEventArgs<PropertyType> args)
            => PropertyAdded?.Invoke(this, args);

        protected void OnPropertyCacheRemoved(PropertyCacheRemovedEventArgs<PropertyType> args)
            => PropertyRemoved?.Invoke(this, args);

        /// <summary>
        /// When calling it, you are about to trigger a change of a property.
        /// </summary>
        /// <param name="propertyName"></param>
        public void SingleTypePropertyNotifierPropertyChanged(string propertyName)
        {
            var propertyMember = SingleTypedPropertiesOwnerType.GetVariableMember(propertyName, VariableInfoDescriptor);

            // There is no member that can be handled, so we return.
            if (propertyMember == null) {
                return;
            }

            var propertyType = propertyMember.GetVariableType();
            PropertyType typedPropertyValue;
            var propertyValue = propertyMember.GetValue(SingleTypedPropertiesOwner);

            // We want to assign property value (object) to typed property value. 
            if (propertyComparisonMode == PropertyComparisonMode.ValueType && TrackingPropertyType == propertyType
                || propertyComparisonMode == PropertyComparisonMode.ReferenceType && TrackingPropertyType.IsAssignableFrom(propertyType)) {
                typedPropertyValue = (PropertyType)propertyValue;
            } else {
                return; // The type of the property does not meet the requirements, so we skip the property
            }

            var isPropertNameTracked = cachedPropertyValues.ContainsKey(propertyName);

            // We want to abort if tracking property name is untracked and tracking property value is equals default/null.
            if (!isPropertNameTracked
                && CanHandleDefaultValue
                && PropertyValueEqualityComparer.Equals(typedPropertyValue!, TrackingPropertyDefaultValue!)) {
                return;
            }

            void trackPropertyViaArgs(PropertyCachedEventArgs<PropertyType> args)
            {
                cachedPropertyValues.Add(propertyName, args.PropertyValue!);
                OnPropertyCacheAdded(args);
            }

            void trackProperty(PropertyType uncachedProperty)
            {
                var args = new PropertyCachedEventArgs<PropertyType>(propertyName, uncachedProperty);
                trackPropertyViaArgs(args);
            }

            void untrackProperty(PropertyType cachedProperty, bool isRetrack)
            {
                cachedPropertyValues.Remove(propertyName);

                if (isRetrack && CanSkipPropertyRemovedEventInvocationWhenRetracking) {
                    return;
                }

                var args = new PropertyCacheRemovedEventArgs<PropertyType>(propertyName, cachedProperty);
                OnPropertyCacheRemoved(args);
            }

            void retrackProperty(PropertyType cachedProperty, PropertyType uncachedProperty)
            {
                untrackProperty(cachedProperty, true);
                var args = new PropertyCachedEventArgs<PropertyType>(propertyName, uncachedProperty, cachedProperty);
                trackPropertyViaArgs(args);
            }

            if (!isPropertNameTracked) {
                var args = new PropertyCachingEventArgs<PropertyType>(propertyName, typedPropertyValue);
                OnPropertyCacheAdding(args);

                if (!args.CanTrackProperty) {
                    return;
                }

                // Add new subscription
                trackProperty(typedPropertyValue!);
            } else {
                var cachedProperty = cachedPropertyValues[propertyName];

                if (CanHandleDefaultValue && PropertyValueEqualityComparer.Equals(typedPropertyValue!, TrackingPropertyDefaultValue!)) {
                    untrackProperty(cachedProperty, false);
                } else if (!PropertyValueEqualityComparer.Equals(typedPropertyValue!, cachedProperty)) {
                    retrackProperty(cachedProperty, typedPropertyValue!);
                } else {
                    // Both values are equal, so we don't need to uncache
                    return;
                }
            }
        }

        private void SingleTypePropertyNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => SingleTypePropertyNotifierPropertyChanged(e.PropertyName);
    }
}
