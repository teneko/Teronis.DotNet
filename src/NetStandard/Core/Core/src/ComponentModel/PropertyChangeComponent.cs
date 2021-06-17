// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Teronis.Linq.Expressions;

namespace Teronis.ComponentModel
{
    public class PropertyChangeComponent : INotifyPropertyChanging, INotifyPropertyChanged, IMemberCallablePropertyChangeComponent, IEventInvocablePropertyChangeComponent
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        public bool HasAlternativeEventSender { get; }
        public object? AlternativeEventSender { get; }

        public PropertyChangeComponent(object? alternativeSender)
        {
            HasAlternativeEventSender = true;
            AlternativeEventSender = alternativeSender ?? throw new ArgumentNullException(nameof(alternativeSender));
        }

        public PropertyChangeComponent()
        {
            HasAlternativeEventSender = false;
            AlternativeEventSender = null;
        }

        /// <summary>
        /// Returns <see cref="AlternativeEventSender"/> if <see cref="HasAlternativeEventSender"/> is
        /// <see langword="true"/>, otherwise <paramref name="originalSender"/> gets returned.
        /// </summary>
        /// <param name="originalSender">The sender that initially got received.</param>
        /// <param name="methodOrigin">The caller method origin.</param>
        /// <returns></returns>
        protected virtual object? InterceptSender(object? originalSender, PropertyChangeMethodOrigin methodOrigin)
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
            PropertyChanging?.Invoke(InterceptSender(sender, PropertyChangeMethodOrigin.PropertyChanging), args);

        /// <summary>
        /// Initiates a property changed event invocation.
        /// </summary>
        /// <param name="sender">The sender to be sent.</param>
        /// <param name="args">The argument to be sent.</param>
        public virtual void OnPropertyChanged(object? sender, PropertyChangedEventArgs args) =>
            PropertyChanged?.Invoke(InterceptSender(sender, PropertyChangeMethodOrigin.PropertyChanged), args);

        public void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangingEventArgs(propertyName);
            OnPropertyChanging(this, args);
        }

        public void OnPropertyChanging(params string?[] propertyNames)
        {
            foreach (var propertyName in propertyNames) {
                OnPropertyChanging(propertyName);
            }
        }

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            OnPropertyChanged(this, args);
        }

        public void OnPropertyChanged(params string?[] propertyNames)
        {
            foreach (var propertyName in propertyNames) {
                OnPropertyChanged(propertyName);
            }
        }

        public void ChangeProperty<T>(ref T property, T value, [CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanging(propertyName);
            property = value;
            OnPropertyChanged(propertyName);
        }

        public void ChangeProperty<T>(ref T property, T value, bool noChangeIfEqual, [CallerMemberName] string? propertyName = null)
        {
            if (noChangeIfEqual && Equals(property, value)) {
                return;
            }

            ChangeProperty(ref property, value, propertyName: propertyName);
        }

        public void ChangeProperty<T>(ref T property, T value, params string[] propertyNames)
        {
            OnPropertyChanging(propertyNames);
            property = value;
            OnPropertyChanged(propertyNames);
        }

        public void ChangeProperty<T>(ref T property, T value, bool noChangeIfEqual, params string[] propertyNames)
        {
            if (noChangeIfEqual && Equals(property, value)) {
                return;
            }

            ChangeProperty(ref property, value, propertyNames: propertyNames);
        }

        public void ChangeProperty(Action propertyChangeHandler, params string[] propertyNames)
        {
            OnPropertyChanging(propertyNames);
            propertyChangeHandler?.Invoke();
            OnPropertyChanged(propertyNames);
        }

        /// <summary>
        /// Fires <see cref="PropertyChanging"/> before invoking <paramref name="propertyChangeHandler"/> and
        /// fires <see cref="PropertyChanged"/> after invoking <paramref name="propertyChangeHandler"/>.
        /// </summary>
        /// <param name="propertyChangeHandler">The handler that perfomens the property change.</param>
        /// <param name="anonymousProperties">The properties that are affected by change. (e.g. () => { prop1, prop2 })</param>
        public void ChangeProperty(Action propertyChangeHandler, Expression<Func<object?>> anonymousProperties) =>
            ChangeProperty(propertyChangeHandler, ExpressionGenericTools.GetAnonymousTypeNames(anonymousProperties));

        public IMemberCallablePropertyChangeComponent AsMemberCallable() =>
            this;

        public IEventInvocablePropertyChangeComponent AsEventInvocable() =>
            this;
    }
}
