using System;
using System.Collections.Generic;
using System.ComponentModel;
using Teronis.Reflection;
using System.Reflection;

namespace Teronis.Data
{
    public class PropertyChangedRelay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler NotifiersPropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add => NotifiersPropertyChanged += value;
            remove => NotifiersPropertyChanged -= value;
        }

        public Dictionary<string, Type> AllowedProperties { get; set; }

        private BindingFlags propertyBindingFlags;
        private List<INotifyPropertyChanged> cachedPropertyChangedNotifiers;
        private PropertyChangedCache<INotifyPropertyChanged> propertyChangedNotifiersCache;

        /// <summary>
        /// The notifying changes are from notifiable containers of <paramref name="propertyChangedNotifiers"/>.
        /// </summary>
        public PropertyChangedRelay(IEnumerable<KeyValuePair<string, Type>> allowedProperties, params INotifyPropertyChanged[] propertyChangedNotifiers)
        {
            onConstruction();
            onInitialization(allowedProperties);
            onInitialization(propertyChangedNotifiers);
        }

        public PropertyChangedRelay(IEnumerable<KeyValuePair<string, Type>> allowedProperties)
        {
            onConstruction();
            onInitialization(allowedProperties);
        }

        /// <summary>
        /// The notifying changes are from notifiable containers of <paramref name="propertyChangedNotifiers"/>.
        /// </summary>
        /// <param name="commonPropertiesContainerType">Only the members of an instance of <see cref="CommonPropertiesContainerType"/> are going to relay</param>
        /// <param name="propertyChangedNotifiers">In most scenarios, the property changed notifiers (instances of <see cref="INotifyPropertyChanged"/>) are children of an instance of <see cref="CommonPropertiesContainerType"/>.</param>
        public PropertyChangedRelay(params INotifyPropertyChanged[] propertyChangedNotifiers)
        {
            onConstruction();
            onInitialization(propertyChangedNotifiers);
        }

        /// <summary>
        /// Relay upcoming property changes that are in common with members of a type of the instance of <see cref="commonPropertiesContainerType"/>.
        /// The notifying changes are from notifiable containers of <paramref name="propertyChangedNotifiers"/>.
        /// </summary>
        /// <param name="commonPropertiesContainerType">Only the members of an instance of <see cref="CommonPropertiesContainerType"/> are going to relay</param>
        /// <param name="propertyChangedNotifiers">In most scenarios, the property changed notifiers (instances of <see cref="INotifyPropertyChanged"/>) are children of an instance of <see cref="CommonPropertiesContainerType"/>.</param>
        public PropertyChangedRelay(Type commonPropertiesContainerType, params INotifyPropertyChanged[] propertyChangedNotifiers)
        {
            onConstruction();
            var propertyInfos = commonPropertiesContainerType.GetProperties(propertyBindingFlags);
            onInitialization(propertyInfos);
            onInitialization(propertyChangedNotifiers);
        }

        /// <summary>
        /// Relay upcoming property changes that are in common with members of a type of the instance of <see cref="commonPropertiesContainerType"/>.
        /// The notifying changes are from notifiable containers of the type of the instance of <paramref name="commonPropertiesContainerNotifier"/> 
        /// that are un-/subscribing automatically.
        /// </summary>
        public PropertyChangedRelay(INotifyPropertyChanged commonPropertiesContainerNotifier)
            : this(commonPropertiesContainerNotifier?.GetType())
        {
            propertyChangedNotifiersCache = new PropertyChangedCache<INotifyPropertyChanged>(commonPropertiesContainerNotifier);

            void PropertyChangedNotifiersCache_PropertyCacheAdded(object sender, PropertyCacheAddedEventArgs<INotifyPropertyChanged> args)
                => SubscribePropertyChangedNotifier(args.AddedProperty);

            propertyChangedNotifiersCache.PropertyCacheAdded += PropertyChangedNotifiersCache_PropertyCacheAdded;

            void PropertyChangedNotifiersCache_PropertyCacheRemoved(object sender, PropertyCacheRemovedEventArgs<INotifyPropertyChanged> args)
                => UnsubscribePropertyChangedNotifier(args.Property);

            propertyChangedNotifiersCache.PropertyCacheRemoved += PropertyChangedNotifiersCache_PropertyCacheRemoved;
        }

        private void onConstruction()
        {
            propertyBindingFlags = VariableInfoSettings.DefaultFlags | BindingFlags.GetProperty;
            cachedPropertyChangedNotifiers = new List<INotifyPropertyChanged>();
        }

        private void onAllowedPropertiesInitialization()
        {
            AllowedProperties = AllowedProperties ?? new Dictionary<string, Type>();
            AllowedProperties.Clear();
        }

        private void onInitialization(IEnumerable<KeyValuePair<string, Type>> allowedProperties)
        {
            onAllowedPropertiesInitialization();
            var allowedPropertyCollection = (ICollection<KeyValuePair<string, Type>>)AllowedProperties;

            foreach (var allowedProperty in allowedProperties)
                allowedPropertyCollection.Add(allowedProperty);
        }

        private void onInitialization(IEnumerable<PropertyInfo> propertyInfos)
        {
            onAllowedPropertiesInitialization();

            foreach (var propertyInfo in propertyInfos)
                AllowedProperties.Add(propertyInfo.Name, propertyInfo.PropertyType);
        }

        private void onInitialization(INotifyPropertyChanged[] propertyChangedNotifiers)
        {
            if (propertyChangedNotifiers != null)
                foreach (var propertyChangedNotifier in propertyChangedNotifiers)
                    SubscribePropertyChangedNotifier(propertyChangedNotifier);
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            => NotifiersPropertyChanged?.Invoke(sender, e);

        private void PropertyChangedNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var shouldNotifyUnknownProperty = AllowedProperties == null || (AllowedProperties.TryGetValue(e.PropertyName, out var propertyType)
                && (propertyType == null || sender.GetType().GetProperty(e.PropertyName, propertyBindingFlags).PropertyType == propertyType));

            if (!shouldNotifyUnknownProperty)
                return;

            OnPropertyChanged(sender, e);
        }

        public void SubscribePropertyChangedNotifier(INotifyPropertyChanged propertyChangedNotifier)
        {
            propertyChangedNotifier.PropertyChanged += PropertyChangedNotifier_PropertyChanged;
            cachedPropertyChangedNotifiers.Add(propertyChangedNotifier);
        }

        public void UnsubscribePropertyChangedNotifier(INotifyPropertyChanged propertyChangedNotifier)
        {
            propertyChangedNotifier.PropertyChanged -= PropertyChangedNotifier_PropertyChanged;
            cachedPropertyChangedNotifiers.Remove(propertyChangedNotifier);
        }
    }
}
