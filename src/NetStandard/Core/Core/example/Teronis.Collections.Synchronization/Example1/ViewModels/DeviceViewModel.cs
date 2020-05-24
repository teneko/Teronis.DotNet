using System;
using System.Threading.Tasks;
using Teronis.ObjectModel.Updates;
using Teronis.Collections.Synchronization.Example1.Models;

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

        public async Task LoadExternalBody()
        {
            // TODO: BodyContainer.ApplyContentUpdateByAsync(..)
        }

        /// <summary>
        /// Load note and populate <see cref="NoteContainer"/>
        /// </summary>
        public async Task LoadExternalNote()
        {
            // TODO: NoteContainer.ApplyContentUpdateByAsync(..)
        }

        public Task LoadExternal()
            => Task.WhenAll(LoadExternalBody(), LoadExternalNote());
    }
}
