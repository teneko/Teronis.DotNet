// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Teronis.Microsoft.JSInterop.Collections;
using static Teronis.Microsoft.JSInterop.Component.Assigners.ValueAssignerContext;

namespace Teronis.Microsoft.JSInterop.Component.Assigners
{
    public class ValueAssignerContext : TreeIterator<IValueAssigner, ValueAssignerEntry, ValueAssignerEntryEnumerator>
    {
        public virtual Valuable<object> ValueResult { get; protected set; }
        public virtual bool ValueOriginatesFromInterceptor { get; protected set; }

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

        /// <summary>
        /// Sets the new value result and sets <see cref="ValueOriginatesFromInterceptor"/> to false.
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetValueResult(object? value)
        {
            ValueOriginatesFromInterceptor = false;
            ValueResult = value;
        }

        public virtual void ClearValueResult() =>
            ValueResult = default;

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
    }
}
