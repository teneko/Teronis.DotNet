using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PropertyChanging;
using Teronis.Extensions.NetStandard;
using Teronis.Tools.NetStandard;

namespace Teronis.Data
{
    public class ContainerUpdater<T> : INotifyPropertyChanged, IUpdatableContainer<T>
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
        public event UpdatingEventHandler<T> ContainerUpdating;
        public event UpdatedEventHandler<T> ContainerUpdated;

        public bool IsContainerUpdating => updateSequenceStatus.IsContainerUpdating;

        private ContainerUpdateSequenceStatus updateSequenceStatus;
        private PropertyChangedRelay propertyChangedRelay;

        public ContainerUpdater()
        {
            updateSequenceStatus = new ContainerUpdateSequenceStatus();
            propertyChangedRelay = new PropertyChangedRelay(GetType(), updateSequenceStatus);
            propertyChangedRelay.PropertyChanged += PropertyChangedRelay_PropertyChanged;
        }

        private void PropertyChangedRelay_PropertyChanged(object sender, PropertyChangedEventArgs e)
           => PropertyChanged?.Invoke(this, e);

        public bool IsContainerUpdatable(Update<T> update)
           => new UpdatingEventArgs<T>(update).IsUpdateAppliable(this, ContainerUpdating);

        /// <summary>
        /// When using this function, you have to call <see cref="EndContainerUpdate"/> by yourself explicit.
        /// </summary>
        public void BeginContainerUpdate()
            => updateSequenceStatus.BeginContainerUpdate();

        /// <summary>
        /// Buisness logic have to be implemented here
        /// </summary>
        /// <param name="update"></param>
        protected virtual void InnerUpdatBy(Update<T> update)
        { }

        /// <summary>
        /// Buisness logic have to be implemented here
        /// </summary>
        /// <param name="update"></param>
        protected virtual Task InnerUpdatByAsync(Update<T> update)
            => Task.CompletedTask;

        public void EndContainerUpdate()
            => updateSequenceStatus.EndContainerUpdate();

        public void UpdateContainerBy(Update<T> update)
        {
            if (IsContainerUpdatable(update)) {
                InnerUpdatBy(update);
                ContainerUpdated?.Invoke(this, update);
            }
        }

        private async Task updateByAsync(Update<T> update)
        {
            await InnerUpdatByAsync(update);
            ContainerUpdated?.Invoke(this, update);
        }

        public async Task UpdateContainerByAsync(Update<T> update)
        {
            if (IsContainerUpdatable(update)) {
                BeginContainerUpdate();
                await updateByAsync(update);
                EndContainerUpdate();
            }
        }

        public async Task UpdateContainerByAsync(Task<Update<T>> updateTask)
        {
            BeginContainerUpdate();
            var update = await updateTask;

            if (IsContainerUpdatable(update))
                await updateByAsync(update);

            EndContainerUpdate();
        }
    }
}
