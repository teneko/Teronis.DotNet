namespace Teronis.Identity.Entities
{
    public interface IBearerUserEntity
    {
        string Id { get; }
        string UserName { get; }
        string SecurityStamp { get; }
    }
}
