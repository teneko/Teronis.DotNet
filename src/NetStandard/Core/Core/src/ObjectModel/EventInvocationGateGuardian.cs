// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.ComponentModel
{
    public readonly struct EventInvocationGateGuardian : IReleasableEventInvocationGate
    {
        private readonly IReleasableEventInvocationGate gate;

        internal EventInvocationGateGuardian(IReleasableEventInvocationGate gate) =>
            this.gate = gate;

        /// <summary>
        /// Releases the gate and all withheld invocation are invoked at once.
        /// </summary>
        public void ReleaseGate() =>
            gate.ReleaseGate();
    }
}
