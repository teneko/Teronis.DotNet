using System;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Reflection;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceBodyViewModel : ViewModelBase
    {
        public DeviceBodyEntity Body {
            get => body;
            set => PropertyChangeComponent.ChangeProperty(ref body, value ?? Instantiator.Instantiate<DeviceBodyEntity>());
        }

        private DeviceBodyEntity body = null!;

        public DeviceBodyViewModel(DeviceBodyEntity deviceBody) =>
            Body = deviceBody ?? throw new ArgumentNullException(nameof(deviceBody));
    }
}
