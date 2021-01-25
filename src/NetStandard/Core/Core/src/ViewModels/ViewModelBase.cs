using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Teronis.Linq.Expressions;
using Teronis.ObjectModel;
using Teronis.ObjectModel.Parenting;
using Teronis.Reflection.Caching;

namespace Teronis.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged, IHaveParents, IHaveRegisteredParents
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
        }

        internal protected void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangingEventArgs(propertyName);
            PropertyChanging?.Invoke(this, args);
        }

        internal protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, args);
        }

        protected void ChangeProperty(Action action, params string[] properties) {
            foreach (var propertyName in properties) {
                OnPropertyChanging(propertyName);
            }

            action?.Invoke();

            foreach (var propertyName in properties) {
                OnPropertyChanged(propertyName);
            }
        }

        public void ChangeProperty(Action action, Expression<Func<object?>> anonymousProperties) =>
            ChangeProperty(action, ExpressionGenericTools.GetAnonTypeNames(anonymousProperties));

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
