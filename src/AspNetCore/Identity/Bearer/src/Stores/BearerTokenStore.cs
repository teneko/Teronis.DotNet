using Microsoft.EntityFrameworkCore;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.Bearer.Stores
{
    public class BearerTokenStore<DbContextType> : BearerTokenStore<DbContextType, BearerTokenEntity>, IBearerTokenStore
        where DbContextType : DbContext
    {
        public BearerTokenStore(DbContextType dbContext)
            : base(dbContext) { }
    }
}
