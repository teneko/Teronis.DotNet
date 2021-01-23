namespace Teronis.ObjectModel.ContentUpdates
{
    public abstract class ContentUpdater<ContentContainerType, ContentType> : IApplyContentUpdate<ContentContainerType, ContentType>, INotifyContentUpdating<ContentType>, INotifyContentUpdated<ContentType>
        where ContentContainerType : notnull
    {
        public event ContentUpdatingEventHandler<ContentType>? ContentUpdating;
        public event ContentUpdatedEventHandler<ContentType>? ContentUpdated;

        public ContentUpdater() { }

        protected virtual void OnContentUpdating(ContentUpdatingEventArgs<ContentType> args)
            => ContentUpdating?.Invoke(this, args);

        protected virtual void OnContentUpdated(ContentUpdatedEventArgs<ContentType> args)
            => ContentUpdated?.Invoke(this, args);

        /// <summary>
        /// Checks if <paramref name="contentUpdate"/> is applicable.
        /// </summary>
        /// <param name="contentUpdate"></param>
        /// <returns></returns>
        public virtual bool IsContentUpdateApplicable(ContentContainerType contentContainer, ContentType contentUpdate)
        {
            var args = new ContentUpdatingEventArgs<ContentType>(contentContainer, contentUpdate);
            OnContentUpdating(args);
            return !args.Handled;
        }

        protected abstract void OnContentUpdate(ContentContainerType contentContainer, ContentType contentUpdate);

        /// <summary>
        /// Applies the content.
        /// </summary>
        /// <param name="contentUpdate">The content that is used to update.</param>
        public bool TryApplyContentUpdate(ContentContainerType contentContainer, ContentType contentUpdate)
        {
            if (IsContentUpdateApplicable(contentContainer, contentUpdate)) {
                OnContentUpdate(contentContainer, contentUpdate);
                OnContentUpdated(new ContentUpdatedEventArgs<ContentType>(contentContainer, contentUpdate));
                return true;
            }

            return false;
        }

        public void ApplyContentUpdate(ContentContainerType contentContainer, ContentType contentUpdate) =>
            TryApplyContentUpdate(contentContainer, contentUpdate);

        void IApplyContentUpdate<ContentContainerType, ContentType>.ApplyContentUpdate(ContentContainerType contentContainer, ContentType contentUpdate) =>
            TryApplyContentUpdate(contentContainer, contentUpdate);
    }
}
