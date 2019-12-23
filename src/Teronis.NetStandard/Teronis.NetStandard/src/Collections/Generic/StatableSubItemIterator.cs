using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    /* TODO: reimplement */
    public class TStatableSubItemIterator<SUB_ITEM_TYPE, ENUM_TYPE> : SubItemIterator<SUB_ITEM_TYPE> where SUB_ITEM_TYPE : class, IChild<SUB_ITEM_TYPE> where ENUM_TYPE : struct, IComparable, IFormattable, IConvertible
    {
        protected Func<SUB_ITEM_TYPE, ENUM_TYPE> getState;

        public TStatableSubItemIterator(SUB_ITEM_TYPE childRef, Func<SUB_ITEM_TYPE, ENUM_TYPE> getState) : base(childRef) => this.getState = getState;

        public SUB_ITEM_TYPE TryGetLastBase(ENUM_TYPE[] states = null, ENUM_TYPE[] skippingStates = null, int afterWhileSkips = 0, int beforeWhileSkips = 0)
        {
            TryGetLastBase(out var lastBase, states, skippingStates, afterWhileSkips, beforeWhileSkips);
            return lastBase;
        }

        /// <returns>If true, the search of the last child has interrupted within the search and the end of children (null) has not been reached.</returns>
        public bool TryGetLastBase(out SUB_ITEM_TYPE lastBase, ENUM_TYPE[] states = null, ENUM_TYPE[] skippingStates = null, int afterWhileSkips = 0, int beforeWhileSkips = 0, Func<int, int, IEnumerable<SUB_ITEM_TYPE>> iterateBases = null)
        {
            bool innerInterruption = false;

            foreach (var child in iterateBases?.Invoke(afterWhileSkips, beforeWhileSkips) ?? IterateBases(afterWhileSkips, beforeWhileSkips)) {
                if ((skippingStates ?? new ENUM_TYPE[] { }).Any(x => x.Equals(getState(child))))
                    continue;
                else if (!(states ?? EnumExtensions.CombineEachEnumValue<ENUM_TYPE>().CrushContainingBitsToEnumerable()).Any(x => x.Equals(getState(child)))) {
                    innerInterruption = true;
                    break; // important
                } else {
                    innerInterruption = true;
                    lastBase = child;
                    goto exit;
                }
            }

            lastBase = null;
            exit:
            return innerInterruption;
        }
    }
}
