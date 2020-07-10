using System;
using System.Threading.Tasks;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.Extensions;
using Teronis.ObjectModel.Updates;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceBodyViewModel : ViewModelBase<DeviceBodyViewModel, DeviceBodyEntity>
    {
        public DeviceBodyEntity Body { get; set; }

        public DeviceBodyViewModel(DeviceBodyEntity deviceBody)
        {
            Body = deviceBody ?? throw new ArgumentNullException(nameof(deviceBody));
        }

        protected override async Task UpdateContentByAsync(IContentUpdate<DeviceBodyEntity> update)
        {
            var updateContent = await update.ContentTask ?? typeof(DeviceBodyEntity).InstantiateUninitializedObject<DeviceBodyEntity>();
            Body = updateContent;
        }
    }
}
