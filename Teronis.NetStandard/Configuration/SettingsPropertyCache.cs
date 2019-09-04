using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Teronis.Data;

namespace Teronis.Configuration
{
    public class SettingsPropertyCache<PropertyType> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; private set; }

        public bool IsCopySynchronous {
            get => isCopySynchronous;

            set {
                isCopySynchronous = value;
                OnPropertyChanged();
            }
        }

        public PropertyType Copy {
            get => copy;

            private set {
                copy = value;
                OnPropertyChanged();
                recalculateIsCopySynchronous();
            }
        }

        /// <summary>
        /// You can partial extend auto generated settings class and provide a method that calls 
        /// <see cref="ApplicationSettingsBase.OnPropertyChanged(object, PropertyChangedEventArgs)"/>.
        /// You may then refer to this method.
        /// </summary>
        public Action<string> NotifySettingsAboutPropertyChange { get; set; }

        public IEqualityComparer<PropertyType> PropertyEqualityComparer { get; set; }

        private PropertyType copy;
        private SettingsBase settings;
        private SettingsProperty property;
        private PropertyChangedCache<PropertyType> propertyChangedCache;
        private bool isCopySynchronous;

        protected SettingsPropertyCache(SettingsBase settings, string name, bool refreshCopy)
        {
            this.settings = settings
                ?? throw new ArgumentNullException(nameof(settings));

            Name = name;

            property = settings.Properties[Name]
                ?? throw new ArgumentException($"Property '{Name}' is not member of {nameof(settings)}");

            var genericPropertyType = typeof(PropertyType);

            if (genericPropertyType != property.PropertyType && !typeof(PropertyType).IsAssignableFrom(property.PropertyType))
                throw new ArgumentException($"The types aren't the same");

            if (refreshCopy)
                RefreshCopy();
        }

        public SettingsPropertyCache(SettingsBase settings, string name)
            : this(settings, name, true) { }

        public SettingsPropertyCache(ApplicationSettingsBase settings, string name, IEqualityComparer<PropertyType> propertyEqualityComparer)
            : this(settings, name, false)
        {
            PropertyEqualityComparer = propertyEqualityComparer
                ?? throw new ArgumentNullException(nameof(propertyEqualityComparer));

            // Now we have the equality comparer
            RefreshCopy();

            if (property.PropertyType.IsValueType)
                throw new NotSupportedException("The property type is a value type and therefore not supported to be observed");

            propertyChangedCache = new PropertyChangedCache<PropertyType>(settings)
            {
                CanSkipRemovedEventInvocationWhenRecaching = true
            };

            propertyChangedCache.PropertyCacheAdding += PropertyChangedCache_PropertyCacheAdding;
            propertyChangedCache.PropertyCacheAdded += PropertyChangedCache_PropertyCacheAdded;
            propertyChangedCache.PropertyCacheRemoved += PropertyChangedCache_PropertyCacheRemoved;
            propertyChangedCache.OnPropertyChangedNotifierPropertyChanged(Name);
        }

        private void PropertyChangedCache_PropertyCacheAdding(object sender, PropertyCacheAddingEventArgs<PropertyType> args)
            => args.IsPropertyCacheable = args.PropertyName == Name;

        static int test = 0;

        private void recalculateIsCopySynchronous()
        {
            if (PropertyEqualityComparer == null)
                return;

            Debug.WriteLine(test++);

            var isPropertyChangedCacheNotNull = propertyChangedCache != null;
            var doesCachedPropertyExist = isPropertyChangedCacheNotNull && propertyChangedCache.CachedProperties.ContainsKey(Name);

            if (isPropertyChangedCacheNotNull && !doesCachedPropertyExist)
                IsCopySynchronous = false;
            else
            {
                PropertyType cachedProperty;

                if (doesCachedPropertyExist)
                    cachedProperty = propertyChangedCache.CachedProperties[Name];
                else
                    cachedProperty = (PropertyType)settings[Name];

                IsCopySynchronous = PropertyEqualityComparer.Equals(Copy, cachedProperty);
            }
        }

        private void SettingsPropertyNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => recalculateIsCopySynchronous();

        private void SettingsPropertyCollectionNotifier_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => recalculateIsCopySynchronous();

        private void propertyChangedCache_PropertyCacheRemoved(PropertyType settingsProperty)
        {
            if (settingsProperty is INotifyPropertyChanged settingsPropertyNotifier)
                settingsPropertyNotifier.PropertyChanged -= SettingsPropertyNotifier_PropertyChanged;

            if (settingsProperty is INotifyCollectionChanged settingsPropertyCollectionNotifier)
                settingsPropertyCollectionNotifier.CollectionChanged -= SettingsPropertyCollectionNotifier_CollectionChanged;
        }

        private void PropertyChangedCache_PropertyCacheAdded(object sender, PropertyCacheAddedEventArgs<PropertyType> args)
        {
            if (args.IsRecache)
                propertyChangedCache_PropertyCacheRemoved(args.RemovedProperty);

            Debug.WriteLine($"Property '{Name}' has been added to the cached");

            var settingsProperty = args.AddedProperty;

            if (settingsProperty is INotifyPropertyChanged settingsPropertyNotifier)
                settingsPropertyNotifier.PropertyChanged += SettingsPropertyNotifier_PropertyChanged;

            if (settingsProperty is INotifyCollectionChanged settingsPropertyCollectionNotifier)
                settingsPropertyCollectionNotifier.CollectionChanged += SettingsPropertyCollectionNotifier_CollectionChanged;

            recalculateIsCopySynchronous();
        }

        private void PropertyChangedCache_PropertyCacheRemoved(object sender, PropertyCacheRemovedEventArgs<PropertyType> args)
        {
            propertyChangedCache_PropertyCacheRemoved(args.Property);
            recalculateIsCopySynchronous();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private PropertyType copyValue(object propertyValue)
        {
            var serializer = new XmlSerializer(property.PropertyType);

            using (var stream = new MemoryStream())
            {
                using (var writer = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    serializer.Serialize(writer, propertyValue);
                    stream.Position = 0;
                    var copiedValue = (PropertyType)serializer.Deserialize(stream);
                    return copiedValue;
                }
            }
        }

        private PropertyType copySettingsPropertyDefaultValue()
        {
            var serializer = new XmlSerializer(property.PropertyType);
            bool isDefaultValueCopied = false;
            object defaultPropertyValue = property.DefaultValue;

            if (defaultPropertyValue is string propertyString)
            {
                var bytes = Encoding.UTF8.GetBytes(propertyString);

                using (var stream = new MemoryStream(bytes))
                using (var reader = XmlReader.Create(stream))
                {
                    if (serializer.CanDeserialize(reader))
                    {
                        defaultPropertyValue = serializer.Deserialize(reader);
                        isDefaultValueCopied = true;
                    }
                }
            }

            PropertyType typedDefaultPropertyValue;

            if (isDefaultValueCopied)
                typedDefaultPropertyValue = (PropertyType)defaultPropertyValue;
            else
                typedDefaultPropertyValue = copyValue(defaultPropertyValue);

            return typedDefaultPropertyValue;
        }

        private PropertyType copySettingsPropertyValue()
        {
            var value = settings[Name];
            value = copyValue(value);
            return (PropertyType)value;
        }

        public virtual void RefreshCopy(bool useDefaultValue = false)
        {
            PropertyType propertyValue;

            if (useDefaultValue)
                propertyValue = copySettingsPropertyDefaultValue();
            else
                propertyValue = copySettingsPropertyValue();

            Copy = propertyValue;
        }

        /// <summary>
        /// Notifies <see cref="settings"/> about changes from settings property "<see cref="Name"/>".
        /// </summary>
        public void TriggerSettingsPropertyChanged()
        {
            if (NotifySettingsAboutPropertyChange != null)
                NotifySettingsAboutPropertyChange?.Invoke(Name);
            else
                settings[Name] = settings[Name];
        }

        /// <summary>
        /// It removes property value from cache and notifies <see cref="settings"/> about changes from settings property "<see cref="Name"/>".
        /// </summary>
        public void SetOriginalFromDisk()
        {
            settings.PropertyValues.Remove(Name);
            TriggerSettingsPropertyChanged();
        }

        private void setOriginalFromSource(object source)
        {
            settings[Name] = source;
            TriggerSettingsPropertyChanged();
        }

        public void SetOriginalFromCopy()
        {
            var copiedCopy = copyValue(Copy);
            setOriginalFromSource(copiedCopy);
        }

        public void SetOriginalFromDefault()
        {
            var defaultValue = copySettingsPropertyDefaultValue();
            setOriginalFromSource(defaultValue);
        }

        public void SaveToDisk()
        {
            settings.Save();
            RefreshCopy();
            SetOriginalFromDisk();
        }
    }
}
