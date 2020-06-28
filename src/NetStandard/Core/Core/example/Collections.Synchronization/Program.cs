using System.Threading.Tasks;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Collections.Synchronization.Example1.ViewModels;

namespace Teronis.Collections.Synchronization
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var devicesViewModel = new DevicesViewModel();

            var initialDeviceHeaders = new DeviceHeaderEntity[] {
                new DeviceHeaderEntity() { Serial = "5", State = new DeviceHeaderStateEntity() },
                new DeviceHeaderEntity() { Serial = "2", State = new DeviceHeaderStateEntity() },
                new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = false } }
            };

            var deviceHeaders = new DeviceHeaderEntity[] {
                new DeviceHeaderEntity() { Serial = "1", State = new DeviceHeaderStateEntity() },
                new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = true } },
                new DeviceHeaderEntity() { Serial = "5", State = new DeviceHeaderStateEntity() }
            };

            await devicesViewModel.UpdateDevicesAsync(initialDeviceHeaders);
            await devicesViewModel.UpdateDevicesAsync(deviceHeaders);

            ;
        }
    }
}
