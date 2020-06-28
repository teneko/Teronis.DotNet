using System;
using Teronis.Extensions;

namespace Teronis.Data
{
    public class ParentsPicker
    {
        public object Sender { get; private set; }

        private WantParentsEventHandler? eventHandler;

        public ParentsPicker(object sender, WantParentsEventHandler? eventHandler)
        {
            Sender = sender;
            this.eventHandler = eventHandler;
        }

        public ParentsContainer.ParentCollection GetParents(Type? wantedParentType)
            => new HavingParentsEventArgs(Sender, wantedParentType).GetParents(eventHandler);

        public object GetSingleParent(Type wantedParentType)
        {
            var parents = GetParents(wantedParentType);
            var parentsCount = parents.Count;

            if (parentsCount == 0)
                throw new Exception("No parents have been found");
            else if (parentsCount >= 2)
                throw new Exception($"There are more than 2 parents ({parentsCount}, but only one is expected.");

            return parents[0];
        }

        public ParentType GetSingleParent<ParentType>()
            => (ParentType)GetSingleParent(typeof(ParentType));
    }
}
