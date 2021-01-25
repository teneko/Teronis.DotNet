using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Teronis.Extensions;
using Teronis.Linq.Expressions;
using Teronis.Reflection.Caching;

namespace Teronis.Configuration
{
    /// <summary>
    /// This class creates a shallow copy of a property value from a settings instance.
    /// </summary>
    /// <typeparam name="PropertyType"></typeparam>
    public class CachedSettingsPropertyValue<PropertyType> : ICachedSettingsPropertyValue
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string PropertyName
            => settingsProperty.Name;

        private object? propertyValue {
            get => settings[PropertyName];
            set {
                settings[PropertyName] = value;
            }
        }

        /// <summary>
        /// If <see cref="settings"/> does not implement
        /// <see cref="INotifyPropertyChanged"/> then you
        /// should NOT bind to this property, as this class
        /// won't know about any changes made to the property
        /// of <see cref="settings"/>.
        /// </summary>
        [MaybeNull, AllowNull]
        public PropertyType PropertyValue {
            get => (PropertyType)propertyValue;
            set => propertyValue = value;
        }

        object? ICachedSettingsPropertyValue.PropertyValue {
            get => propertyValue;
            set => propertyValue = value;
        }

        [MaybeNull, AllowNull]
        public PropertyType CachedPropertyValue {
            get => cachedProperty;

            private set {
                cachedProperty = value;
                OnPropertyChanged();
                recalculateIsCopySynchronous();
            }
        }

        object? ICachedSettingsPropertyValue.CachedPropertyValue
            => CachedPropertyValue;

