using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Teronis.Identity.BearerSignInManaging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BearerSignInManagerExtensions
    {
        public static IServiceCollection AddSignInService(this IServiceCollection services, Action<BearerSignInManagerOptions>? configureOptions)
        {
            // Both are used by underlying services.
            services.AddLogging();
            services.AddOptions();

            if (configureOptions != null) {
                services.Configure(configureOptions);
            }

            services.PostConfigure<BearerSignInManagerOptions>(options => {
                options.CreateDefaultedTokenDescriptor ??= () => new SecurityTokenDescriptor();
                var defaultTokenDescriptor = options.CreateDefaultedTokenDescriptor();

                // Only those values are taken over whose property values are null.
                void TakeOverFromDefaultOptions(SecurityTokenDescriptor tokenDescriptor)
                {
                    // Claims can be null..
                    if (tokenDescriptor.Claims == null && defaultTokenDescriptor.Claims == null) {
                        tokenDescriptor.Claims = new Dictionary<string, object>();
                    }
                    // Take over claims.
                    else if (tokenDescriptor.Claims == null && defaultTokenDescriptor.Claims != null) {
                        tokenDescriptor.Claims = new Dictionary<string, object>(defaultTokenDescriptor.Claims);
                    }
                    // Add defaulted claims to existing claims.
                    else if (tokenDescriptor.Claims != null && defaultTokenDescriptor.Claims != null) {
                        foreach (var claim in (ICollection<KeyValuePair<string, object>>)defaultTokenDescriptor.Claims) {
                            tokenDescriptor.Claims.Add(claim);
                        }
                    }

                    tokenDescriptor.Issuer ??= defaultTokenDescriptor.Issuer;
                    tokenDescriptor.Audience ??= defaultTokenDescriptor.Audience;
                    tokenDescriptor.Subject ??= defaultTokenDescriptor.Subject;
                    tokenDescriptor.NotBefore ??= defaultTokenDescriptor.NotBefore;
                    tokenDescriptor.IssuedAt ??= defaultTokenDescriptor.IssuedAt;
                    tokenDescriptor.SigningCredentials ??= defaultTokenDescriptor.SigningCredentials;
                }

                var createAccessTokenDescriptor = options.CreateAccessTokenDescriptor ??=
                    () => new SecurityTokenDescriptor();

                options.CreateAccessTokenDescriptor = () => {
                    var tokenDescriptor = createAccessTokenDescriptor();
                    tokenDescriptor.Expires ??= defaultTokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(15);
                    TakeOverFromDefaultOptions(tokenDescriptor);
                    return tokenDescriptor;
                };

                var createRefreshTokenDescriptor = options.CreateRefreshTokenDescriptor ??=
                    () => new SecurityTokenDescriptor();

                options.CreateRefreshTokenDescriptor = () => {
                    var tokenDescriptor = createRefreshTokenDescriptor();
                    tokenDescriptor.Expires ??= defaultTokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(7);
                    TakeOverFromDefaultOptions(tokenDescriptor);
                    return tokenDescriptor;
                };
            });

            services.AddScoped<BearerSignInManager>();
            return services;
        }

        public static IServiceCollection AddSignInService(this IServiceCollection services) =>
            AddSignInService(services, null);
    }
}
