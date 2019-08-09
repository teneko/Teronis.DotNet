using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IHaveKnownParents : IHaveParents
    {
        void AttachKnownWantParentsHandler(object caller, WantParentsEventHandler handler);
        void AttachWantParentsHandler(WantParentsEventHandler handler);
        void DetachKnownWantParentsHandler(object caller);
        void DetachWantParentsHandler(WantParentsEventHandler handler);
    }
}
