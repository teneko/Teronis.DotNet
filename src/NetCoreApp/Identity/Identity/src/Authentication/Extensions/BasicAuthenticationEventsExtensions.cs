using Teronis.Identity.Authentication.Tools;
using Teronis.Identity.Entities;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace Teronis.Identity.Authentication.Extensions
{
    public static class BasicAuthenticationEventsExtensions
    {
        /// <summary>
        /// The property <see cref="BasicAuthenticationEvents.OnValidatePrincipal"/> will be overridden.
        /// </summary>
        public static BasicAuthenticationEvents UseAuthenticateWhenValidatePrincipal<UserType>(this BasicAuthenticationEvents events)
            where UserType : class, IUserEntity
        {
            events.OnValidatePrincipal = BasicAuthenticationEventsTools.ValidatePrincipal<UserType>;
            return events;
        }
    }
}
