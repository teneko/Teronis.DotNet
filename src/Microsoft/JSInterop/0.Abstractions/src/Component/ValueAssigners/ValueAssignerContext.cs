// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Microsoft.JSInterop.Collections;
using static Teronis.Microsoft.JSInterop.Component.ValueAssigners.ValueAssignerContext;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigners
{
    public class ValueAssignerContext : TreeIterator<IValueAssigner, ValueAssignerEntry, ValueAssignerEntryEnumerator>
    {
        public YetNullable<object> ValueResult { get; private set; }

        public ValueAssignerContext(ValueAssignerContext context)
            : base(context) { }

        public ValueAssignerContext(IEnumerable<IValueAssigner> propertyAssigners, int startIndex)
            : base(propertyAssigners, startIndex) { }

        public ValueAssignerContext(IValueAssigner propertyAssigner)
            : base(propertyAssigner) { }

        public ValueAssignerContext(IEnumerable<IValueAssigner> propertyAssigners)
            : base(propertyAssigners) { }

        protected override ValueAssignerEntry CreateEntry(IValueAssigner item) =>
            new ValueAssignerEntry(item);

        protected override ValueAssignerEntryEnumerator CreateEnumerator(IReadOnlyList<ValueAssignerEntry> entries, int initialIndex = -1) =>
            new ValueAssignerEntryEnumerator(entries, initialIndex);

        public class ValueAssignerEntryEnumerator : TreeIteratorEnumerator<ValueAssignerEntry>
        {
            internal ValueAssignerEntryEnumerator(IReadOnlyList<ValueAssignerEntry> entries, int initialIndex = -1)
                : base(entries, initialIndex) { }
        }

        public class ValueAssignerEntry : TreeIteratorEntry<IValueAssigner>
        {
            internal ValueAssignerEntry(IValueAssigner propertyAssigner)
                : base(propertyAssigner) { }
        }

        public void SetValueResult(object value) =>
            ValueResult = value;
    }
}
