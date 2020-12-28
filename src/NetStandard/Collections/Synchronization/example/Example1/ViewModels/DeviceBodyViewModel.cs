using System;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Extensions;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceBodyViewModel : ViewModelBase
    {
        public DeviceBodyEntity Body {
            get => body;

            set {
                OnPropertyChanging();
                body = value ?? typeof(DeviceBodyEntity).CreateInstanceUninitialized<DeviceBodyEntity>();
                OnPropertyChanged();
            }
        }

        private DeviceBodyEntity body;

        public DeviceBodyViewModel(DeviceBodyEntity deviceBody) =>
            Body = deviceBody ?? throw new ArgumentNullException(nameof(deviceBody));
    }
}
