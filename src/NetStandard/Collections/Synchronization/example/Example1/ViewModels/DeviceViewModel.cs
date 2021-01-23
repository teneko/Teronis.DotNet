using System;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
        public DeviceHeaderViewModel HeaderContainer { get; private set; }
        /// <summary>
        /// Belongs to <see cref="Header"/>
        /// </summary>
        public DeviceBodyViewModel BodyContainer { get; private set; }

        public DeviceViewModel(DeviceHeaderViewModel headerContainer)
        {
            BodyContainer = new DeviceBodyViewModel(new DeviceBodyEntity());
            HeaderContainer = headerContainer ?? throw new ArgumentNullException(nameof(headerContainer));
        }
    }
}
