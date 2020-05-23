using System.Threading.Tasks;
using MorseCode.ITask;
using Teronis.Data;
using Teronis.ObjectModel.Updates;

namespace Teronis.ViewModels.UpdatableContent
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

        public bool IsContentUpdateAppliable(ObjectModel.Updates.IContentUpdate<ContentType> update)
            => Updater.IsContentUpdateAppliable(update);

        public override void BeginWork()
            => Updater.BeginWork();

        protected abstract void InnerUpdateContentBy(ObjectModel.Updates.IContentUpdate<ContentType> update);

        protected virtual Task InnerUpdateContentByAsync(ObjectModel.Updates.IContentUpdate<ContentType> update)
        {
            // Update synchronously as not overridden
            InnerUpdateContentBy(update);
            // And we return a completed task
            // as we do not have to await something
            return Task.CompletedTask;
        }

        public Task ApplyContentUpdateByAsync(ObjectModel.Updates.IContentUpdate<ContentType> update)
            => Updater.ApplyContentUpdateByAsync(update);

        public async Task UpdateContentByAsync(ITask<ObjectModel.Updates.IContentUpdate<ContentType>> update)
            // TODO: look here if you need to begin and end work.
            => await Updater.ApplyContentUpdateByAsync(await update);

        public override void EndWork()
            => Updater.EndWork();

        protected class ContentUpdater : ParentalContentUpdater<ViewModelBase<ViewModelType, ContentType>, ContentType>
        {
            public ContentUpdater(WorkStatus workStatus, ViewModelType parent)
                : base(workStatus, parent) { }

            public override Task ApplyContentUpdateByAsync(ObjectModel.Updates.IContentUpdate<ContentType> update)
                => parent.InnerUpdateContentByAsync(update);
        }

        #endregion
    }
}
