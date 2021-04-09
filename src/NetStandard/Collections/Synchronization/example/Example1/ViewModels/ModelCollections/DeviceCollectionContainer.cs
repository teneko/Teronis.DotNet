using Teronis.ObjectModel;

namespace Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections
{
    /// <summary>
    /// It holds an observable collection of <see cref="DeviceHeaderViewModel"/>. Its purpose
    /// is to have a long running synced collection of <see cref="DeviceViewModel"/>.
    /// </summary>
    public class DeviceCollectionSynchronization : CustomSynchronizingCollection<DeviceHeaderViewModel, DeviceViewModel>
    {
        public DeviceViewModel SelectedItem {
            get => deviceHeaderCollectionContainer.SelectedItem?.CreateParentsCollector().SingleParent<DeviceViewModel>()!;
            private set => deviceHeaderCollectionContainer.SelectedItem = value.HeaderContainer;
        }

        private readonly PropertyChangedForwarder propertyChangedRelay;
        private readonly DeviceHeaderCollectionSynchronization deviceHeaderCollectionContainer;
#pragma warning disable IDE0052 // Ungelesene private Member entfernen
        private readonly SynchronizationMirror<DeviceHeaderViewModel> synchronizationMirror;
        private readonly AddRemoveResetBehaviourForCollectionItemByAddRemoveParents<DeviceHeaderViewModel, DeviceViewModel> itemParentsBehaviour;
#pragma warning restore IDE0052 // Ungelesene private Member entfernen

        public DeviceCollectionSynchronization(DeviceHeaderCollectionSynchronization deviceHeaderCollectionContainer)
        {
            propertyChangedRelay = new PropertyChangedForwarder(PropertyChangeComponent.AsEventInvocable().OnPropertyChanged);
            propertyChangedRelay.AddPropertyChangeForwarding(nameof(DeviceHeaderCollectionSynchronization.SelectedItem));
            synchronizationMirror = CreateSynchronizationMirror(deviceHeaderCollectionContainer.SubItems);
            itemParentsBehaviour = new AddRemoveResetBehaviourForCollectionItemByAddRemoveParents<DeviceHeaderViewModel, DeviceViewModel>(this);
            this.deviceHeaderCollectionContainer = deviceHeaderCollectionContainer;
        }

        protected override DeviceViewModel CreateSubItem(DeviceHeaderViewModel newItem) =>
            new DeviceViewModel(newItem);
    }
}
