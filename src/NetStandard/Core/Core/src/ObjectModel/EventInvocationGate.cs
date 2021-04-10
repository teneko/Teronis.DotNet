// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.ComponentModel
{
    public class EventInvocationGate<SenderType, ArgumentType> : IReleasableEventInvocationGate
    {
        private readonly Action<SenderType, ArgumentType> eventInvoker;
        private List<EventInvocation> eventInvocations;
        private bool guarded;

        public EventInvocationGate(Action<SenderType, ArgumentType> eventInvoker)
        {
            eventInvocations = new List<EventInvocation>();
            this.eventInvoker = eventInvoker;
        }

        /// <summary>
        /// Passes the gate. Depending whether it is guarded or not the
        /// event gets fired immediatelly, otherwise the guardian has full
        /// control when the invocations are going to pass the gate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="argument"></param>
        public void PassGate(SenderType sender, ArgumentType argument)
        {
            if (guarded) {
                eventInvocations.Add(new EventInvocation(sender, argument));
            } else {
                eventInvoker?.Invoke(sender, argument);
            }
        }

        void IReleasableEventInvocationGate.ReleaseGate()
        {
            foreach (var invocation in eventInvocations) {
                eventInvoker?.Invoke(invocation.Sender, invocation.Argument);
            }

            guarded = false;
        }

        public EventInvocationGateGuardian Guard()
        {
            if (guarded) {
                throw new InvalidOperationException("The event invocation gate can only have one guardian.");
            }

            guarded = true;
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
