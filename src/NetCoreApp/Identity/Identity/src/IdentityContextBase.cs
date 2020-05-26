using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Teronis.Identity.Entities;

namespace Teronis.Identity
{
    public abstract class IdentityContextBase : IdentityDbContext<UserEntity, RoleEntity, string>
    {
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;

        public IdentityContextBase()
            : base() { }

        public IdentityContextBase(DbContextOptions options)
            : base(options) { }
    }
}
