using System;
using Microsoft.IdentityModel.Tokens;

namespace Teronis.Identity.BearerSignInManaging
{
    public class BearerSignInManagerOptions
    {
        public bool SetDefaultTimesOnTokenCreation { get; set; } = false;

        /// <summary>
        /// Non-null values (if they have been set) are applied to 
        /// those values of the other token descriptors who are null.
        /// </summary>
        public Func<SecurityTokenDescriptor> CreateDefaultedTokenDescriptor { get; set; } = null!;
        public Func<SecurityTokenDescriptor> CreateAccessTokenDescriptor { get; set; } = null!;
        public Func<SecurityTokenDescriptor> CreateRefreshTokenDescriptor { get; set; } = null!;
    }
}
