using System;

namespace Teronis.ObjectModel
{
    public abstract class EventInvocationForwarder<SenderType, EventArgumentType>
    {
        protected event Action<SenderType, EventArgumentType>? EventInvocationForward;

        protected abstract Action<SenderType, EventArgumentType>? ForwardEventInvocation { get; }

        protected readonly SenderType AlternativeEventSender;

        public EventInvocationForwarder(SenderType alternativeEventSender) =>
            AlternativeEventSender = alternativeEventSender;

        protected abstract bool CanForwardEventInvocation(EventArgumentType eventArgument);

        protected abstract EventArgumentType CreateEventArgument(EventArgumentType eventArgument);

        protected void OnEventInvocationForward(SenderType sender, EventArgumentType eventArgument)
        {
            if (CanForwardEventInvocation(eventArgument)) {
                return;
            }

            sender = AlternativeEventSender ?? sender;
            eventArgument = CreateEventArgument(eventArgument);
            ForwardEventInvocation?.Invoke(sender!, eventArgument);
            EventInvocationForward?.Invoke(sender!, eventArgument);
        }
    }
}
