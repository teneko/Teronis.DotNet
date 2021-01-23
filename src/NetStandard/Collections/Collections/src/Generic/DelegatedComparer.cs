using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public class DelegatedComparer<T> : Comparer<T>
    {
        private readonly Comparison<T> compareHandler;

        public DelegatedComparer(Comparison<T> compareHandler) => this.compareHandler = compareHandler ?? throw new ArgumentNullException(nameof(compareHandler));

        public override int Compare([AllowNull] T arg1, [AllowNull] T arg2) => compareHandler(arg1!, arg2!);
    }
}
