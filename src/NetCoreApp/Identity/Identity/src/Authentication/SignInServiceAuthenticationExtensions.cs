using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ZNetCS.AspNetCore.Authentication.Basic.Events;
using Teronis.Identity.Authentication.Extensions;
using Teronis.Identity.Authentication.Tools;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Authentication
{
    public static class SignInServiceAuthenticationExtensions
    {
        /// <summary>
        /// Uses <see cref="BasicAuthenticationDefaults.AuthenticationScheme"/> as scheme.
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddIdentityBasicAuthentication(this AuthenticationBuilder authenticationBuilder)
        {
            authenticationBuilder.AddBasicAuthentication(options => {
                options.Events = new BasicAuthenticationEvents()
                   .AddAuthenticateWhenValidatePrincipal();
            });

            return authenticationBuilder;
        }

        private static void validateSignInServiceAuthenticationOptions(SignInServiceAuthenticationOptions options)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));
            Validator.ValidateObject(options, new ValidationContext(options), true);
        }

        private static AuthenticationBuilder addIdentitySignInServiceRefreshTokenJwtBearer<BearerTokenType>(AuthenticationBuilder authenticationBuilder, [DisallowNull] Action<Action<JwtBearerOptions>> addJwtBearer, [DisallowNull] SignInServiceAuthenticationOptions options)
            where BearerTokenType : class, IBearerTokenEntity
        {
            validateSignInServiceAuthenticationOptions(options);

            addJwtBearer(jwtBearerOptions => {
                jwtBearerOptions.RequireHttpsMetadata = false;

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = options.TokenSigningKey,
                    /// Is mandatory for <see cref="TokenValidatedContextTools.ValidateRefreshTokenIdClaim"/>.
                    SaveSigninToken = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                jwtBearerOptions.Events = new JwtBearerEvents()
                   .WhenTokenValidated(
                       // The order matters! When validating, the user
                       // related identity is added to the claims principal.
                       TokenValidatedContextTools.ValidateRefreshTokenIdClaim<BearerTokenType>,
                       TokenValidatedContextTools.ValidateSecurityStamp);
            });

            return authenticationBuilder;
        }

        public static AuthenticationBuilder AddIdentitySignInServiceRefreshTokenJwtBearer<BearerTokenType>(AuthenticationBuilder authenticationBuilder, string authenticationScheme, [DisallowNull] SignInServiceAuthenticationOptions options)
            where BearerTokenType : class, IBearerTokenEntity
        {
            authenticationScheme = authenticationScheme ?? throw new ArgumentNullException(nameof(authenticationScheme));

            return addIdentitySignInServiceRefreshTokenJwtBearer<BearerTokenType>(authenticationBuilder,
                configureOptions => authenticationBuilder.AddJwtBearer(authenticationScheme, configureOptions),
                options);
        }



        public static AuthenticationBuilder AddIdentitySignInServiceRefreshTokenJwtBearer<BearerTokenType, UserIdType>(AuthenticationBuilder authenticationBuilder, SignInServiceAuthenticationOptions options)
            where BearerTokenType : class, IBearerTokenEntity
        {
            return addIdentitySignInServiceRefreshTokenJwtBearer<BearerTokenType>(authenticationBuilder,
                configureOptions => authenticationBuilder.AddJwtBearer(SignInServiceAuthenticationDefaults.RefreshTokenBearerScheme, configureOptions),
                options);
        }

        private static AuthenticationBuilder addIdentitySignInServiceAccessTokenJwtBearer(AuthenticationBuilder authenticationBuilder, Action<Action<JwtBearerOptions>> addJwtBearer, [DisallowNull] SignInServiceAuthenticationOptions options)
        {
            validateSignInServiceAuthenticationOptions(options);

            addJwtBearer(jwtBearerOptions => {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters() {
                    IssuerSigningKey = options.TokenSigningKey,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            return authenticationBuilder;
        }

        public static AuthenticationBuilder AddIdentitySignInServiceAccessTokenJwtBearer(AuthenticationBuilder authenticationBuilder, string authenticationScheme, [DisallowNull] SignInServiceAuthenticationOptions options)
        {
            authenticationScheme = authenticationScheme ?? throw new ArgumentNullException(nameof(authenticationScheme));

            return addIdentitySignInServiceAccessTokenJwtBearer(authenticationBuilder,
                configureOptions => authenticationBuilder.AddJwtBearer(authenticationScheme, configureOptions),
                options);
        }

        public static AuthenticationBuilder AddIdentitySignInServiceAccessTokenJwtBearer(AuthenticationBuilder authenticationBuilder, SignInServiceAuthenticationOptions options)
        {
            return addIdentitySignInServiceAccessTokenJwtBearer(authenticationBuilder,
                configureOptions => authenticationBuilder.AddJwtBearer(SignInServiceAuthenticationDefaults.AccessTokenBearerScheme, configureOptions),
                options);
        }
    }
}
