using System;
using Teronis.AspNetCore.Identity.Bearer.Stores;

namespace Teronis.AspNetCore.Identity.Bearer
{
    public class BearerSignInManagerOptions
    {
        public bool IncludeErrorDetails { get; set; }
        public bool SetDefaultTimesOnTokenCreation { get; set; } = false;

        /// <summary>
        /// Non-null values (if they have been set) are applied to 
        /// those values of the other token descriptors who are null.
        /// </summary>
        public Func<BearerTokenDescriptor> CreateDefaultedTokenDescriptor { get; set; } = null!;
        public Func<BearerTokenDescriptor> CreateAccessTokenDescriptor { get; set; } = null!;
        public Func<BearerTokenDescriptor> CreateRefreshTokenDescriptor { get; set; } = null!;
    }
}
