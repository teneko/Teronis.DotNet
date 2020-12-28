using System;
using System.ComponentModel;

namespace Teronis.ObjectModel
{
    public class PropertyChangingForwarder : PropertyChangeForwarder<INotifyPropertyChanging, PropertyChangingEventArgs>, INotifyPropertyChanging
    {
        private static Action<object, PropertyChangingEventArgs>? convertNotifyPropertyChangingMethod(Action<PropertyChangingEventArgs>? notifyPropertyChanging)
        {
            if (notifyPropertyChanging is null) {
                return null;
            } else {
                return (sender, args) => notifyPropertyChanging(args);
            }
        }

        private static Action<object, PropertyChangingEventArgs>? convertNotifyPropertyChangingMethod(Action<string>? notifyPropertyChanging)
        {
            if (notifyPropertyChanging is null) {
                return null;
            } else {
                return (sender, args) => notifyPropertyChanging(args.PropertyName);
            }
        }

        /// <summary>
        /// Notifies about forwarded property changes from <see cref="PropertyContainer"/>.
        /// </summary>
        public event PropertyChangingEventHandler? PropertyChangingForward {
            add => EventInvocationForward += value!.Invoke;
            remove => EventInvocationForward -= value!.Invoke;
        }

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging {
            add => PropertyChangingForward += value;
            remove => PropertyChangingForward -= value;
        }

        protected override Action<object, PropertyChangingEventArgs>? ForwardEventInvocation { get; }

        public PropertyChangingForwarder(INotifyPropertyChanging propertyContainer, Action<object, PropertyChangingEventArgs>? notifyPropertyChange = null, object? alternativeEventSender = null)
            : base(propertyContainer, alternativeEventSender: alternativeEventSender) =>
            ForwardEventInvocation = notifyPropertyChange;

        public PropertyChangingForwarder(INotifyPropertyChanging propertyContainer, object? alternativeEventSender = null)
           : this(propertyContainer, default(Action<object, PropertyChangingEventArgs>?), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangingForwarder(INotifyPropertyChanging propertyContainer)
           : this(propertyContainer, default(Action<object, PropertyChangingEventArgs>?), alternativeEventSender: default) { }

        public PropertyChangingForwarder(INotifyPropertyChanging propertyContainer, Action<PropertyChangingEventArgs>? notifyPropertyChange = null, object? alternativeEventSender = null)
            : this(propertyContainer, notifyPropertyChange: convertNotifyPropertyChangingMethod(notifyPropertyChange), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangingForwarder(INotifyPropertyChanging propertyContainer, Action<string>? notifyPropertyChange = null, object? alternativeEventSender = null)
            : this(propertyContainer, convertNotifyPropertyChangingMethod(notifyPropertyChange), alternativeEventSender: alternativeEventSender) { }

        protected override bool CanForwardEventInvocation(PropertyChangingEventArgs eventArgs) =>
            CalleePropertyNameByCallerPropertyNameDictionary.ContainsKey(eventArgs.PropertyName);

        protected override PropertyChangingEventArgs CreateEventArgument(PropertyChangingEventArgs eventArgs)
        {
            var calleePropertyName = CalleePropertyNameByCallerPropertyNameDictionary[eventArgs.PropertyName];
            var forwardingEventArgs = new PropertyChangingEventArgs(calleePropertyName);
            return forwardingEventArgs;
        }

        private void PropertyContainer_PropertyChanging(object sender, PropertyChangingEventArgs args) =>
            OnEventInvocationForward(sender, args);
    }
}
