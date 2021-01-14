using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teronis.Collections.Changes;

namespace Teronis.Collections.Synchronization
{
    public class Program
    {
        static async Task Main()
        {
            int[] list, list2;
            List<CollectionModification<int, int>> modifications;

            list = new int[] { 1, 2, 3 };
            list2 = new int[] { 2, 3, 3 };

            modifications = SortedCollectionModifications.YieldCollectionModifications(list, list2, SortedCollectionModificationsOrder.Ascending).ToList();

            ;

            //list = new int[] { 1, 1, 2 };
            //list2 = new int[] { 1, 2, 1 };

            //modifications = list.GetCollectionModifications(list2, EqualityComparer<int>.Default).ToList();

            //;

            //list = new int[] { 1, 1, 2 };
            //list2 = new int[] { 2, 1, 1 };

            //modifications = list.GetCollectionModifications(list2, EqualityComparer<int>.Default).ToList();

            //;

            //list = new int[] { 3, 1, 3 };
            //list2 = new int[] { 5, 3, 5, 3 };

            //modifications = list.GetCollectionModifications(list2, EqualityComparer<int>.Default).ToList();

            //;

            //list = new int[] { 1, 2, 3 };
            //list2 = new int[] { 3, 2, 1 };

            //modifications = list.GetCollectionModifications(list2, EqualityComparer<int>.Default).ToList();

            //;

            //var devicesViewModel = new DevicesViewModel();

            ////var initialDeviceHeaders = new DeviceHeaderEntity[] {
            ////    new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            ////    new DeviceHeaderEntity() { Serial = "1", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            ////    new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            ////};

            ////var deviceHeaders = new DeviceHeaderEntity[] {
            ////    new DeviceHeaderEntity() { Serial = "5", State = new DeviceHeaderStateEntity() },
            ////    new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            ////    new DeviceHeaderEntity() { Serial = "5", State = new DeviceHeaderStateEntity() { IsFactory = true  } },
            ////    new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = true } },
            ////};

            //var initialDeviceHeaders = new DeviceHeaderEntity[] {
            //    new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            //    //new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            //    new DeviceHeaderEntity() { Serial = "2", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            //    new DeviceHeaderEntity() { Serial = "1", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            //};

            //var deviceHeaders = new DeviceHeaderEntity[] {
            //    new DeviceHeaderEntity() { Serial = "1", State = new DeviceHeaderStateEntity() },
            //    new DeviceHeaderEntity() { Serial = "2", State = new DeviceHeaderStateEntity() { IsFactory = false } },
            //    new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = true  } },
            //    //new DeviceHeaderEntity() { Serial = "3", State = new DeviceHeaderStateEntity() { IsFactory = true } },
            //};

            //var test = devicesViewModel.DeviceHeaderCollectionSynchronisation.SubItems.CreateKeyedItemIndexTracker(item => item.Header.Serial);

            //devicesViewModel.UpdateDevices(initialDeviceHeaders);
            //devicesViewModel.UpdateDevices(deviceHeaders);
            ////var deviceHeaderModifications = devicesViewModel.DeviceHeaderCollectionSynchronisation.SuperItems.GetCollectionModifications(deviceHeaders, DeviceHeaderEntityEqualityComparer.Default).ToList();

            ////;

            //devicesViewModel.DeviceHeaderCollectionSynchronisation.SelectedItem = devicesViewModel.DeviceHeaderCollectionSynchronisation.SubItems[0];

            //;
        }
    }
}
