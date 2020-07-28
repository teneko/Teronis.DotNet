using Microsoft.EntityFrameworkCore;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Bearer.Stores
{
    public class BearerTokenStore<DbContextType> : BearerTokenStore<DbContextType, BearerTokenEntity>, IBearerTokenStore
        where DbContextType : DbContext
    {
        public BearerTokenStore(DbContextType dbContext)
            : base(dbContext) { }
    }
}
