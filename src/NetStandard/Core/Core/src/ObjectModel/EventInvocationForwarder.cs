using System;

namespace Teronis.ObjectModel
{
    public abstract class EventInvocationForwarder<ForwardingEventContainerType, EventArgumentType>
    {
        protected event Action<object, EventArgumentType>? EventInvocationForward;

        public ForwardingEventContainerType ForwardingEventContainer { get; }

        protected abstract Action<object, EventArgumentType>? ForwardEventInvocation { get; }

        protected readonly object? AlternativeEventSender;

        public EventInvocationForwarder(ForwardingEventContainerType forwardingEventContainer, object? alternativeEventSender = null)
        {
            ForwardingEventContainer = forwardingEventContainer;
            AlternativeEventSender = alternativeEventSender;
        }

        protected abstract bool CanForwardEventInvocation(EventArgumentType eventArgument);

        protected abstract EventArgumentType CreateEventArgument(EventArgumentType eventArgument);

        protected void OnEventInvocationForward(object sender, EventArgumentType eventArgument)
        {
            if (CanForwardEventInvocation(eventArgument)) {
                return;
            }

            sender = AlternativeEventSender ?? sender;
            eventArgument = CreateEventArgument(eventArgument);
            ForwardEventInvocation?.Invoke(sender, eventArgument);
            EventInvocationForward?.Invoke(sender, eventArgument);
        }
    }
}
