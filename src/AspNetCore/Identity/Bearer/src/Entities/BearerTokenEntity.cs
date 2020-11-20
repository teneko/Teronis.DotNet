using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.AspNetCore.Identity.Entities
{
    /// <summary>
    /// Kind of read-only entity which represents a refresh token.
    /// </summary>
    public class BearerTokenEntity : IBearerTokenEntity
    {
        public Guid BearerTokenId { get; private set; }
        [NotNull]
        public string UserId { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }

        /// Due to private backed field of <see cref="BearerTokenId"/>.
        /// <see cref="BearerTokenId"/> represents the primary key.
        internal BearerTokenEntity(Guid refreshToken)
        {
            BearerTokenId = refreshToken;
            UserId = default!;
        }

        public BearerTokenEntity(string userId, DateTime createdAtUtc, DateTime expiresAtUtc)
        {
            UserId = userId;
            CreatedAtUtc = createdAtUtc;
            ExpiresAtUtc = expiresAtUtc;
        }
    }
}
