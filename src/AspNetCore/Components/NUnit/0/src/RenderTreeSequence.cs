namespace Teronis.AspNetCore.Components.NUnit
{
    /// <summary>
    /// Be careful and read
    /// https://gist.github.com/SteveSandersonMS/ec232992c2446ab9a0059dd0fbc5d0c3
    /// before using this helper class.
    /// </summary>
    internal class RenderTreeSequence
    {
        public int Sequence { get; set; }

        public void Reset(int sequence = 0) =>
            Sequence = sequence;

        public static implicit operator int(RenderTreeSequence sequence) =>
            sequence.Sequence++;
    }
}
