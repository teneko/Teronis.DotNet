using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IHaveParents
    {
        event WantParentsEventHandler WantParents;

        ParentsPicker GetParentsPicker();
    }
}
