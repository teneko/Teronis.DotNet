using System.Collections.ObjectModel;
using System.ComponentModel;
using Teronis.Collections.Changes;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.ObjectModel;

namespace Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections
{
    /// <summary>
    /// It holds an observable collection of <see cref="DeviceHeaderSyntheticEntity"/>. Its purpose
    /// is to have a long running synced collection of <see cref="DeviceHeaderSyntheticEntity"/>.
    /// </summary>
    public class DeviceCollectionSynchronisation : CollectionSynchronisation<DeviceViewModel, DeviceHeaderViewModel>
    {
        public DeviceViewModel SelectedItem {
            get => deviceHeaderCollectionContainer.SelectedItem?.GetParentsPicker().GetSingleParent<DeviceViewModel>();
            private set => deviceHeaderCollectionContainer.SelectedItem = value.HeaderContainer;
        }

        private readonly PropertyChangedRelay propertyChangedRelay;
        private readonly DeviceHeaderCollectionSynchronisation deviceHeaderCollectionContainer;
        private readonly ConversionAdapter deviceHeaderConversionAdapter;
#pragma warning disable IDE0052 // Ungelesene private Member entfernen
        private readonly CollectionItemParentsBehaviour<DeviceViewModel, DeviceHeaderViewModel> itemParentsBehaviour;
#pragma warning restore IDE0052 // Ungelesene private Member entfernen

        public DeviceCollectionSynchronisation(DeviceHeaderCollectionSynchronisation deviceHeaderCollectionContainer)
            : base(new ObservableCollection<DeviceViewModel>(), new ObservableCollection<DeviceHeaderViewModel>())
        {
            propertyChangedRelay = new PropertyChangedRelay()
                .SubscribePropertyChangedNotifier(deviceHeaderCollectionContainer)
                .AddAllowedProperty(nameof(SelectedItem), null);

            propertyChangedRelay.NotifiersPropertyChanged += PropertyChangedRelay_NotifiersPropertyChanged;
            deviceHeaderConversionAdapter = CreateConversionAdapter();
            itemParentsBehaviour = new CollectionItemParentsBehaviour<DeviceViewModel, DeviceHeaderViewModel>(this);
            this.deviceHeaderCollectionContainer = deviceHeaderCollectionContainer;
            this.deviceHeaderCollectionContainer.CollectionChangeApplied += DeviceHeaderCollectionContainer_CollectionChangeApplied;
        }

        private void DeviceHeaderCollectionContainer_CollectionChangeApplied(object sender, CollectionChangeAppliedEventArgs<DeviceHeaderViewModel, DeviceHeaderEntity> args)
            => deviceHeaderConversionAdapter.RelayCollectionChange(args);

        protected override DeviceViewModel CreateItem(DeviceHeaderViewModel newItem)
        {
            return new DeviceViewModel(newItem);
        }

        private void PropertyChangedRelay_NotifiersPropertyChanged(object sender, PropertyChangedEventArgs args)
            => OnPropertyChanged(args);
    }
}
