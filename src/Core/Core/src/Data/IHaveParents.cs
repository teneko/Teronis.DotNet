

namespace Teronis.Data
{
    public interface IHaveParents
    {
        event WantParentsEventHandler WantParents;

        ParentsPicker GetParentsPicker();
    }
}
