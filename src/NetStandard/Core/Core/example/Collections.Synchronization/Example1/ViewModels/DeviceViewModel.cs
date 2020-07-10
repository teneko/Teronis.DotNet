using System;
using System.Threading.Tasks;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.ObjectModel.Updates;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceViewModel : ViewModelBase<DeviceViewModel, DeviceHeaderViewModel>
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

        protected override Task UpdateContentByAsync(IContentUpdate<DeviceHeaderViewModel> update)
            => throw new NotImplementedException();
    }
}
