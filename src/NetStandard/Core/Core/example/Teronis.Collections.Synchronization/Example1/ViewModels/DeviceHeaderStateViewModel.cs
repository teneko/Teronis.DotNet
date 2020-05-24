using System.Threading.Tasks;
using Teronis.Collections.Synchronization.Example1.Models;
using Teronis.ObjectModel.Updates;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public class DeviceHeaderStateViewModel : ViewModelBase<DeviceHeaderStateViewModel, DeviceHeaderStateEntity>
    {
        public DeviceHeaderStateEntity State { get; private set; }

        public DeviceHeaderStateViewModel(DeviceHeaderStateEntity headerState)
        {
            State = headerState ?? new DeviceHeaderStateEntity();
        }

        public DeviceHeaderStateViewModel()
            : this(null) { }

        protected override async Task UpdateContentByAsync(IContentUpdate<DeviceHeaderStateEntity> update)
        {
            State = await update.ContentTask;
        }
    }
}
