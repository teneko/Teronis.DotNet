using System.Collections.ObjectModel;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Data;

namespace Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections
{
    /// <summary>
    /// It holds an observable collection of <see cref="DeviceHeaderSyntheticEntity"/>. Its purpose
    /// is to have a long running synced collection of <see cref="DeviceHeaderSyntheticEntity"/>.
    /// </summary>
    public class DeviceHeaderCollectionSynchronisation : CollectionSynchronisation<DeviceHeaderViewModel, DeviceHeaderEntity>, IHaveParents
    {
        public DeviceHeaderViewModel SelectedItem { get; set; }

#pragma warning disable IDE0052 // Ungelesene private Member entfernen
        private readonly CollectionItemParentsBehaviour<DeviceHeaderViewModel, DeviceHeaderEntity> itemParentsBehaviour;
        private readonly CollectionItemUpdateBehaviour<DeviceHeaderViewModel, DeviceHeaderEntity> itemUpdateBehaviour;
#pragma warning restore IDE0052 // Ungelesene private Member entfernen

        public DeviceHeaderCollectionSynchronisation()
            : base(new ObservableCollection<DeviceHeaderViewModel>(), new ObservableCollection<DeviceHeaderEntity>(), DeviceHeaderEntityEqualityComparer.Default)
        {
            itemParentsBehaviour = new CollectionItemParentsBehaviour<DeviceHeaderViewModel, DeviceHeaderEntity>(this);
            itemUpdateBehaviour = new CollectionItemUpdateBehaviour<DeviceHeaderViewModel, DeviceHeaderEntity>(this);
        }

        protected override DeviceHeaderViewModel CreateItem(DeviceHeaderEntity newItem)
            => new DeviceHeaderViewModel(newItem);
    }
}
