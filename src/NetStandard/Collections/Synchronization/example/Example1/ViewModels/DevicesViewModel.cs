using System.Collections.Generic;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DevicesViewModel : ViewModelBase
    {
        public DeviceHeaderCollectionSynchronization DeviceHeaderCollectionSynchronization { get; private set; }
        public DeviceCollectionSynchronization DeviceCollectionSynchronization { get; private set; }

        public DevicesViewModel()
        {
            DeviceHeaderCollectionSynchronization = new DeviceHeaderCollectionSynchronization();
            DeviceCollectionSynchronization = new DeviceCollectionSynchronization(DeviceHeaderCollectionSynchronization);
        }

        public void UpdateDevices(IEnumerable<DeviceHeaderEntity> deviceHeaders) => 
            DeviceHeaderCollectionSynchronization.SynchronizeCollection(deviceHeaders);
    }
}
