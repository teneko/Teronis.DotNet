using System;

namespace Teronis.Identity.Entities
{
    /// <summary>
    /// Kind of read-only entity which represents a refresh token.
    /// </summary>
    public class RefreshTokenEntity
    {
        public Guid RefreshTokenId { get; private set; }
        public string UserId { get; private set; }
        public UserEntity? User { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }

        /// Due to private backed field of <see cref="RefreshTokenId"/>.
        /// <see cref="RefreshTokenId"/> represents the primary key.
        internal RefreshTokenEntity(Guid refreshToken)
        {
            RefreshTokenId = refreshToken;
            UserId = null!;
        }

        public RefreshTokenEntity(string userId, DateTime createdAtUtc, DateTime expiresAtUtc)
        {
            UserId = userId;
            CreatedAtUtc = createdAtUtc;
            ExpiresAtUtc = expiresAtUtc;
        }
    }
}
