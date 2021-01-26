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
