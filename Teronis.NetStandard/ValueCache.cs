using System;

namespace Teronis
{
    public class ValueCache<T> : ValueWrap<T>
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

        public ValueCache(Func<T> getValue) : base(default) => this.getValue = getValue;
    }
}
