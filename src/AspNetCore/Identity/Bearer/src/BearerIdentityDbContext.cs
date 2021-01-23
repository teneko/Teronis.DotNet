using Microsoft.EntityFrameworkCore;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.Bearer
{
    public class BearerIdentityDbContext : BearerIdentityDbContext<UserEntity, RoleEntity>
    {
        public BearerIdentityDbContext()
            : base() { }

        public BearerIdentityDbContext(DbContextOptions options)
            : base(options) { }
    }
}
