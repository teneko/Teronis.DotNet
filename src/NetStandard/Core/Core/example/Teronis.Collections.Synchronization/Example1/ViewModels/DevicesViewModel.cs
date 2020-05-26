using System.Threading.Tasks;
using Teronis.ViewModels;
using Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections;
using Teronis.Collections.Synchronization.Example1.Models;
using System.Collections.Generic;
using MorseCode.ITask;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DevicesViewModel : ViewModelBase
    {
        //public delegate DevicesViewModel CreateFromSession(AuthenticatedSession session);

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
