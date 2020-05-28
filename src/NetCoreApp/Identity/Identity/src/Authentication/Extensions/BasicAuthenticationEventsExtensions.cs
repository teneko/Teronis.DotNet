using Teronis.Identity.Authentication.Tools;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace Teronis.Identity.Authentication.Extensions
{
    public static class BasicAuthenticationEventsExtensions
    {
        /// <summary>
        /// The property <see cref="BasicAuthenticationEvents.OnValidatePrincipal"/> will be overridden.
        /// </summary>
        public static BasicAuthenticationEvents UseAuthenticateWhenValidatePrincipal(this BasicAuthenticationEvents events)
        {
            events.OnValidatePrincipal = BasicAuthenticationEventsTools.ValidatePrincipal;
            return events;
        }
    }
}
