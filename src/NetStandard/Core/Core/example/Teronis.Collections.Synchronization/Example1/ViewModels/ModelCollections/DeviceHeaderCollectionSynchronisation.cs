using System.Collections.ObjectModel;
using Teronis.Data;
using Teronis.Collections.Synchronization.Example1.Models;

namespace Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections
{
    /// <summary>
    /// It holds an observable collection of <see cref="DeviceHeaderSyntheticEntity"/>. Its purpose
    /// is to have a long running synced collection of <see cref="DeviceHeaderSyntheticEntity"/>.
    /// </summary>
    public class DeviceHeaderCollectionSynchronisation : CollectionSynchronisation<DeviceHeaderViewModel, DeviceHeaderEntity>, IHaveParents
    {
        public DeviceHeaderViewModel SelectedItem { get; set; }

        private CollectionItemParentsBehaviour<DeviceHeaderViewModel, DeviceHeaderEntity> itemParentsBehaviour;
        private CollectionItemUpdateBehaviour<DeviceHeaderViewModel, DeviceHeaderEntity> itemUpdateBehaviour;

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
