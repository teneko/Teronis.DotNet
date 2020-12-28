namespace Teronis.Collections.Synchronization
{
    internal class ContentUpdateWithTargetContainer<ContentType, TargetType>
    {
        public ContentType ContentUpdate { get; private set; }
        public TargetType Target { get; private set; }

        public ContentUpdateWithTargetContainer(ContentType contentUpdate, TargetType target)
        {
            ContentUpdate = contentUpdate;
            Target = target;
        }
    }
}
