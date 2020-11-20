using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.Bearer
{
    public class BearerIdentityDbContext<UserType, RoleType> : IdentityDbContext<UserType, RoleType, string>
        where UserType : IdentityUser<string>
        where RoleType : IdentityRole<string>
    {
        public DbSet<BearerTokenEntity> RefreshTokens { get; set; } = null!;

        public BearerIdentityDbContext()
            : base() { }

        public BearerIdentityDbContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BearerTokenEntity>(options => {
                options.HasKey(x => x.BearerTokenId);
            });

            base.OnModelCreating(builder);
        }
    }
}