        public bool IsCacheSynchronous {
            get => isCopySynchronous;

            set {
                if (isCopySynchronous == value) {
                    return;
                }

                isCopySynchronous = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// You may refer to an method that notifies about changes of a 
        /// property of <see cref="settings"/> if it does not implement 
        /// <see cref="INotifyPropertyChanged"/> already.
        /// </summary>
        public Action<string>? NotifySettingsAboutPropertyChange { get; set; }

        public IEqualityComparer<PropertyType> PropertyValueEqualityComparer { get; private set; }

        private SettingsProperty settingsProperty
            => settingsPropertyValue.Property;

        [AllowNull]
        [MaybeNull]
        private PropertyType cachedProperty = default!;
        private readonly SettingsBase settings;
        private readonly SettingsPropertyValue settingsPropertyValue;
        private bool isCopySynchronous;
        /// <summary>
        /// The tracked properties are restricted by the name of the 
        /// property. So effectively one property gets ever tracked.
        /// </summary>
        private readonly SingleTypePropertyCache<PropertyType>? propertyChangedCache;

        public CachedSettingsPropertyValue(SettingsBase settings, string name, IEqualityComparer<PropertyType>? propertyValueEqualityComparer)
        {
            this.settings = settings
                ?? throw new ArgumentNullException(nameof(settings));

            settingsPropertyValue = settings.PropertyValues[name]
                ?? throw new ArgumentException($"Property '{name}' is not member of {nameof(settings)}.");

            var genericPropertyType = typeof(PropertyType);

            if (genericPropertyType != settingsProperty.PropertyType && !typeof(PropertyType).IsAssignableFrom(settingsProperty.PropertyType)) {
                throw new ArgumentException($"The types are not the same.");
            }

            PropertyValueEqualityComparer = propertyValueEqualityComparer
                ?? EqualityComparer<PropertyType>.Default;

            // We want the property value of the settings copied to the cache.
            RefreshCachedValue();

            if (settings is INotifyPropertyChanged settingsPropertyChangedNotifier) {
                propertyChangedCache = new SingleTypePropertyCache<PropertyType>(settingsPropertyChangedNotifier) {
                    CanSkipPropertyRemovedEventInvocationWhenRetracking = true, // We benefit from not calculating twice during recache (remove/add)
                    CanHandleDefaultValue = false
                };

                propertyChangedCache.PropertyAdding += PropertyChangedCache_PropertyCacheAdding;
                propertyChangedCache.PropertyAdded += PropertyChangedCache_PropertyCacheAdded;
                propertyChangedCache.PropertyRemoved += PropertyChangedCache_PropertyCacheRemoved;
                propertyChangedCache.SingleTypePropertyNotifierPropertyChanged(PropertyName);

                if (propertyChangedCache.CachedPropertyValues.Count == 0) {
                    throw new Exception($"The property {PropertyName} has not been found.");
                }
            }
        }

        public CachedSettingsPropertyValue(SettingsBase settings, string name)
            : this(settings, name, default) { }

        private void PropertyChangedCache_PropertyCacheAdding(object sender, PropertyCachingEventArgs<PropertyType> args)
            => args.CanTrackProperty = args.PropertyName == PropertyName;

        private void recalculateIsCopySynchronous()
        {
            if (PropertyValueEqualityComparer == null) {
                return;
            }

            var isPropertyChangedCacheNotNull = !(propertyChangedCache is null);

            var doesCachedPropertyExist = isPropertyChangedCacheNotNull
                && propertyChangedCache!.CachedPropertyValues.ContainsKey(PropertyName);

            if (isPropertyChangedCacheNotNull && !doesCachedPropertyExist) {
                IsCacheSynchronous = false;
            } else {
                PropertyType cachedProperty;

                if (doesCachedPropertyExist) {
                    cachedProperty = propertyChangedCache!.CachedPropertyValues[PropertyName];
                } else {
                    cachedProperty = PropertyValue;
                }

                IsCacheSynchronous = PropertyValueEqualityComparer.Equals(CachedPropertyValue!, cachedProperty!);
            }
        }

        private void SettingsPropertyNotifier_Changed(object sender, PropertyChangedEventArgs e)
            => recalculateIsCopySynchronous();

        private void SettingsPropertyCollectionNotifier_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => recalculateIsCopySynchronous();

        private void propertyChangedCache_PropertyCacheRemoved(PropertyType settingsProperty)
        {
            if (settingsProperty is INotifyPropertyChanged settingsPropertyNotifier) {
                settingsPropertyNotifier.PropertyChanged -= SettingsPropertyNotifier_Changed;
            }

            if (settingsProperty is INotifyCollectionChanged settingsPropertyCollectionNotifier) {
                settingsPropertyCollectionNotifier.CollectionChanged -= SettingsPropertyCollectionNotifier_CollectionChanged;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void PropertyChangedCache_PropertyCacheAdded(object sender, PropertyCachedEventArgs<PropertyType> args)
        {
            // When it is a recache, then first we have to remove the attached handlers we added before
            if (args.IsRecache) {
                var removedPropertyValue = args.RemovedPropertyValue ??
                    throw ShortException.ArgumentNullException(() => args.RemovedPropertyValue);

                propertyChangedCache_PropertyCacheRemoved(args.RemovedPropertyValue);
            }

            Debug.WriteLine($"Property '{PropertyName}' has been added to the cache");

            var settingsProperty = args.PropertyValue;

            if (settingsProperty is INotifyPropertyChanged settingsPropertyNotifier) {
                settingsPropertyNotifier.PropertyChanged += SettingsPropertyNotifier_Changed;
            }

            if (settingsProperty is INotifyCollectionChanged settingsPropertyCollectionNotifier) {
                settingsPropertyCollectionNotifier.CollectionChanged += SettingsPropertyCollectionNotifier_CollectionChanged;
            }

            recalculateIsCopySynchronous();
            OnPropertyChanged(nameof(PropertyValue));
        }

        private void PropertyChangedCache_PropertyCacheRemoved(object sender, PropertyCacheRemovedEventArgs<PropertyType> args)
        {
            var propertyValue = args.PropertyValue ?? throw ShortException.ArgumentNullException(() => args.PropertyValue);
            propertyChangedCache_PropertyCacheRemoved(args.PropertyValue);
            recalculateIsCopySynchronous();
        }

        [return: MaybeNull]
        private PropertyType copySettingsPropertyDefaultValue() =>
            settingsPropertyValue.CopyDefaultValue<PropertyType>();

        [return: MaybeNull]
        private PropertyType copySettingsPropertyValue() =>
            settingsPropertyValue.CopyPropertyValue<PropertyType>();

        public virtual void ResetCachedValue() =>
            CachedPropertyValue = copySettingsPropertyDefaultValue();

        public virtual void RefreshCachedValue() =>
            CachedPropertyValue = copySettingsPropertyValue();

        /// <summary>
        /// Notifies <see cref="settings"/> about changes from settings property "<see cref="PropertyName"/>".
        /// </summary>
        public void TriggerSettingsPropertyChanged()
        {
            if (NotifySettingsAboutPropertyChange != null) {
                NotifySettingsAboutPropertyChange?.Invoke(PropertyName);
            } else {
                propertyValue = propertyValue;
            }
        }

        /// <summary>
        /// It removes property value from cache and notifies <see cref="settings"/> about changes from settings property "<see cref="PropertyName"/>".
        /// </summary>
        public void SetOriginalFromDisk()
        {
            settings.PropertyValues.Remove(PropertyName);
            TriggerSettingsPropertyChanged();
        }

        private void setOriginal(object? source)
        {
            propertyValue = source;
            TriggerSettingsPropertyChanged();
        }

        public void SetOriginalFromCache()
        {
            var copiedCache = settingsPropertyValue.CopyValue(CachedPropertyValue, true);
            setOriginal(copiedCache);
        }

        public void SetOriginalFromDefault()
        {
            var defaultValue = copySettingsPropertyDefaultValue();
            setOriginal(defaultValue);
        }

        /// <summary>
        /// Reminder: <see cref="CachedPropertyValue"/> gets renewed by calling <see cref="SaveToDisk"/>.
        /// </summary>
        public void SaveToDisk()
        {
            settings.Save();
            RefreshCachedValue();
            TriggerSettingsPropertyChanged();
        }

        internal static partial class ShortException
        {
            public static ArgumentNullException ArgumentNullException(Expression<Func<object?>> propertySelector, string? message = null)
            {
                var propertyName = ExpressionTools.GetReturnName(propertySelector);
                return new ArgumentNullException(propertyName, message);
            }
        }
    }
}
