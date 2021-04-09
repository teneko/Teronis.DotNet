using System;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceHeaderViewModel : ViewModelBase
    {
        public DeviceHeaderEntity Header {
            get => header;

            set {
                PropertyChangeComponent.OnPropertyChanging();
                PropertyChangeComponent.ChangeProperty(ref header, value);

                if (!(value.State is null)) {
                    StateContainer.State = value.State;
                }

                PropertyChangeComponent.OnPropertyChanged();
            }
        }

        public DeviceHeaderStateViewModel StateContainer { get; private set; }

        private DeviceHeaderEntity header = null!;

        public DeviceHeaderViewModel(DeviceHeaderEntity header)
        {
            StateContainer = new DeviceHeaderStateViewModel(header.State);
            Header = header ?? throw new ArgumentNullException(nameof(header));
        }

        public DeviceHeaderViewModel()
            : this(new DeviceHeaderEntity()) { }
    }
}
