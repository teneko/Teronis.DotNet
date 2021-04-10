// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using System.Linq;

namespace Teronis.ComponentModel.Parenthood
{
    /// <summary>
    /// Class that implements <see cref="DynamicObject.TryGetMember(GetMemberBinder, out object)"/>
    /// to get single parent by type name.
    /// </summary>
    public class SingleParentByTypeDynamicObject : DynamicObject
    {
        private readonly IHaveParents havingParents;

        public SingleParentByTypeDynamicObject(IHaveParents havingParents)
            => this.havingParents = havingParents;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var parents = havingParents.CreateParentsCollector().CollectParents(null);
            result = parents.Single(x => x.GetType().Name == binder.Name);
            return true;
        }
    }
}
