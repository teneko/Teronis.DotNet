// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using Teronis.ComponentModel;
using Teronis.ComponentModel.Parenthood;
using Teronis.Reflection.Caching;

namespace Teronis.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged, IHaveParents, IHaveRegisteredParents
    {
        public event PropertyChangedEventHandler? PropertyChanged {
            add => PropertyChangeComponent.PropertyChanged += value;
            remove => PropertyChangeComponent.PropertyChanged -= value;
        }

        public event PropertyChangingEventHandler? PropertyChanging {
            add => PropertyChangeComponent.PropertyChanging += value;
            remove => PropertyChangeComponent.PropertyChanging -= value;
        }

        protected PropertyChangeComponent PropertyChangeComponent { get; private set; } = null!;

        private readonly RegisteredRequestParentHandlerDictionary registeredRequestParentHandlerDictionary;
        private readonly SingleTypePropertyCache<IHaveParents> havingParentsPropertyChangedCache;

        public ViewModelBase()
        {
            PropertyChangeComponent = new PropertyChangeComponent(this);
            registeredRequestParentHandlerDictionary = new RegisteredRequestParentHandlerDictionary(this);
            havingParentsPropertyChangedCache = new SingleTypePropertyCache<IHaveParents>(this);
            havingParentsPropertyChangedCache.PropertyAdded += HavingParentsPropertyChangedCache_PropertyCacheAdded;
            havingParentsPropertyChangedCache.PropertyRemoved -= HavingParentsPropertyChangedCache_PropertyCacheRemoved;
        }

        private void Property_RequestParents(object sender, HavingParentsEventArgs havingParents)
            => havingParents.AddParentAndItsParents(this);

        private void HavingParentsPropertyChangedCache_PropertyCacheAdded(object sender, PropertyCachedEventArgs<IHaveParents> args)
        {
            var propertyValue = args.PropertyValue ?? throw new ArgumentNullException("Property value is null.");
            propertyValue.ParentsRequested += Property_RequestParents;
        }

        private void HavingParentsPropertyChangedCache_PropertyCacheRemoved(object sender, PropertyCacheRemovedEventArgs<IHaveParents> args)
        {
            var propertyValue = args.PropertyValue ?? throw new ArgumentNullException("Property value is null.");
            propertyValue.ParentsRequested -= Property_RequestParents;
        }

        #region IHaveParents

        public event ParentsRequestedEventHandler? ParentsRequested;

        public ParentsCollector CreateParentsCollector()
            => new ParentsCollector(this, ParentsRequested);

        #endregion

        #region IHaveRegisteredParents

        void IHaveRegisteredParents.RegisterParent(ParentsRequestedEventHandler handler)
            => ParentsRequested += handler;

        public void RegisterParent(object caller, ParentsRequestedEventHandler handler)
            => registeredRequestParentHandlerDictionary.RegisterParent(caller, handler);

        void IHaveRegisteredParents.UnregisterParent(ParentsRequestedEventHandler handler)
            => ParentsRequested -= handler;

        public void UnregisterParent(object caller)
            => registeredRequestParentHandlerDictionary.UnregisterParent(caller);

        #endregion

        public class DefaultDataErrorInfos : DataErrorInfosBase { }
    }
}
