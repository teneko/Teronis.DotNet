namespace Teronis.ObjectModel.ContentUpdates
{
    public abstract class ContainerContentUpdater<ContentContainerType, ContentType> : ContentUpdater<ContentType>
        where ContentContainerType : notnull
    {
        public virtual ContentContainerType ContentContainer { get; } = default!;

        protected ContainerContentUpdater() { }

        public ContainerContentUpdater(ContentContainerType contentContainer) =>
            ContentContainer = contentContainer;

        protected override ContentUpdatingEventArgs<ContentType> CreateContentUpdatingEventArgs(ContentType contentUpdate) =>
            new ContentUpdatingEventArgs<ContentType>(ContentContainer, contentUpdate);

        protected override ContentUpdatedEventArgs<ContentType> CreateContentUpdatedEventArgs(ContentType contentUpdate) =>
            new ContentUpdatedEventArgs<ContentType>(ContentContainer, contentUpdate);
    }
}
