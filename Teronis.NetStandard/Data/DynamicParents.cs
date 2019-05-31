using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;

namespace Teronis.Data
{
    public class DynamicParents : DynamicObject
    {
        private IHaveParents havingParents;

        public DynamicParents(IHaveParents havingParents)
            => this.havingParents = havingParents;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var parents = havingParents.GetParents(null);
            result = parents.Single(x => x.GetType().Name == binder.Name);
            return true;
        }
    }
}
