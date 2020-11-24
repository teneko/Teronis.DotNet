using System;
using System.Threading.Tasks;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.DataModeling.TreeColumn;
using Teronis.Extensions;
using Teronis.ObjectModel.Updates;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceHeaderViewModel : ViewModelBase<DeviceHeaderViewModel, DeviceHeaderEntity>
    {
        [HasTreeColumns]
        public DeviceHeaderEntity Header { get; private set; }

        [HasTreeColumns]
        public DeviceHeaderStateViewModel StateContainer { get; private set; }

        public DeviceHeaderViewModel(DeviceHeaderEntity header)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            StateContainer = new DeviceHeaderStateViewModel(header.State);
        }

        public DeviceHeaderViewModel()
            : this(new DeviceHeaderEntity()) { }

        protected override async Task UpdateContentByAsync(IContentUpdate<DeviceHeaderEntity> update)
        {
            Header = await update.ContentTask;
            var stateUpdate = update.CreateUpdateFromContent(_ => Header.State, this);
            await StateContainer.ApplyContentUpdateByAsync(stateUpdate);
        }
    }
}
