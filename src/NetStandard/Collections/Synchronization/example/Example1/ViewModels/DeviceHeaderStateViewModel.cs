using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceHeaderStateViewModel : ViewModelBase
    {
        public DeviceHeaderStateEntity State { 
            get => state; 
            set => PropertyChangeComponent.ChangeProperty(ref state, value); }

        private DeviceHeaderStateEntity state = null!;

        public DeviceHeaderStateViewModel(DeviceHeaderStateEntity? headerState) =>
            State = headerState ?? new DeviceHeaderStateEntity();

        public DeviceHeaderStateViewModel()
            : this(null) { }
    }
}
