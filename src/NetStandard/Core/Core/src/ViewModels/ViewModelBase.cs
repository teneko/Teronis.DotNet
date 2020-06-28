using System.ComponentModel;
using System.Linq;
using Teronis.Data;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Collections;
using Teronis.Extensions;
using Teronis.Reflection.Caching;

namespace Teronis.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IHaveParents, IHaveKnownParents, IWorking, INotifyDataErrorInfo
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067
        public event WantParentsEventHandler? WantParents;

        public DynamicParentResolver DynamicParentResolver { get; private set; }
        public bool IsWorking => workStatusPropertyChangedCache.CachedPropertyValues.Values.Any(x => x.IsWorking);

        protected WorkStatus WorkStatus { get; private set; }

        private readonly KnownParentsContainer knownParentsContainer;
        private readonly SingleTypePropertyCache<IHaveParents> havingParentsPropertyChangedCache;
        private readonly SingleTypePropertyCache<IWorking> workStatusPropertyChangedCache;

        public ViewModelBase()
        {
            DynamicParentResolver = new DynamicParentResolver(this);
            knownParentsContainer = new KnownParentsContainer(this);
            havingParentsPropertyChangedCache = new SingleTypePropertyCache<IHaveParents>(this);
            havingParentsPropertyChangedCache.PropertyAdded += HavingParentsPropertyChangedCache_PropertyCacheAdded;
            havingParentsPropertyChangedCache.PropertyRemoved -= HavingParentsPropertyChangedCache_PropertyCacheRemoved;
            /// We only subscribe to <see cref="IWorking"/>-container, so that we can on calculate
            /// <see cref="IsWorking"/> properly.
            workStatusPropertyChangedCache = new SingleTypePropertyCache<IWorking>(this);
            WorkStatus = new WorkStatus();
            validationErrors = new Dictionary<string, ICollection<string>>();
            validationErrorPreviews = new Dictionary<string, ICollection<string>>();
        }

        private void Property_WantParents(object sender, HavingParentsEventArgs havingParents)
            => havingParents.AttachParentParents(this);

        private void HavingParentsPropertyChangedCache_PropertyCacheAdded(object sender, PropertyCachedEventArgs<IHaveParents> args)
            => args.PropertyValue.WantParents += Property_WantParents;

        private void HavingParentsPropertyChangedCache_PropertyCacheRemoved(object sender, PropertyCacheRemovedEventArgs<IHaveParents> args)
            => args.OldPropertyValue.WantParents -= Property_WantParents;

        protected void OnPropertyChanged(PropertyChangedEventArgs args)
            => PropertyChanged?.Invoke(this, args);

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            OnPropertyChanged(args);
        }

        public ParentsPicker GetParentsPicker()
            => new ParentsPicker(this, WantParents);

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public virtual bool HasErrors
            => validationErrors.Count > 0;

        public virtual bool HasErrorPreviews
            => HasErrors || validationErrorPreviews.Count > 0;

        private Dictionary<string, ICollection<string>> validationErrors;
        private Dictionary<string, ICollection<string>> validationErrorPreviews;

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

        private void onErrorPreviewsChanged(string propertyName)
            => OnPropertyChanged(nameof(HasErrorPreviews));

        protected void SetErrors(string propertyName, ICollection<string> errors, bool isPreview)
        {
            if (!isPreview)
                validationErrors[propertyName] = errors;

            validationErrorPreviews[propertyName] = errors;

            if (!isPreview)
                onErrorsChanged(propertyName);

            onErrorPreviewsChanged(propertyName);
        }

        protected void RemoveErrors(string propertyName, bool isPreview)
        {
            if (!isPreview && validationErrors.ContainsKey(propertyName))
                validationErrors.Remove(propertyName);

            if (validationErrorPreviews.ContainsKey(propertyName))
                validationErrorPreviews.Remove(propertyName);

            if (!isPreview)
                onErrorsChanged(propertyName);

            onErrorPreviewsChanged(propertyName);
        }

        #endregion

        #region IHaveKnownParents

        public void AttachKnownWantParentsHandler(object caller, WantParentsEventHandler handler)
            => knownParentsContainer.AttachWantParentsHandler(caller, handler);

        public void AttachWantParentsHandler(WantParentsEventHandler handler)
            => WantParents += handler;

        public void DetachKnownWantParentsHandler(object caller)
            => knownParentsContainer.DetachWantParentsHandler(caller);

        public void DetachWantParentsHandler(WantParentsEventHandler handler)
            => WantParents -= handler;

        #endregion

        #region IWorking

        public virtual void BeginWork()
            => WorkStatus.BeginWork();

        public virtual void EndWork()
            => WorkStatus.EndWork();

        #endregion
    }
}
