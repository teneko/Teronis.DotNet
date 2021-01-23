namespace Teronis.ObjectModel.ContentUpdates
{
    public interface IApplyContentUpdate<in ContentContainerType, in ContentType>
    {
        void ApplyContentUpdate(ContentContainerType contentContainer, ContentType contentUpdate);
    }
}
