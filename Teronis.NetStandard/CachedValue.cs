using System;

namespace Teronis
{
    public class CachedValue<T> : WrappedValue<T>
    {
        public bool IsValueCached { get; private set; }

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

        public CachedValue(Func<T> getValue) : base(default) => this.getValue = getValue;
    }
}
