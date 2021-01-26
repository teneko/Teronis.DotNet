using System;
using System.Collections.Generic;

namespace Teronis.ObjectModel
{
    public class EventInvocationGate<SenderType, ArgumentType> : IPassableEventInvocationGate
    {
        private readonly Action<SenderType, ArgumentType> eventInvoker;
        private List<Invocation> invocations;
        private bool guarded;

        public EventInvocationGate(Action<SenderType, ArgumentType> eventInvoker)
        {
            invocations = new List<Invocation>();
            this.eventInvoker = eventInvoker;
        }

        public void PassThrough(SenderType sender, ArgumentType argument)
        {
            if (guarded) {
                invocations.Add(new Invocation(sender, argument));
            } else {
                eventInvoker?.Invoke(sender, argument);
            }
        }

        void IPassableEventInvocationGate.LetPassThrough()
        {
            foreach (var invocation in invocations) {
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

        private readonly struct Invocation
        {
            public SenderType Sender { get; }
            public ArgumentType Argument { get; }

            public Invocation(SenderType sender, ArgumentType argument)
            {
                Sender = sender;
                Argument = argument;
            }
        }
    }
}
