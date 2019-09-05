using System.ComponentModel;
using System.Threading.Tasks;
using MorseCode.ITask;

namespace Teronis.Data
{
    public class ContentUpdater<ContentType> : INotifyPropertyChanged, IUpdatableContent<ContentType>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event UpdatingEventHandler<ContentType> ContainerUpdating;
        public event UpdatedEventHandler<ContentType> ContainerUpdated;

        public bool IsContentUpdating => updateSequenceStatus.IsContentUpdating;

        private ContainerUpdateSequenceStatus updateSequenceStatus;
        private PropertyChangedRelay propertyChangedRelay;

        public ContentUpdater()
        {
            updateSequenceStatus = new ContainerUpdateSequenceStatus();
            propertyChangedRelay = new PropertyChangedRelay(GetType(), updateSequenceStatus);
            propertyChangedRelay.NotifiersPropertyChanged += PropertyChangedRelay_NotifiersPropertyChanged;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
            => PropertyChanged?.Invoke(this, args);

        protected virtual void OnContainerUpdating(IUpdatingEventArgs<ContentType> args)
            => ContainerUpdating?.Invoke(this, args);

        protected virtual void OnContainerUpdated(IUpdate<ContentType> update)
            => ContainerUpdated?.Invoke(this, update);

        private void PropertyChangedRelay_NotifiersPropertyChanged(object sender, PropertyChangedEventArgs args)
           => OnPropertyChanged(args);

        public virtual bool IsContentUpdatable(IUpdate<ContentType> update)
        {
            var args = new UpdatingEventArgs<ContentType>(update);
            OnContainerUpdating(args);
            return !args.Handled;
        }

        public void BeginContentUpdate()
            => updateSequenceStatus.BeginContentUpdate();

        public void EndContentUpdate()
            => updateSequenceStatus.EndContentUpdate();

        /// <summary>
        /// Buisness logic have to be implemented here
        /// </summary>
        /// <param name="update"></param>
        protected virtual void InnerUpdatBy(IUpdate<ContentType> update)
        { }

        /// <summary>
        /// Buisness logic have to be implemented here
        /// </summary>
        /// <param name="update"></param>
        protected virtual Task InnerUpdatByAsync(IUpdate<ContentType> update)
        {
            InnerUpdatBy(update);
            return Task.CompletedTask;
        }

        public virtual void UpdateContentBy(IUpdate<ContentType> update)
        {
            if (IsContentUpdatable(update)) {
                InnerUpdatBy(update);
                OnContainerUpdated(update);
            }
        }

        private async Task updateByAsync(IUpdate<ContentType> update)
        {
            await InnerUpdatByAsync(update);
            OnContainerUpdated(update);
        }

        public virtual async Task UpdateContentByAsync(IUpdate<ContentType> update)
        {
            if (IsContentUpdatable(update)) {
                BeginContentUpdate();
                await updateByAsync(update);
                EndContentUpdate();
            }
        }

        public virtual async Task UpdateContentByAsync(ITask<IUpdate<ContentType>> updateTask)
        {
            BeginContentUpdate();
            var update = await updateTask;

            if (IsContentUpdatable(update))
                await updateByAsync(update);

            EndContentUpdate();
        }
    }
}
