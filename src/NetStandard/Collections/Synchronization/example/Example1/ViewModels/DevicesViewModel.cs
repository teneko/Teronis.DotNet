using System.Collections.Generic;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DevicesViewModel : ViewModelBase
    {
        public DeviceHeaderCollectionSynchronisation DeviceHeaderCollectionSynchronisation { get; private set; }
        public DeviceCollectionSynchronisation DeviceCollectionSynchronisation { get; private set; }

        public DevicesViewModel()
        {
            DeviceHeaderCollectionSynchronisation = new DeviceHeaderCollectionSynchronisation();
            DeviceCollectionSynchronisation = new DeviceCollectionSynchronisation(DeviceHeaderCollectionSynchronisation);
        }

        public void UpdateDevices(IEnumerable<DeviceHeaderEntity> deviceHeaders) => 
            DeviceHeaderCollectionSynchronisation.SynchronizeCollection(deviceHeaders);
    }
}
