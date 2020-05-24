using MorseCode.ITask;
using System.Threading.Tasks;
using Teronis.Data;
using Teronis.ViewModels;
using Teronis.ObjectModel.Updates;
using Teronis.ViewModels.UpdatableContent;

namespace Teronis.Collections.Synchronization.Example1.ViewModels
{
    public abstract class ViewModelBase<ViewModelType, ContentType> : ViewModelBase, IApplyContentUpdate<ContentType>
        where ViewModelType : ViewModelBase<ViewModelType, ContentType>
    {
        protected ContentUpdater Updater { get; set; }

        public ViewModelBase()
            => Updater = new ContentUpdater(WorkStatus, (ViewModelType)this);

        #region IUpdatable Support

        public event ContentUpdatingEventHandler<ContentType> ContainerUpdating {
            add => Updater.ContainerUpdating += value;
            remove => Updater.ContainerUpdating += value;
        }

        public event ContentUpdatedEventHandler<ContentType> ContainerUpdated {
            add => Updater.ContainerUpdated += value;
            remove => Updater.ContainerUpdated += value;
        }

        public bool IsContentUpdateAppliable(IContentUpdate<ContentType> update)
            => Updater.IsContentUpdateAppliable(update);

        public override void BeginWork()
            => Updater.BeginWork();

        protected abstract Task UpdateContentByAsync(IContentUpdate<ContentType> update);

        public Task ApplyContentUpdateByAsync(IContentUpdate<ContentType> update)
            => Updater.ApplyContentUpdateByAsync(update);

        public async Task ApplyContentUpdateByAsync(ITask<IContentUpdate<ContentType>> update)
            // TODO: look here if you need to begin and end work.
            => await Updater.ApplyContentUpdateByAsync(await update);

        public override void EndWork()
            => Updater.EndWork();

        protected class ContentUpdater : ParentalContentUpdater<ViewModelBase<ViewModelType, ContentType>, ContentType>
        {
            public ContentUpdater(WorkStatus workStatus, ViewModelType parent)
                : base(workStatus, parent) { }

            public override Task ApplyContentUpdateByAsync(IContentUpdate<ContentType> update)
                => parent.UpdateContentByAsync(update);
        }

        #endregion
    }
}
