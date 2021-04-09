// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.ObjectModel
{
    internal interface IReleasableEventInvocationGate
    {
        /// <summary>
        /// Releases the gate and all withheld invocation are invoked at once.
        /// </summary>
        void ReleaseGate();
    }
}
