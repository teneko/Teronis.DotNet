using System;
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

        public bool IsWorking => workStatus.IsWorking;

        private WorkStatus workStatus;
        private PropertyChangedRelay propertyChangedRelay;

        public ContentUpdater(WorkStatus workStatus)
        {
            this.workStatus = workStatus ?? throw new ArgumentNullException(nameof(workStatus));
            propertyChangedRelay = new PropertyChangedRelay(GetType(), workStatus);
            propertyChangedRelay.NotifiersPropertyChanged += PropertyChangedRelay_NotifiersPropertyChanged;
        }

        public ContentUpdater()
        : this(new WorkStatus())
        { }

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

        public void BeginWork()
            => workStatus.BeginWork();

        public void EndWork()
            => workStatus.EndWork();

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
            if (IsContentUpdatable(update))
            {
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
            if (IsContentUpdatable(update))
            {
                BeginWork();
                await updateByAsync(update);
                EndWork();
            }
        }

        public virtual async Task UpdateContentByAsync(ITask<IUpdate<ContentType>> updateTask)
        {
            BeginWork();
            var update = await updateTask;

            if (IsContentUpdatable(update))
                await updateByAsync(update);

            EndWork();
        }
    }
}
