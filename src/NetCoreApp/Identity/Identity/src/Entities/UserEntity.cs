using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Teronis.Identity.Entities
{
    public class UserEntity : IdentityUser
    {
        // EF Core uses backed field by default.
        public IReadOnlyList<RefreshTokenEntity> RefreshTokens =>
            refreshTokens.AsReadOnly();

        private List<RefreshTokenEntity> refreshTokens;

        public UserEntity()
            : base()
            => refreshTokens = new List<RefreshTokenEntity>();

        public UserEntity(string userName)
            : base(userName ?? throw new ArgumentNullException(userName)) =>
            refreshTokens = null!; // Behaviour have to be implemented in each contructor at its own to ensure non nullable.

        public bool HasRefreshToken(Guid refreshTokenId, out RefreshTokenEntity? entity)
        {
            foreach (var refreshToken in refreshTokens) {
                if (refreshToken.RefreshTokenId.Equals(refreshTokenId)) {
                    entity = refreshToken;
                    return true;
                }
            }

            entity = default;
            return false;
        }

        public void AttachRefreshToken(RefreshTokenEntity refreshTokenEntity, bool throwOnDuplication = true)
        {
            if (throwOnDuplication && HasRefreshToken(refreshTokenEntity.RefreshTokenId, out _)) {
                throw new ArgumentException("The refresh token does already exist");
            }

            refreshTokens.Add(refreshTokenEntity);
        }

        public bool TryDettachRefreshToken(RefreshTokenEntity refreshTokenEntity)
            => refreshTokens.Remove(refreshTokenEntity);

        public bool TryDettachRefreshToken(Guid refreshTokenId)
        {
            var refreshTokenEntity = refreshTokens.SingleOrDefault(x => x.RefreshTokenId == refreshTokenId);

            if (refreshTokenEntity != null) {
                refreshTokens.Remove(refreshTokenEntity);
                return true;
            }

            return false;
        }
    }
}
