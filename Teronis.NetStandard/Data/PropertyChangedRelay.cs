using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Teronis.Reflection;
using Teronis.Extensions.NetStandard;

namespace Teronis.Data
{
    public class PropertyChangedRelay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IList<INotifyPropertyChanged> PropertyChangedNotifiers { get; private set; }
        public Type CommonPropertiesContainerType { get; private set; }

        private List<INotifyPropertyChanged> cachedNotifiers;
        private PropertyChangedCache<INotifyPropertyChanged> propertyChangedNotifiersCache;

        /// <summary>
        /// Relay upcoming property changes that are in common with members of an instance of <see cref="commonPropertiesContainerType"/>.
        /// </summary>
        /// <param name="commonPropertiesContainerType">Only the members of an instance of <see cref="CommonPropertiesContainerType"/> are going to relay</param>
        /// <param name="propertyChangedNotifiers">In most scenarios, the property changed notifiers (instances of <see cref="INotifyPropertyChanged"/>) are children of an instance of <see cref="CommonPropertiesContainerType"/>.</param>
        public PropertyChangedRelay(Type commonPropertiesContainerType, params INotifyPropertyChanged[] propertyChangedNotifiers)
        {
            cachedNotifiers = new List<INotifyPropertyChanged>();
            PropertyChangedNotifiers = propertyChangedNotifiers;
            CommonPropertiesContainerType = commonPropertiesContainerType ?? throw new ArgumentNullException(nameof(commonPropertiesContainerType));

            if (propertyChangedNotifiers != null)
                foreach (var propertyChangedNotifier in propertyChangedNotifiers)
                    SubscribePropertyChangedNotifier(propertyChangedNotifier);
        }

        /// <summary>
        /// Relay upcoming property changes, whose containers are un-/subscribing automatically, that are in common with members of an instance of <see cref="commonPropertiesContainerType"/>.
        /// </summary>
        public PropertyChangedRelay(INotifyPropertyChanged commonPropertiesContainerNotifier)
            : this(commonPropertiesContainerNotifier?.GetType())
        {
            propertyChangedNotifiersCache = new PropertyChangedCache<INotifyPropertyChanged>(commonPropertiesContainerNotifier);
            propertyChangedNotifiersCache.PropertyCacheAdded += PropertyChangedNotifiersCache_PropertyCacheAdded;
            propertyChangedNotifiersCache.PropertyCacheRemoved += PropertyChangedNotifiersCache_PropertyCacheRemoved;
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(sender, e);

        private void Property_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => OnPropertyChanged(sender, e);

        public void SubscribePropertyChangedNotifier(INotifyPropertyChanged propertyChangedNotifier)
        {
            propertyChangedNotifier.PropertyChanged += PropertyChangedNotifier_PropertyChanged;
            cachedNotifiers.Add(propertyChangedNotifier);
        }

        public void UnsubscribePropertyChangedNotifier(INotifyPropertyChanged propertyChangedNotifier)
        {
            propertyChangedNotifier.PropertyChanged -= PropertyChangedNotifier_PropertyChanged;
            cachedNotifiers.Remove(propertyChangedNotifier);
        }

        private void PropertyChangedNotifiersCache_PropertyCacheAdded(object sender, PropertyCacheAddedEventArgs<INotifyPropertyChanged> args)
            => SubscribePropertyChangedNotifier(args.Property);

        private void PropertyChangedNotifiersCache_PropertyCacheRemoved(object sender, PropertyCacheRemovedEventArgs<INotifyPropertyChanged> args)
            => UnsubscribePropertyChangedNotifier(args.Property);

        private void PropertyChangedNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertySettings = VariableInfoSettings.DefaultFlags | System.Reflection.BindingFlags.GetProperty;
            var shouldNotNotifyUncommonProperty = CommonPropertiesContainerType != null && CommonPropertiesContainerType.GetProperty(e.PropertyName, propertySettings) == null;

            if (shouldNotNotifyUncommonProperty)
                return;

            OnPropertyChanged(sender, e);
        }
    }
}
