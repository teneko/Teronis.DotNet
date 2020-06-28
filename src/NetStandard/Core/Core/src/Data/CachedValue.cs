using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Data
{
    public class CachedValue<T> : WrappedValue<T>
    {
        public bool IsValueCached { get; private set; }

        [MaybeNull]
        public override T Value {
            get {
                if (IsValueCached)
                    return base.Value;
                else {
                    var value = getValue();
                    base.Value = value;
                    IsValueCached = true;
                    return value;
                }
            }
        }

        private Func<T> getValue;

        public CachedValue(Func<T> getValue) 
            : base(default) => this.getValue = getValue;
    }
}
