using System.Threading.Tasks;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Collections.Synchronization.Example1.ViewModels;
using Teronis.Collections.Synchronization.Example1.ViewModels.ModelCollections;

namespace Teronis.Collections.Sync
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var devicesViewModel = new DevicesViewModel();

            var deviceHeaderCollectionSynchronisation = new DeviceHeaderCollectionSynchronisation();

            var initialDeviceHeaders = new DeviceHeaderEntity[] {
                new DeviceHeaderEntity() { Serial = "5", State = new DeviceHeaderStateEntity() },
                new DeviceHeaderEntity() { Serial = "2", State = new DeviceHeaderStateEntity() },
                new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = false } }
            };

            deviceHeaderCollectionSynchronisation.Synchronize(initialDeviceHeaders);

            var deviceHeaders2 = new DeviceHeaderEntity[] {
                new DeviceHeaderEntity() { Serial = "1", State = new DeviceHeaderStateEntity() },
                new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = true } },
                new DeviceHeaderEntity() { Serial = "5", State = new DeviceHeaderStateEntity() }
            };

            await devicesViewModel.UpdateDevicesAsync(initialDeviceHeaders);
            await devicesViewModel.UpdateDevicesAsync(deviceHeaders2);

            ;
        }
    }
}
