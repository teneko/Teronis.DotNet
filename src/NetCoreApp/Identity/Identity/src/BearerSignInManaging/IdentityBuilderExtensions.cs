using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Teronis.Extensions;
using Teronis.Identity.BearerSignInManaging;
using Teronis.Identity.Entities;

namespace Teronis.Identity
{
    public static partial class IdentityBuilderExtensions
    {
        private static void addBearerSignInManagerOptions(IdentityBuilder identityBuilder, Action<BearerSignInManagerOptions>? configureOptions = null)
        {
            var services = identityBuilder.Services;
            // Both are used by underlying services.
            services.AddLogging();
            services.AddOptions();

            if (configureOptions != null) {
                services.Configure(configureOptions);
            }

            services.PostConfigure<BearerSignInManagerOptions>(options => {
                options.CreateDefaultedTokenDescriptor ??= () => new SecurityTokenDescriptor();
                SecurityTokenDescriptor defaultTokenDescriptor = options.CreateDefaultedTokenDescriptor() ?? new SecurityTokenDescriptor();

                // Only those values are taken over whose property values are null.
                void TakeOverFromDefaultOptions(SecurityTokenDescriptor tokenDescriptor)
                {
                    // Claims can be null..
                    if (tokenDescriptor.Claims.IsNullOrEmpty() && defaultTokenDescriptor.Claims.IsNullOrEmpty()) {
                        tokenDescriptor.Claims = new Dictionary<string, object>();
                    }
                    // Take over claims.
                    else if (tokenDescriptor.Claims.IsNullOrEmpty() && !defaultTokenDescriptor.Claims.IsNullOrEmpty()) {
                        tokenDescriptor.Claims = new Dictionary<string, object>(defaultTokenDescriptor.Claims);
                    }
                    // Add defaulted claims to existing claims.
                    else if (!tokenDescriptor.Claims.IsNullOrEmpty() && !defaultTokenDescriptor.Claims.IsNullOrEmpty()) {
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
        }

        public static IdentityBuilder AddBearerSignInManager<DbContextType>(this IdentityBuilder identityBuilder, Action<BearerSignInManagerOptions>? configureOptions = null)
            where DbContextType : DbContext
        {
            addBearerSignInManagerOptions(identityBuilder, configureOptions);
            var services = identityBuilder.Services;
            services.AddScoped<BearerSignInManager>();
            services.AddScoped<BearerSignInManager<UserEntity, BearerTokenEntity>>(serviceProvider => serviceProvider.GetRequiredService<BearerSignInManager>());
            services.AddScoped<IBearerSignInManager>(serviceProvider => serviceProvider.GetRequiredService<BearerSignInManager>());
            AddBearerSignInStores<DbContextType>(identityBuilder);
            return identityBuilder;
        }
    }
}
