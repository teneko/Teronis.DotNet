using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IHaveParents
    {
        event EventHandler<ParentsEventArgs> WantParents;

        ParentsContainer.ParentCollection GetParents(Type wantedParentType);
    }
}
