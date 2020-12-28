namespace Teronis.ObjectModel.Parenting
{
    public interface IHaveRegisteredParents : IHaveParents
    {
        void RegisterParent(object caller, ParentsRequestedEventHandler handler);
        void RegisterParent(ParentsRequestedEventHandler handler);
        void UnregisterParent(object caller);
        void UnregisterParent(ParentsRequestedEventHandler handler);
    }
}
