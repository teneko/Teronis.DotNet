// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.ComponentModel
{
    public abstract class EventInvocationForwarder<SenderType, EventArgumentType>
    {
        protected event Action<SenderType, EventArgumentType>? EventInvocationForward;

        protected abstract Action<SenderType, EventArgumentType>? ForwardEventInvocation { get; }

        public virtual bool HasAlternativeEventSender { get; }
        public virtual SenderType AlternativeEventSender { get; }

        public EventInvocationForwarder(SenderType alternativeEventSender)
        {
            HasAlternativeEventSender = true;
            AlternativeEventSender = alternativeEventSender;
        }

        public EventInvocationForwarder() {
            HasAlternativeEventSender = false;
            AlternativeEventSender = default(SenderType)!;
        }

        protected abstract bool CanForwardEventInvocation(EventArgumentType eventArgument);

        protected abstract EventArgumentType CreateEventArgument(EventArgumentType eventArgument);

        protected virtual SenderType GetAlternativeEventSender(SenderType originalSender)
        {
            if (HasAlternativeEventSender) {
                originalSender = AlternativeEventSender;
            }

            return originalSender;
        }

        protected void OnEventInvocationForward(SenderType sender, EventArgumentType eventArgument)
        {
            if (CanForwardEventInvocation(eventArgument)) {
                return;
            }

            sender = GetAlternativeEventSender(sender);
            eventArgument = CreateEventArgument(eventArgument);
            ForwardEventInvocation?.Invoke(sender, eventArgument);
            EventInvocationForward?.Invoke(sender, eventArgument);
        }
    }
}
