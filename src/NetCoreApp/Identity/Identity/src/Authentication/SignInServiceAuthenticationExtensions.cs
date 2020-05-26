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

        private static void validateSignInServiceAuthenticationOptions([DisallowNull]SignInServiceAuthenticationOptions options)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));
            Validator.ValidateObject(options, new ValidationContext(options), true);
        }

        private static AuthenticationBuilder addIdentitySignInServiceRefreshTokenJwtBearer(AuthenticationBuilder authenticationBuilder, [DisallowNull]Action<Action<JwtBearerOptions>> addJwtBearer, [DisallowNull]SignInServiceAuthenticationOptions options)
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
                       TokenValidatedContextTools.ValidateRefreshTokenIdClaim,
                       TokenValidatedContextTools.ValidateSecurityStamp);
            });

            return authenticationBuilder;
        }

        public static AuthenticationBuilder AddIdentitySignInServiceRefreshTokenJwtBearer(AuthenticationBuilder authenticationBuilder, [DisallowNull]string authenticationScheme, [DisallowNull]SignInServiceAuthenticationOptions options)
        {
            authenticationScheme = authenticationScheme ?? throw new ArgumentNullException(nameof(authenticationScheme));

            return addIdentitySignInServiceRefreshTokenJwtBearer(authenticationBuilder,
                configureOptions => authenticationBuilder.AddJwtBearer(authenticationScheme, configureOptions),
                options);
        }



        public static AuthenticationBuilder AddIdentitySignInServiceRefreshTokenJwtBearer(AuthenticationBuilder authenticationBuilder, [DisallowNull]SignInServiceAuthenticationOptions options)
        {
            return addIdentitySignInServiceRefreshTokenJwtBearer(authenticationBuilder,
                configureOptions => authenticationBuilder.AddJwtBearer(SignInServiceAuthenticationDefaults.RefreshTokenBearerScheme, configureOptions),
                options);
        }

        private static AuthenticationBuilder addIdentitySignInServiceAccessTokenJwtBearer(AuthenticationBuilder authenticationBuilder, [DisallowNull]Action<Action<JwtBearerOptions>> addJwtBearer, [DisallowNull]SignInServiceAuthenticationOptions options)
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

        public static AuthenticationBuilder AddIdentitySignInServiceAccessTokenJwtBearer(AuthenticationBuilder authenticationBuilder, [DisallowNull]string authenticationScheme, [DisallowNull]SignInServiceAuthenticationOptions options)
        {
            authenticationScheme = authenticationScheme ?? throw new ArgumentNullException(nameof(authenticationScheme));

            return addIdentitySignInServiceAccessTokenJwtBearer(authenticationBuilder,
                configureOptions => authenticationBuilder.AddJwtBearer(authenticationScheme, configureOptions),
                options);
        }

        public static AuthenticationBuilder AddIdentitySignInServiceAccessTokenJwtBearer(AuthenticationBuilder authenticationBuilder, [DisallowNull]SignInServiceAuthenticationOptions options)
        {
            return addIdentitySignInServiceAccessTokenJwtBearer(authenticationBuilder,
                configureOptions => authenticationBuilder.AddJwtBearer(SignInServiceAuthenticationDefaults.AccessTokenBearerScheme, configureOptions),
                options);
        }
    }
}
