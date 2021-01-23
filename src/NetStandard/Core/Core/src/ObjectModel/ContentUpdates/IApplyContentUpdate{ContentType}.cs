namespace Teronis.ObjectModel.ContentUpdates
{
    public interface IApplyContentUpdate<in ContentType>
    {
        void ApplyContentUpdate(ContentType contentUpdate);
    }
}
