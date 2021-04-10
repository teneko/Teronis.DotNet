// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Teronis.ComponentModel.Parenthood
{
    public class ParentsContainer
    {
        public ParentCollection Parents { get; private set; }
        public Type? WantedType { get; private set; }

        /// <summary>
        /// If <paramref name="wantedType"/> is null, then all parents may attach themselves.
        /// </summary>
        /// <param name="wantedType"></param>
        public ParentsContainer(Type? wantedType)
        {
            Parents = new ParentCollection();
            WantedType = wantedType;
        }

        /// <summary>
        /// The <paramref name="parent"/> will be added to the <see cref="Parents"/>, 
        /// if the type of <paramref name="parent"/> the same type of 
        /// <see cref="WantedType"/> or if none <see cref="WantedType"/> is provided.
        /// </summary>
        public void AddParent(object parent)
        {
            parent = parent ?? throw new ArgumentNullException(nameof(parent));
            bool canAddParent = false;

            if (WantedType == null) {
                canAddParent = true;
            } else {
                var parentType = parent.GetType();

                if (WantedType == parentType || WantedType.IsInterface && WantedType.IsAssignableFrom(parentType)) {
                    canAddParent = true;
                }
            }

            if (canAddParent) {
                Parents.Add(parent);
            }
        }

        public void AddParents(IEnumerable<object> parents)
        {
            foreach (var parent in parents) {
                AddParent(parent);
            }
        }

        public class ParentCollection : Collection<object> { }
    }
}
