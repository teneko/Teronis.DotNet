using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teronis.Identity.Entities;

namespace Teronis.Identity.BearerSignInManaging
{
    public class RefreshTokenStore<DbContextType, RefreshTokenType> : IRefreshTokenStore<RefreshTokenType>
        where DbContextType : DbContext
        where RefreshTokenType : RefreshTokenEntity
    {
        private readonly DbSet<RefreshTokenType> refreshTokenSet;

        public RefreshTokenStore(DbContextType dbContext) =>
            refreshTokenSet = dbContext.Set<RefreshTokenType>();

        public ValueTask<RefreshTokenType> FindAsync(object[] keyValues, CancellationToken cancellationToken = default) =>
            refreshTokenSet.FindAsync(keyValues, cancellationToken);
    }
}
