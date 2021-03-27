// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Components.NUnit
{
    /// <summary>
    /// Be careful and read
    /// https://gist.github.com/SteveSandersonMS/ec232992c2446ab9a0059dd0fbc5d0c3
    /// before using this helper class.
    /// </summary>
    internal struct RenderTreeSequence
    {
        public int Sequence { get; set; }

        public RenderTreeSequence(int sequence) =>
            Sequence = sequence;

        public int Reset(int sequence = 0) =>
            Sequence = sequence;

        public int Increment(int operand = 1) =>
            Sequence += operand;

        public RenderTreeSequence PlanIncrement(int operand = 1)
        {
            var temporarySequence = Sequence;
            Sequence += operand;
            return new RenderTreeSequence(temporarySequence);
        }
    }
}
