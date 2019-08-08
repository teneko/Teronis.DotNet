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
        public INotifyPropertyChanged CommonPropertiesContainerNotifier { get; private set; }
        public Type CommonPropertiesContainerType { get; private set; }

        private Dictionary<string, INotifyPropertyChanged> cachedPropertyChangedNotifiers;

        public PropertyChangedRelay(Type commonPropertiesContainerType, params INotifyPropertyChanged[] propertyChangedNotifiers)
        {
            PropertyChangedNotifiers = propertyChangedNotifiers ?? throw new ArgumentNullException(nameof(propertyChangedNotifiers));

            foreach (var propertyChangedNotifier in PropertyChangedNotifiers)
                SubscribeNotifier(propertyChangedNotifier);

            CommonPropertiesContainerType = commonPropertiesContainerType;
        }

        public PropertyChangedRelay(INotifyPropertyChanged commonPropertiesContainerNotifier)
            : this(commonPropertiesContainerNotifier?.GetType())
            => CommonPropertiesContainerNotifier = commonPropertiesContainerNotifier;

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(sender, e);

        private void UncachedNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => OnPropertyChanged(sender, e);

        private void PropertyChangedNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var readonlyPropertyBindingAttributes = VariableInfoSettings.DefaultFlags | System.Reflection.BindingFlags.GetProperty;
            var shouldNotNotifyUncommonProperty = CommonPropertiesContainerType != null && CommonPropertiesContainerType.GetProperty(e.PropertyName, readonlyPropertyBindingAttributes) == null;

            if (shouldNotNotifyUncommonProperty)
                return;

            if (CommonPropertiesContainerNotifier != null) {
                var propertyName = e.PropertyName;
                var notifier = CommonPropertiesContainerType.GetVariableMember(e.PropertyName).GetValue(this) as INotifyPropertyChanged;

                void cacheNotifier(INotifyPropertyChanged uncachedNotifier)
                {
                    uncachedNotifier.PropertyChanged += UncachedNotifier_PropertyChanged;
                    cachedPropertyChangedNotifiers.Add(propertyName, uncachedNotifier);
                }

                void uncacheNotifier(INotifyPropertyChanged cachedNotifier)
                {
                    cachedNotifier.PropertyChanged -= UncachedNotifier_PropertyChanged;
                    cachedPropertyChangedNotifiers.Remove(propertyName);
                }

                if (notifier != null && !cachedPropertyChangedNotifiers.ContainsKey(propertyName)) {
                    // Add new subscription
                    cacheNotifier(notifier);
                } else if (cachedPropertyChangedNotifiers.ContainsKey(propertyName)) {
                    var cachedNotifier = cachedPropertyChangedNotifiers[propertyName];

                    if (notifier == null)
                        uncacheNotifier(cachedNotifier);
                    else if (notifier != cachedNotifier) {
                        uncacheNotifier(cachedNotifier);
                        cacheNotifier(notifier);
                    }
                }
            }

            OnPropertyChanged(sender, e);
        }

        public void SubscribeNotifier(INotifyPropertyChanged propertyChangedNotifier)
            => propertyChangedNotifier.PropertyChanged += PropertyChangedNotifier_PropertyChanged;

        public void UnsubscribeNotifier(INotifyPropertyChanged propertyChangedNotifier)
            => propertyChangedNotifier.PropertyChanged -= PropertyChangedNotifier_PropertyChanged;
    }
}
