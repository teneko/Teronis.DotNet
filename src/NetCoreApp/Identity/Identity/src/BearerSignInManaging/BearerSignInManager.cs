using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Teronis.Identity.Entities;

namespace Teronis.Identity.BearerSignInManaging
{
    public class BearerSignInManager : BearerSignInManager<UserEntity, BearerTokenEntity>
    {
        public BearerSignInManager(IOptions<BearerSignInManagerOptions> options, UserManager<UserEntity> userManager,
            IOptions<IdentityOptions> identityOptions, IBearerTokenStore bearerTokenStore,
            ILogger<BearerSignInManager>? logger = null)
            : base(options, userManager, identityOptions, bearerTokenStore, logger)
        { }

        protected override BearerTokenEntity CreateRefreshToken(string userId, DateTime issuedAtUtc, DateTime expiresAtUtc) =>
            new BearerTokenEntity(userId, issuedAtUtc, expiresAtUtc);
    }
}
