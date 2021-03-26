// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.ObjectModel
{
    public readonly struct EventInvocationGateGuardian
    {
        private readonly IPassableEventInvocationGate buffer;

        internal EventInvocationGateGuardian(IPassableEventInvocationGate buffer)
        {
            this.buffer = buffer;
        }

        public void LetPassThrough() =>
            buffer.LetPassThrough();
    }
}
