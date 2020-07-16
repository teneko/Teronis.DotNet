using System.Collections.Generic;
using System.Threading.Tasks;
using MorseCode.ITask;
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

        public Task UpdateDevicesAsync(IEnumerable<DeviceHeaderEntity> deviceHeaders)
            => DeviceHeaderCollectionSynchronisation.SynchronizeAsync(Task.FromResult(deviceHeaders).AsITask());
    }
}
