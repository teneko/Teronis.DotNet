using System.Threading.Tasks;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Collections.Synchronization.Example1.ViewModels;

namespace Teronis.Collections.Synchronization
{
    public class Program
    {
        static async Task Main()
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

            devicesViewModel.UpdateDevices(initialDeviceHeaders);
            devicesViewModel.UpdateDevices(deviceHeaders);
            devicesViewModel.DeviceHeaderCollectionSynchronisation.SelectedItem = devicesViewModel.DeviceHeaderCollectionSynchronisation.SubItems[0];
            ;
        }
    }
}
