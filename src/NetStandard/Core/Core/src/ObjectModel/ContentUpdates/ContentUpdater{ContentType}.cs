namespace Teronis.ObjectModel.ContentUpdates
{
    public abstract class ContentUpdater<ContentType> : IApplyContentUpdate<ContentType>, INotifyContentUpdating<ContentType>, INotifyContentUpdated<ContentType>
    {
        public event ContentUpdatingEventHandler<ContentType>? ContentUpdating;
        public event ContentUpdatedEventHandler<ContentType>? ContentUpdated;

        public ContentUpdater() { }

        protected abstract ContentUpdatingEventArgs<ContentType> CreateContentUpdatingEventArgs(ContentType contentUpdate);
        protected abstract ContentUpdatedEventArgs<ContentType> CreateContentUpdatedEventArgs(ContentType contentUpdate);

        protected virtual void OnContentUpdating(ContentUpdatingEventArgs<ContentType> args)
            => ContentUpdating?.Invoke(this, args);

        protected virtual void OnContentUpdated(ContentUpdatedEventArgs<ContentType> args)
            => ContentUpdated?.Invoke(this, args);

        /// <summary>
        /// Checks if <paramref name="contentUpdate"/> is applicable.
        /// </summary>
        /// <param name="contentUpdate"></param>
        /// <returns></returns>
        public virtual bool IsContentUpdateApplicable(ContentType contentUpdate)
        {
            var args = CreateContentUpdatingEventArgs(contentUpdate);
            OnContentUpdating(args);
            return !args.Handled;
        }

        protected abstract void OnContentUpdate(ContentType contentUpdate);

        /// <summary>
        /// Applies the content.
        /// </summary>
        /// <param name="contentUpdate">The content that is used to update.</param>
        public bool TryApplyContentUpdate(ContentType contentUpdate)
        {
            if (IsContentUpdateApplicable(contentUpdate)) {
                OnContentUpdate(contentUpdate);
                OnContentUpdated(CreateContentUpdatedEventArgs(contentUpdate));
                return true;
            }

            return false;
        }

        public void ApplyContentUpdate(ContentType contentUpdate) =>
           TryApplyContentUpdate(contentUpdate);

        void IApplyContentUpdate<ContentType>.ApplyContentUpdate(ContentType contentUpdate) =>
            TryApplyContentUpdate(contentUpdate);
    }
}
