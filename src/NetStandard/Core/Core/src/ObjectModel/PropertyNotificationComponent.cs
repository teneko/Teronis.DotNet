// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Teronis.Linq.Expressions;

namespace Teronis.ObjectModel
{
    public class PropertyNotificationComponent : INotifyPropertyChanging, INotifyPropertyChanged, IMemberCallablePropertyNotificationComponent, IEventInvocablePropertyNotificationComponent
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        public bool HasAlternativeEventSender { get; }
        public object? AlternativeEventSender { get; }

        public PropertyNotificationComponent(object? alternativeSender)
        {
            HasAlternativeEventSender = true;
            AlternativeEventSender = alternativeSender ?? throw new ArgumentNullException(nameof(alternativeSender));
        }

        public PropertyNotificationComponent()
        {
            HasAlternativeEventSender = false;
            AlternativeEventSender = null;
        }

        protected virtual object? GetAlternativeEventSender(object? originalSender)
        {
            if (HasAlternativeEventSender) {
                originalSender = AlternativeEventSender;
            }

            return originalSender;
        }

        /// <summary>
        /// Initiates a property changing event invocation.
        /// </summary>
        /// <param name="sender">The sender to be sent.</param>
        /// <param name="args">The argument to be sent.</param>
        public virtual void OnPropertyChanging(object? sender, PropertyChangingEventArgs args) =>
            PropertyChanging?.Invoke(GetAlternativeEventSender(sender), args);

        /// <summary>
        /// Initiates a property changed event invocation.
        /// </summary>
        /// <param name="sender">The sender to be sent.</param>
        /// <param name="args">The argument to be sent.</param>
        public virtual void OnPropertyChanged(object? sender, PropertyChangedEventArgs args) =>
            PropertyChanged?.Invoke(GetAlternativeEventSender(sender), args);

        public void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangingEventArgs(propertyName);
            OnPropertyChanging(this, args);
        }

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            OnPropertyChanged(this, args);
        }

        public void ChangeProperty(Action prePropertyChangedDelegate, params string[] properties)
        {
            foreach (var propertyName in properties) {
                OnPropertyChanging(propertyName);
            }

            prePropertyChangedDelegate?.Invoke();

            foreach (var propertyName in properties) {
                OnPropertyChanged(propertyName);
            }
        }

        public void ChangeProperty(Action prePropertyChangedDelegate, Expression<Func<object?>> anonymousProperties) =>
            ChangeProperty(prePropertyChangedDelegate, ExpressionGenericTools.GetAnonymousTypeNames(anonymousProperties));

        public IMemberCallablePropertyNotificationComponent AsMemberCallable() =>
            this;

        public IEventInvocablePropertyNotificationComponent AsEventInvocable() =>
            this;
    }
}
