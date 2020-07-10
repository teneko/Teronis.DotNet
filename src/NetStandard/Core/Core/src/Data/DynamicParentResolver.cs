using System.Dynamic;
using System.Linq;

namespace Teronis.Data
{
    public class DynamicParentResolver : DynamicObject
    {
        private readonly IHaveParents havingParents;

        public DynamicParentResolver(IHaveParents havingParents)
            => this.havingParents = havingParents;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var parents = havingParents.GetParentsPicker().GetParents(null);
            result = parents.Single(x => x.GetType().Name == binder.Name);
            return true;
        }
    }
}
