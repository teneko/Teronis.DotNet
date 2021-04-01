// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Microsoft.JSInterop.Collections;
using static Teronis.Microsoft.JSInterop.Component.PropertyAssignerContext;

namespace Teronis.Microsoft.JSInterop.Component
{
    public class PropertyAssignerContext : TreeIterator<IPropertyAssigner, PropertyAssignerEntry, PropertyAssignerEntryEnumerator>
    {
        public YetNullable<IAsyncDisposable> MemberResult { get; set; }

        public PropertyAssignerContext(PropertyAssignerContext context)
            : base(context) { }

        public PropertyAssignerContext(IEnumerable<IPropertyAssigner> propertyAssigners, int startIndex)
            : base(propertyAssigners, startIndex) { }

        public PropertyAssignerContext(IPropertyAssigner propertyAssigner)
            : base(propertyAssigner) { }

        public PropertyAssignerContext(IEnumerable<IPropertyAssigner> propertyAssigners)
            : base(propertyAssigners) { }

        protected override PropertyAssignerEntry CreateEntry(IPropertyAssigner item) =>
            new PropertyAssignerEntry(item);

        protected override PropertyAssignerEntryEnumerator CreateEnumerator(IReadOnlyList<PropertyAssignerEntry> entries, int initialIndex = -1) =>
            new PropertyAssignerEntryEnumerator(entries, initialIndex);

        public class PropertyAssignerEntryEnumerator : TreeIteratorEnumerator<PropertyAssignerEntry>
        {
            internal PropertyAssignerEntryEnumerator(IReadOnlyList<PropertyAssignerEntry> entries, int initialIndex = -1)
                : base(entries, initialIndex) { }
        }

        public class PropertyAssignerEntry : TreeIteratorEntry<IPropertyAssigner>
        {
            internal PropertyAssignerEntry(IPropertyAssigner propertyAssigner)
                : base(propertyAssigner) { }
        }
    }
}
