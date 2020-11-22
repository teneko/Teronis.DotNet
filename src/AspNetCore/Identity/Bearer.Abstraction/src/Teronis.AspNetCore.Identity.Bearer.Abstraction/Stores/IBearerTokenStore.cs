using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.Bearer.Stores
{
    public interface IBearerTokenStore : IBearerTokenStore<BearerTokenEntity>
    { }
}
