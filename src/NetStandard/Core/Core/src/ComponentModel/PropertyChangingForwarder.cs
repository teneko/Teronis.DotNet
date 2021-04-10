// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace Teronis.ComponentModel
{
    public class PropertyChangingForwarder : PropertyChangeForwarder<PropertyChangingEventArgs>, INotifyPropertyChanging
    {
        private static Action<object?, PropertyChangingEventArgs>? convertNotifyPropertyChangingMethod(Action<PropertyChangingEventArgs>? notifyPropertyChanging)
        {
            if (notifyPropertyChanging is null) {
                return null;
            } else {
                return (sender, args) => notifyPropertyChanging(args);
            }
        }

        private static Action<object?, PropertyChangingEventArgs>? convertNotifyPropertyChangingMethod(Action<string?>? notifyPropertyChanging)
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

        event PropertyChangingEventHandler? INotifyPropertyChanging.PropertyChanging {
            add => PropertyChangingForward += value;
            remove => PropertyChangingForward -= value;
        }

        protected override Action<object?, PropertyChangingEventArgs>? ForwardEventInvocation { get; }

        public PropertyChangingForwarder(Action<object?, PropertyChangingEventArgs>? notifyPropertyChange, object? alternativeEventSender)
            : base(alternativeEventSender: alternativeEventSender) =>
            ForwardEventInvocation = notifyPropertyChange;

        public PropertyChangingForwarder(Action<object?, PropertyChangingEventArgs>? notifyPropertyChange) =>
            ForwardEventInvocation = notifyPropertyChange;

        public PropertyChangingForwarder(object? alternativeEventSender)
           : this(default(Action<object?, PropertyChangingEventArgs>?), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangingForwarder()
           : this(default(Action<object?, PropertyChangingEventArgs>?)) { }

        public PropertyChangingForwarder(Action<PropertyChangingEventArgs>? notifyPropertyChange, object? alternativeEventSender)
            : this(notifyPropertyChange: convertNotifyPropertyChangingMethod(notifyPropertyChange), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangingForwarder(Action<PropertyChangingEventArgs>? notifyPropertyChange)
            : this(notifyPropertyChange: convertNotifyPropertyChangingMethod(notifyPropertyChange)) { }

        public PropertyChangingForwarder(Action<string?>? notifyPropertyChange, object? alternativeEventSender)
            : this(convertNotifyPropertyChangingMethod(notifyPropertyChange), alternativeEventSender: alternativeEventSender) { }

        public PropertyChangingForwarder(Action<string?>? notifyPropertyChange)
            : this(convertNotifyPropertyChangingMethod(notifyPropertyChange)) { }

        protected override bool CanForwardEventInvocation(PropertyChangingEventArgs eventArgs) =>
            eventArgs.PropertyName != null && CalleePropertyNameByCallerPropertyNameDictionary.ContainsKey(eventArgs.PropertyName);

        protected override PropertyChangingEventArgs CreateEventArgument(PropertyChangingEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == null) {
                throw new ArgumentException("Non-null property name was expected.");
            }

            var calleePropertyName = CalleePropertyNameByCallerPropertyNameDictionary[eventArgs.PropertyName];
            var forwardingEventArgs = new PropertyChangingEventArgs(calleePropertyName);
            return forwardingEventArgs;
        }

        private void PropertyContainer_PropertyChanging(object sender, PropertyChangingEventArgs args) =>
            OnEventInvocationForward(sender, args);
    }
}
