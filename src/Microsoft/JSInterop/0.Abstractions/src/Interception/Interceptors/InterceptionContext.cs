// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Teronis.Microsoft.JSInterop.Collections;
using static Teronis.Microsoft.JSInterop.Interception.Interceptors.InterceptionContext;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public class InterceptionContext : TreeIterator<IJSInterceptor, InterceptionEntry, InterceptionEntryEnumerator>
    {
        public InterceptionContext(InterceptionContext context)
            : base(context) { }

        public InterceptionContext(IEnumerable<IJSInterceptor> interceptors, int startIndex)
            : base(interceptors, startIndex) { }

        public InterceptionContext(IJSInterceptor interceptor)
            : base(interceptor) { }

        public InterceptionContext(IEnumerable<IJSInterceptor> interceptors)
            : base(interceptors) { }

        protected override InterceptionEntry CreateEntry(IJSInterceptor item) =>
            new InterceptionEntry(item);

        protected override InterceptionEntryEnumerator CreateEnumerator(IReadOnlyList<InterceptionEntry> entries, int initialIndex = -1) =>
            new InterceptionEntryEnumerator(entries, initialIndex);

        public class InterceptionEntryEnumerator : TreeIteratorEnumerator<InterceptionEntry>
        {
            internal InterceptionEntryEnumerator(IReadOnlyList<InterceptionEntry> entries, int initialIndex = -1)
                : base(entries, initialIndex) { }
        }

        public class InterceptionEntry : TreeIteratorEntry<IJSInterceptor>
        {
            internal InterceptionEntry(IJSInterceptor interceptor)
                : base(interceptor) { }
        }
    }
}
