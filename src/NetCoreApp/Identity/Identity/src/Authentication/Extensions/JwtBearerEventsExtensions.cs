using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Teronis.Identity.Authentication.Extensions
{
    public static class JwtBearerEventsExtensions
    {
        private static JwtBearerEvents buildStackedEventHandler<T>(JwtBearerEvents events, Action<Func<T, Task>> setEvent, params Func<T, Task>[] eventContextHandler)
            where T : ResultContext<JwtBearerOptions>
        {
            setEvent(async (context) => {
                foreach (var handler in eventContextHandler) {
                    if (handler != null)
                        await handler.Invoke(context);

                    // We want to break if result has been set.
                    if (context.Result != null)
                        break;
                }
            });

            return events;
        }

        public static JwtBearerEvents WhenMessageReceived(this JwtBearerEvents events, params Func<MessageReceivedContext, Task>[] messageReceivedContextHandler) =>
            buildStackedEventHandler(events,
                eventContextHandler => events.OnMessageReceived = eventContextHandler,
                messageReceivedContextHandler);

        public static JwtBearerEvents WhenTokenValidated(this JwtBearerEvents events, params Func<TokenValidatedContext, Task>[] tokenValidatedContextHandler) =>
            buildStackedEventHandler(events,
                eventContextHandler => events.OnTokenValidated = eventContextHandler,
                tokenValidatedContextHandler);
    }
}
