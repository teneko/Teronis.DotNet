﻿using System;
using System.ComponentModel;

namespace Teronis.ObjectModel
{
    public class PropertyChangedForwarder : PropertyChangeForwarder<PropertyChangedEventArgs>, INotifyPropertyChanged
    {
        private static Action<object?, PropertyChangedEventArgs>? convertNotifyPropertyChangeMethod(Action<PropertyChangedEventArgs>? notifyPropertyChanged)
        {
            if (notifyPropertyChanged is null) {
                return null;
            } else {
                return (sender, args) => notifyPropertyChanged(args);
            }
        }

        private static Action<object?, PropertyChangedEventArgs>? convertNotifyPropertyChangeMethod(Action<string>? notifyPropertyChanged)
        {
            if (notifyPropertyChanged is null) {
                return null;
            } else {
                return (sender, args) => notifyPropertyChanged(args.PropertyName);
            }
        }

        /// <summary>
        /// Notifies about forwarded property changes from <see cref="PropertyContainer"/>.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChangedForward {
            add => EventInvocationForward += value!.Invoke;
            remove => EventInvocationForward -= value!.Invoke;
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add => PropertyChangedForward += value;
            remove => PropertyChangedForward -= value;
        }

        protected override Action<object?, PropertyChangedEventArgs>? ForwardEventInvocation { get;}

        public PropertyChangedForwarder(Action<object?, PropertyChangedEventArgs>? notifyPropertyChange, object? alternativeEventSender)
            : base(alternativeEventSender: alternativeEventSender) =>
            ForwardEventInvocation = notifyPropertyChange;

        public PropertyChangedForwarder(Action<object?, PropertyChangedEventArgs>? notifyPropertyChange) =>
            ForwardEventInvocation = notifyPropertyChange;

        public PropertyChangedForwarder(object? alternativeEventSender)
           : this(default(Action<object?, PropertyChangedEventArgs>?), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangedForwarder()
           : this(default(Action<object?, PropertyChangedEventArgs>?)) { }

        public PropertyChangedForwarder(Action<PropertyChangedEventArgs>? notifyPropertyChange, object? alternativeEventSender)
            : this(notifyPropertyChange: convertNotifyPropertyChangeMethod(notifyPropertyChange), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangedForwarder(Action<PropertyChangedEventArgs>? notifyPropertyChange)
            : this(notifyPropertyChange: convertNotifyPropertyChangeMethod(notifyPropertyChange)) { }

        public PropertyChangedForwarder(Action<string>? notifyPropertyChange, object? alternativeEventSender)
            : this(convertNotifyPropertyChangeMethod(notifyPropertyChange), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangedForwarder(Action<string>? notifyPropertyChange)
            : this(convertNotifyPropertyChangeMethod(notifyPropertyChange)) { }

        protected override bool CanForwardEventInvocation(PropertyChangedEventArgs eventArgs) =>
            CalleePropertyNameByCallerPropertyNameDictionary.ContainsKey(eventArgs.PropertyName);

        protected override PropertyChangedEventArgs CreateEventArgument(PropertyChangedEventArgs eventArgs)
        {
            var calleePropertyName = CalleePropertyNameByCallerPropertyNameDictionary[eventArgs.PropertyName];
            var forwardingEventArgs = new PropertyChangedEventArgs(calleePropertyName);
            return forwardingEventArgs;
        }

        private void PropertyContainer_PropertyChanged(object sender, PropertyChangedEventArgs args) =>
            OnEventInvocationForward(sender, args);
    }
}
