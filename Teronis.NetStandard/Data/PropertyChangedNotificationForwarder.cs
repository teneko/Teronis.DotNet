using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Teronis.Reflection;

namespace Teronis.Data
{
    public class PropertyChangedNotificationForwarder
    {
        public INotifyPropertyChanged PropertyChangedNotifier { get; private set; }
        public PropertyChangedEventHandler NotifyingEventHandler { get; private set; }
        public Type CommonPropertiesContainerType { get; private set; }
        public object AlternativePropertyChangedSender { get; private set; }

        public PropertyChangedNotificationForwarder(INotifyPropertyChanged propertyChangedNotifier, Action<object, PropertyChangedEventArgs> notifyingEventInvoker, Type commonPropertiesContainerType, object alternativePropertyChangedSender)
        {
            PropertyChangedNotifier = propertyChangedNotifier;
            PropertyChangedNotifier.PropertyChanged += PropertyChangedNotifier_PropertyChanged;
            NotifyingEventHandler = notifyingEventInvoker;
            CommonPropertiesContainerType = commonPropertiesContainerType;
            AlternativePropertyChangedSender = alternativePropertyChangedSender;
        }

        public PropertyChangedNotificationForwarder(INotifyPropertyChanged propertyChangedNotifier,ref PropertyChangedEventHandler notifyingEventHandler, Type notifyingEventHandlerDeclaringType)
            : this(propertyChangedNotifier, notifyingEventHandler, notifyingEventHandlerDeclaringType, default(object)) { }

        public PropertyChangedNotificationForwarder(INotifyPropertyChanged propertyChangedNotifier,ref PropertyChangedEventHandler notifyingEventHandler, object alternativePropertyChangedSender)
            : this(propertyChangedNotifier,notifyingEventHandler, default, alternativePropertyChangedSender) { }

        public PropertyChangedNotificationForwarder(INotifyPropertyChanged propertyChangedNotifier, ref PropertyChangedEventHandler notifyingEventHandler, object alternativePropertyChangedSender, bool alternativePropertyChangedSenderTypeIsCommonPropertyContainerType)
            : this(propertyChangedNotifier,notifyingEventHandler, alternativePropertyChangedSender)
        {
            if (alternativePropertyChangedSender != null && alternativePropertyChangedSenderTypeIsCommonPropertyContainerType)
                CommonPropertiesContainerType = alternativePropertyChangedSender.GetType();
        }

        private void PropertyChangedNotifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var readonlyPropertyBindingAttributes = VariableInfoSettings.DefaultFlags | System.Reflection.BindingFlags.GetProperty;
            var shouldNotNotifyUncommonProperty = CommonPropertiesContainerType != null && CommonPropertiesContainerType.GetProperty(e.PropertyName, readonlyPropertyBindingAttributes) == null;

            if (shouldNotNotifyUncommonProperty)
                return;

            if (AlternativePropertyChangedSender != null)
                sender = AlternativePropertyChangedSender;

            NotifyingEventHandler?.Invoke(sender, e);
        }
    }
}
