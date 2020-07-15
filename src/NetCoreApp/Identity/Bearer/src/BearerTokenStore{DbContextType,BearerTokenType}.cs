using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teronis.Identity.Entities;

namespace Teronis.Identity.BearerSignInManaging
{
    public class BearerTokenStore<DbContextType, BearerTokenType> : IBearerTokenStore<BearerTokenType>
        where DbContextType : DbContext
        where BearerTokenType : class, IBearerTokenEntity
    {
        private readonly DbSet<BearerTokenType> bearerTokenSet;
        private readonly DbContextType dbContext;

        public BearerTokenStore(DbContextType dbContext)
        {
            bearerTokenSet = dbContext.Set<BearerTokenType>();
            this.dbContext = dbContext;
        }

        public ValueTask<BearerTokenType> FindAsync(Guid bearerTokenId, CancellationToken cancellationToken = default) =>
            bearerTokenSet.FindAsync(new object[] { bearerTokenId }, cancellationToken);

        public ValueTask<List<BearerTokenType>> GetUserTokensAsync(string userId, CancellationToken cancellationToken = default) =>
            bearerTokenSet.AsAsyncEnumerable().Where(x => x.UserId.Equals(userId)).ToListAsync(cancellationToken);

        public async Task InsertAsync(BearerTokenType refreshTokenEntity, bool throwOnDuplication = true, CancellationToken cancellationToken = default)
        {
            await bearerTokenSet.AddAsync(refreshTokenEntity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> TryDeleteAsync(BearerTokenType refreshTokenEntity, CancellationToken cancellationToken = default)
        {
            try {
                bearerTokenSet.Remove(refreshTokenEntity);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            } catch {
                return false;
            }
        }

        public async Task<bool> TryDeleteAsync(Guid refreshTokenId, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(refreshTokenId, cancellationToken);

            if (!ReferenceEquals(entity, null)) {
                dbContext.Remove(entity);
                return true;
            }

            return false;
        }

        public async Task DeleteExpiredOnesAsync(CancellationToken cancellationToken = default)
        {
            var entities = await bearerTokenSet.AsAsyncEnumerable()
                .Where(x => x.ExpiresAtUtc < DateTime.UtcNow)
                .ToListAsync();

            bearerTokenSet.RemoveRange(entities);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
