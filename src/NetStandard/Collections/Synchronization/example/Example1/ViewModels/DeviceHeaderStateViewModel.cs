using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceHeaderStateViewModel : ViewModelBase
    {
        public DeviceHeaderStateEntity State { get; set; }

        public DeviceHeaderStateViewModel(DeviceHeaderStateEntity headerState) =>
            State = headerState ?? new DeviceHeaderStateEntity();

        public DeviceHeaderStateViewModel()
            : this(null) { }
    }
}
