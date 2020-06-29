using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Teronis.Data;

namespace Teronis.ObjectModel.Updates
{
    public class ContentUpdater<ContentType> : INotifyPropertyChanged, IApplyContentUpdate<ContentType>
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event ContentUpdatingEventHandler<ContentType>? ContainerUpdating;
        public event ContentUpdatedEventHandler<ContentType>? ContainerUpdated;

        public bool IsWorking => workStatus.IsWorking;

        private WorkStatus workStatus;
        private PropertyChangedRelay propertyChangedRelay;

        /// <summary>
        /// We want to assure, that when a work status is passed, that it is not null by accident.
        /// </summary>
        public ContentUpdater(WorkStatus workStatus)
        {
            this.workStatus = workStatus ?? throw new ArgumentNullException(nameof(workStatus));

            propertyChangedRelay = new PropertyChangedRelay()
                .AddAllowedProperty<IWorking>(prop => prop.IsWorking)
                .SubscribePropertyChangedNotifier(workStatus);

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

        public virtual bool IsContentUpdateAppliable(IContentUpdate<ContentType> update)
        {
            var args = new ContentUpdatingEventArgs<ContentType>(update);
            OnContainerUpdating(args);
            return !args.Handled;
        }

        public void BeginWork()
            => workStatus.BeginWork();

        public void EndWork()
            => workStatus.EndWork();

        protected virtual Task UpdateContentByAsync(IContentUpdate<ContentType> update) =>
            Task.CompletedTask;

        public virtual async Task ApplyContentUpdateByAsync(IContentUpdate<ContentType> update)
        {
            BeginWork();
            await Task.Yield();

            try {
                await update.ContentTask;

                if (IsContentUpdateAppliable(update)) {
                    await UpdateContentByAsync(update);
                    var args = new ContentUpdatedEventArgs<ContentType>(update);
                    OnContainerUpdated(args);
                    //return true;
                }
            } finally {
                EndWork();
            }

            //return false;
        }
    }
}
