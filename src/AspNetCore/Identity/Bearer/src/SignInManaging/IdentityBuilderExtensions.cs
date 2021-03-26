// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Teronis.AspNetCore.Identity.Entities;
using Teronis.AspNetCore.Identity.Bearer.Stores;

namespace Teronis.AspNetCore.Identity.Bearer.SignInManaging
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
                options.CreateDefaultedTokenDescriptor ??= () => new BearerTokenDescriptor(null);
                BearerTokenDescriptor defaultTokenDescriptor = options.CreateDefaultedTokenDescriptor() ?? new BearerTokenDescriptor(null);

                // Only those values are taken over whose property values are null.
                void TakeOverFromDefaultOptions(SecurityTokenDescriptor tokenDescriptor)
                {
                    var isTokenDescriptorClaimsInvalid = tokenDescriptor.Claims is null || tokenDescriptor.Claims.Count == 0;
                    var isDefaultTokenDescriptorClaimsInvalid = defaultTokenDescriptor.Claims is null || defaultTokenDescriptor.Claims.Count == 0;

                    // Claims can be null..
                    if (isTokenDescriptorClaimsInvalid && isDefaultTokenDescriptorClaimsInvalid) {
                        tokenDescriptor.Claims = new Dictionary<string, object>();
                    }
                    // Take over claims.
                    else if (isTokenDescriptorClaimsInvalid && !isDefaultTokenDescriptorClaimsInvalid) {
                        tokenDescriptor.Claims = new Dictionary<string, object>(defaultTokenDescriptor.Claims!);
                    }
                    // Add defaulted claims to existing claims.
                    else if (!isTokenDescriptorClaimsInvalid && !isDefaultTokenDescriptorClaimsInvalid) {
                        foreach (var claim in (ICollection<KeyValuePair<string, object>>)defaultTokenDescriptor.Claims!) {
                            tokenDescriptor.Claims!.Add(claim);
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
                    () => new BearerTokenDescriptor(null);

                options.CreateAccessTokenDescriptor = () => {
                    var tokenDescriptor = createAccessTokenDescriptor();
                    tokenDescriptor.Expires ??= defaultTokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(15);
                    TakeOverFromDefaultOptions(tokenDescriptor);
                    return tokenDescriptor;
                };

                var createRefreshTokenDescriptor = options.CreateRefreshTokenDescriptor ??=
                    () => new BearerTokenDescriptor(null);

                options.CreateRefreshTokenDescriptor = () => {
                    var tokenDescriptor = createRefreshTokenDescriptor();
                    tokenDescriptor.Expires ??= defaultTokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(7);
                    TakeOverFromDefaultOptions(tokenDescriptor);
                    return tokenDescriptor;
                };
            });
        }

        /// <summary>
        /// Adds <see cref="IBearerSignInManager"/> created from <paramref name="bearerSignInManagerFactory"/> to services.
        /// </summary>
        /// <typeparam name="DbContextType">Type of database context.</typeparam>
        /// <typeparam name="UserType">Type of user entity.</typeparam>
        /// <typeparam name="RoleType">Type of role entity.</typeparam>
        /// <param name="identityBuilder">The identity builder.</param>
        /// <param name="bearerSignInManagerFactory">A <see cref="IBearerSignInManager"/> factory.</param>
        /// <param name="configureOptions">Configures options.</param>
        /// <returns></returns>
        public static IdentityBuilder AddBearerSignInManager<DbContextType, UserType, RoleType>(this IdentityBuilder identityBuilder, Func<IServiceProvider, IBearerSignInManager> bearerSignInManagerFactory,
            Action<BearerSignInManagerOptions>? configureOptions = null)
            where DbContextType : DbContext
            where UserType : class
            where RoleType : class
        {
            addBearerSignInManagerOptions(identityBuilder, configureOptions);
            var services = identityBuilder.Services;
            services.AddScoped(bearerSignInManagerFactory);
            return identityBuilder;
        }

        public static IdentityBuilder AddBearerSignInManager<DbContextType>(this IdentityBuilder identityBuilder, Action<BearerSignInManagerOptions>? configureOptions = null)
            where DbContextType : DbContext
        {
            var services = identityBuilder.Services;
            services.AddScoped<BearerSignInManager>();

            static IBearerSignInManager getRequiredService(IServiceProvider serviceProvider) =>
                serviceProvider.GetRequiredService<BearerSignInManager>();

            identityBuilder.AddBearerSignInManager<DbContextType, UserEntity, RoleEntity>(getRequiredService, configureOptions);
            return identityBuilder;
        }
    }
}
