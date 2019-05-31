using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IHaveKnownParents : IHaveParents
    {
        void AttachWantParentsHandler(object caller, WantParentsEventHandler handler);
        void AttachWantParentsHandler(WantParentsEventHandler handler);
        void DetachWantParentsHandler(object caller);
        void DetachWantParentsHandler(WantParentsEventHandler handler);
    }
}
