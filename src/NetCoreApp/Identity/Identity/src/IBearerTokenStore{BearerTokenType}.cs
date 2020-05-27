using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Identity.Entities;

namespace Teronis.Identity.BearerSignInManaging
{
    public interface IBearerTokenStore<BearerTokenType>
        where BearerTokenType : class, IBearerTokenEntity
    {
        ValueTask<List<BearerTokenType>> GetUserTokensAsync(string userId, CancellationToken cancellationToken = default);
        ValueTask<BearerTokenType> FindAsync(Guid bearerTokenId, CancellationToken cancellationToken = default);
        Task InsertAsync(BearerTokenType refreshTokenEntity, bool throwOnDuplication = true, CancellationToken cancellationToken = default);
        Task<bool> TryDeleteAsync(BearerTokenType refreshTokenEntity, CancellationToken cancellationToken = default);
        Task<bool> TryDeleteAsync(Guid refreshTokenId, CancellationToken cancellationToken = default);
        Task DeleteExpiredOnesAsync(CancellationToken cancellationToken = default);
    }
}
