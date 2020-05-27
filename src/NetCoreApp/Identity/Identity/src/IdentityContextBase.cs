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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RefreshTokenEntity>(options => {
                options.HasKey(x => x.RefreshTokenId);

                options.HasOne(x => x.User)
                    .WithMany(x => x!.RefreshTokens)
                    .HasForeignKey(x => x.UserId);
            });

            base.OnModelCreating(builder);
        }
    }
}
