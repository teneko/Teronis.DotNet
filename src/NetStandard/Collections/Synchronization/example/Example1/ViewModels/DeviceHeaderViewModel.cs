using System;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.ObjectModel.TreeColumn;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceHeaderViewModel : ViewModelBase
    {
        [HasTreeColumns]
        public DeviceHeaderEntity Header {
            get => header;

            set {
                OnPropertyChanging();
                header = value;
                StateContainer.State = header.State;
                OnPropertyChanged();
            }
        }

        [HasTreeColumns]
        public DeviceHeaderStateViewModel StateContainer { get; private set; }

        private DeviceHeaderEntity header;

        public DeviceHeaderViewModel(DeviceHeaderEntity header)
        {
            StateContainer = new DeviceHeaderStateViewModel(header.State);
            Header = header ?? throw new ArgumentNullException(nameof(header));
        }

        public DeviceHeaderViewModel()
            : this(new DeviceHeaderEntity()) { }
    }
}
