using System;
using System.Linq;
using Teronis.Extensions.NetStandard;

namespace Teronis.Data
{
    public class ParentsPicker
    {
        public object Sender { get; private set; }

        private WantParentsEventHandler eventHandler;

        public ParentsPicker(object sender, WantParentsEventHandler eventHandler)
        {
            Sender = sender;
            this.eventHandler = eventHandler;
        }

        public ParentsContainer.ParentCollection GetParents(Type wantedParentType)
            => new HavingParentsEventArgs(Sender, wantedParentType).GetParents(eventHandler);

        public object GetSingleParent(Type wantedParentType)
            => GetParents(wantedParentType).Single();

        public ParentType GetSingleParent<ParentType>()
            => (ParentType)GetSingleParent(typeof(ParentType));
    }
}
