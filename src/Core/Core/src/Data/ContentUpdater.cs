using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Teronis.Data
{
    public class ContentUpdater<ContentType> : INotifyPropertyChanged, IUpdatableContent<ContentType>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event ContentUpdatingEventHandler<ContentType> ContainerUpdating;
        public event ContentUpdatedEventHandler<ContentType> ContainerUpdated;

        public bool IsWorking => workStatus.IsWorking;

        private WorkStatus workStatus;
        private PropertyChangedRelay propertyChangedRelay;

        /// <summary>
        /// We want to assure, that when a work status is passed, that it is not null by accident.
        /// </summary>
        public ContentUpdater(WorkStatus workStatus)
        {
            this.workStatus = workStatus ?? throw new ArgumentNullException(nameof(workStatus));
            propertyChangedRelay = new PropertyChangedRelay(GetType(), workStatus);
            propertyChangedRelay.NotifiersPropertyChanged += PropertyChangedRelay_NotifiersPropertyChanged;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
            => PropertyChanged?.Invoke(this, args);

        protected virtual void OnContainerUpdating(IContentUpdatingEventArgs<ContentType> args)
            => ContainerUpdating?.Invoke(this, args);

        protected virtual void OnContainerUpdated(IContentUpdatedEventArgs<ContentType> update)
            => ContainerUpdated?.Invoke(this, update);

        private void PropertyChangedRelay_NotifiersPropertyChanged(object sender, PropertyChangedEventArgs args)
           => OnPropertyChanged(args);

        public virtual bool IsContentUpdatable(IContentUpdate<ContentType> update)
        {
            var args = new ContentUpdatingEventArgs<ContentType>(update);
            OnContainerUpdating(args);
            return !args.Handled;
        }

        public void BeginWork()
            => workStatus.BeginWork();

        public void EndWork()
            => workStatus.EndWork();

        protected virtual void InnerUpdateContentBy(IContentUpdate<ContentType> update)
        { }

        /// <summary>
        /// When not overriden, the method calls <see cref="InnerUpdateContentBy(IContentUpdate{ContentType})"/>.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        protected virtual Task InnerUpdateContentByAsync(IContentUpdate<ContentType> update)
        {
            InnerUpdateContentBy(update);
            return Task.CompletedTask;
        }

        public virtual void UpdateContentBy(IContentUpdate<ContentType> update)
        {
            if (IsContentUpdatable(update))
            {
                InnerUpdateContentBy(update);
                var args = new ContentUpdatedEventArgs<ContentType>(update);
                OnContainerUpdated(args);
            }
        }

        private async Task updateContentByAsync(IContentUpdate<ContentType> update)
        {
            await Task.Yield();
            BeginWork();
            await update.ContentTask;

            if (IsContentUpdatable(update))
            {
                await InnerUpdateContentByAsync(update);
                var args = new ContentUpdatedEventArgs<ContentType>(update);
                OnContainerUpdated(args);
            }

            EndWork();
        }

        public virtual Task UpdateContentByAsync(IContentUpdate<ContentType> update)
            => updateContentByAsync(update);
    }
}
