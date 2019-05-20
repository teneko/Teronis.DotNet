using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IHaveParents
    {
        event EventHandler<ParentsEventArgs> WantParent;

        ParentsContainer.ParentCollection GetParents(Type wantedParentType);
    }
}
