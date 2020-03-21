using System.Threading.Tasks;
using MorseCode.ITask;
using Teronis.Data;

namespace Teronis.ViewModels.UpdatableContent
{
    public abstract class ViewModelBase<ViewModelType, ContentType> : ViewModelBase, IUpdatableContent<ContentType>
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

        public bool IsContentUpdatable(IContentUpdate<ContentType> update)
            => Updater.IsContentUpdatable(update);

        public override void BeginWork()
            => Updater.BeginWork();

        protected abstract void InnerUpdateContentBy(IContentUpdate<ContentType> update);

        protected virtual Task InnerUpdateContentByAsync(IContentUpdate<ContentType> update)
        {
            // Update synchronously as not overridden
            InnerUpdateContentBy(update);
            // And we return a completed task
            // as we do not have to await something
            return Task.CompletedTask;
        }

        public void UpdateContentBy(IContentUpdate<ContentType> update)
            => Updater.UpdateContentBy(update);

        public Task UpdateContentByAsync(IContentUpdate<ContentType> update)
            => Updater.UpdateContentByAsync(update);

        public async Task UpdateContentByAsync(ITask<IContentUpdate<ContentType>> update)
            // TODO: look here if you need to begin and end work.
            => await Updater.UpdateContentByAsync(await update);

        public override void EndWork()
            => Updater.EndWork();

        protected class ContentUpdater : ParentalContentUpdater<ViewModelBase<ViewModelType, ContentType>, ContentType>
        {
            public ContentUpdater(WorkStatus workStatus, ViewModelType parent)
                : base(workStatus, parent) { }

            public override void UpdateContentBy(IContentUpdate<ContentType> update)
                => parent.InnerUpdateContentBy(update);

            public override Task UpdateContentByAsync(IContentUpdate<ContentType> update)
                => parent.InnerUpdateContentByAsync(update);
        }

        #endregion
    }
}
