using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Teronis.Identity.Entities;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace Teronis.Identity.Authentication.Tools
{
    public static class BasicAuthenticationEventsTools
    {
        public static async Task ValidatePrincipal<UserType>(ValidatePrincipalContext context)
            where UserType : class, IUserEntity
        {
            var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()?.CreateLogger(nameof(BasicAuthenticationEventsTools));
            var userManager = (UserManager<UserType>)context.HttpContext.RequestServices.GetService(typeof(UserManager<UserEntity>));
            var signInManager = (SignInManager<UserType>)context.HttpContext.RequestServices.GetService(typeof(SignInManager<UserEntity>));
            var userEntity = await userManager.FindByNameAsync(context.UserName);

            if (userEntity is null) {
                var errorMessage = "The user has not been found.";
                logger?.LogInformation(errorMessage);
                context.AuthenticationFailMessage = errorMessage;
                return;
            }

            // First check password, then sign in.
            var signInResult = signInManager.CheckPasswordSignInAsync(userEntity, context.Password, false);

            if (signInResult.Result.Succeeded) {
                // Required by sign in service.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, context.UserName, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(ClaimTypes.NameIdentifier, userEntity.Id, ClaimValueTypes.String)
                };

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                /// If <see cref="ValidatePrincipalContext.Principal"/>
                /// is not null, the user is authenticated successfully.
                context.Principal = principal;
            }
        }
    }
}
