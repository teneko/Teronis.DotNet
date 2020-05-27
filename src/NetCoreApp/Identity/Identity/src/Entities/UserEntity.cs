using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Teronis.Identity.Entities
{
    public class UserEntity : IdentityUser, IUserEntity
    {
        // EF Core uses backed field by default.
        public IReadOnlyList<BearerTokenEntity> RefreshTokens =>
            refreshTokens.AsReadOnly();

        private List<BearerTokenEntity> refreshTokens;

        public UserEntity()
            : base()
            => refreshTokens = new List<BearerTokenEntity>();

        public UserEntity(string userName)
            : base(userName ?? throw new ArgumentNullException(userName)) =>
            refreshTokens = null!; // Behaviour have to be implemented in each contructor at its own to ensure non nullable.
    }
}
