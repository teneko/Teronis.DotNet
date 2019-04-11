using System;
using System.Collections.Generic;

namespace Teronis
{
    public class DelegatedComparer<T> : Comparer<T>
    {
        private readonly Comparison<T> compareHandler;

        public DelegatedComparer(Comparison<T> compareHandler) => this.compareHandler = compareHandler ?? throw new ArgumentNullException(nameof(compareHandler));

        public override int Compare(T arg1, T arg2) => compareHandler(arg1, arg2);
    }
}
