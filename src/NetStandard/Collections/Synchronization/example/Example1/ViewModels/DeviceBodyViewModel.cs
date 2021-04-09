using System;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Extensions;
using Teronis.Reflection;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceBodyViewModel : ViewModelBase
    {
        public DeviceBodyEntity Body {
            get => body;

            set {
                PropertyChangeComponent.OnPropertyChanging();
                body = value ?? Instantiator.Instantiate<DeviceBodyEntity>();
                PropertyChangeComponent.OnPropertyChanged();
            }
        }

        private DeviceBodyEntity body;

        public DeviceBodyViewModel(DeviceBodyEntity deviceBody) =>
            Body = deviceBody ?? throw new ArgumentNullException(nameof(deviceBody));
    }
}
