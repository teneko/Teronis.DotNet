using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.Bearer.Stores
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

        /// <summary>
        /// Deletes refresh token.
        /// </summary>
        public Task DeleteAsync(BearerTokenType refreshTokenEntity, CancellationToken cancellationToken = default)
        {
            bearerTokenSet.Remove(refreshTokenEntity);
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes refresh token by identifier.
        /// </summary>
        /// <param name="refreshTokenId">The refresh token identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Token by token identifier not found.</exception>
        public async Task DeleteAsync(Guid refreshTokenId, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(refreshTokenId, cancellationToken);

            if (entity is null) {
                throw new InvalidOperationException("The refresh token does not exist.");
            }

            dbContext.Remove(entity);
        }

        /// <summary>
        /// Deletes refresh token by identifier but skips when the bearer token identifier does not exist.
        /// </summary>
        /// <param name="refreshTokenId">The refresh token identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> TryDeleteAsync(Guid refreshTokenId, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(refreshTokenId, cancellationToken);

            if (!ReferenceEquals(entity, null)) {
                dbContext.Remove(entity);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes expired bearer tokens.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
