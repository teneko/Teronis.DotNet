namespace Teronis.ObjectModel.Parenthood
{
    public interface IHaveParents
    {
        event ParentsRequestedEventHandler ParentsRequested;

        ParentsCollector CreateParentsCollector();
    }
}
