namespace Teronis.ObjectModel.Parenting
{
    public interface IHaveParents
    {
        event ParentsRequestedEventHandler ParentsRequested;

        ParentsCollector CreateParentsCollector();
    }
}
