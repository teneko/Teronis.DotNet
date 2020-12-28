using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Teronis.ObjectModel.Parenting;
using Teronis.Reflection.Caching;

namespace Teronis.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged, IHaveParents, IHaveRegisteredParents, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        private readonly RegisteredRequestParentHandlerDictionary registeredRequestParentHandlerDictionary;
        private readonly SingleTypePropertyCache<IHaveParents> havingParentsPropertyChangedCache;

        public ViewModelBase()
        {
            registeredRequestParentHandlerDictionary = new RegisteredRequestParentHandlerDictionary(this);
            havingParentsPropertyChangedCache = new SingleTypePropertyCache<IHaveParents>(this);
            havingParentsPropertyChangedCache.PropertyAdded += HavingParentsPropertyChangedCache_PropertyCacheAdded;
            havingParentsPropertyChangedCache.PropertyRemoved -= HavingParentsPropertyChangedCache_PropertyCacheRemoved;
            validationErrors = new Dictionary<string, ICollection<string>>();
        }

        protected void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangingEventArgs(propertyName);
            PropertyChanging?.Invoke(this, args);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, args);
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

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public virtual bool HasErrors
            => validationErrors.Count > 0;

        private readonly Dictionary<string, ICollection<string>> validationErrors;

        public IEnumerable? GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !validationErrors.ContainsKey(propertyName)) {
                return null;
            }

            return validationErrors[propertyName];
        }

        private void onErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

        protected void SetErrors(string propertyName, ICollection<string> errors)
        {
            validationErrors[propertyName] = errors;
            onErrorsChanged(propertyName);
        }

        protected void RemoveErrors(string propertyName)
        {
            if (validationErrors.ContainsKey(propertyName)) {
                validationErrors.Remove(propertyName);
                onErrorsChanged(propertyName);
            }
        }

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
    }
}
