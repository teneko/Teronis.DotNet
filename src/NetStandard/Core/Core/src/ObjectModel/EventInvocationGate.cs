using System;
using System.Collections.Generic;

namespace Teronis.ObjectModel
{
    public class EventInvocationGate<SenderType, ArgumentType> : IPassableEventInvocationGate
    {
        private readonly Action<SenderType, ArgumentType> eventInvoker;
        private List<EventInvocation> eventInvocations;
        private bool guarded;

        public EventInvocationGate(Action<SenderType, ArgumentType> eventInvoker)
        {
            eventInvocations = new List<EventInvocation>();
            this.eventInvoker = eventInvoker;
        }

        public void PassThrough(SenderType sender, ArgumentType argument)
        {
            if (guarded) {
                eventInvocations.Add(new EventInvocation(sender, argument));
            } else {
                eventInvoker?.Invoke(sender, argument);
            }
        }

        void IPassableEventInvocationGate.LetPassThrough()
        {
            foreach (var invocation in eventInvocations) {
                eventInvoker?.Invoke(invocation.Sender, invocation.Argument);
            }

            guarded = false;
        }

        public EventInvocationGateGuardian Guard()
        {
            if (guarded) {
                throw new InvalidOperationException("The event invocation gate can only have one keeper.");
            }

            return new EventInvocationGateGuardian(this);
        }

        private readonly struct EventInvocation
        {
            public SenderType Sender { get; }
            public ArgumentType Argument { get; }

            public EventInvocation(SenderType sender, ArgumentType argument)
            {
                Sender = sender;
                Argument = argument;
            }
        }
    }
}
