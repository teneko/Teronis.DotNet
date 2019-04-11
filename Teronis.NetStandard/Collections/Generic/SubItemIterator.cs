using System;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    /* TODO: reimplement */
    public class SubItemIterator<SUB_ITEM_TYPE> where SUB_ITEM_TYPE : class, IChild<SUB_ITEM_TYPE>
    {
        public SUB_ITEM_TYPE SubItem { get; private set; }

        public SubItemIterator(SUB_ITEM_TYPE childRef) => SubItem = childRef;

        /// <param name="afterWhileSkips">Skip the amount if <paramref name="afterWhileSkips"/>.
        /// If <paramref name="beforeWhileSkips"/> is zero, and the output of <paramref name="while"/> or current base instance is equals to null, <paramref name="afterWhileSkips"/>
        /// is reduced by one as long as it reached zero.</param>
        /// <param name="beforeWhileSkips">Skip by the amount of <paramref name="beforeWhileSkips"/>. It's used before <paramref name="afterWhileSkips"/> enters into force.
        /// After <paramref name="beforeWhileSkips"/> reached zero, the while function you can specify at <paramref name="while"/> is called.</param>
        /// <param name="throwError"></param>
        /// <param name="while"></param>
        public IEnumerable<SUB_ITEM_TYPE> IterateBases(int afterWhileSkips = 0, int beforeWhileSkips = 0, bool throwError = false, Func<SUB_ITEM_TYPE, bool> @while = null)
        {
            var current = SubItem;

            while (beforeWhileSkips > 0 || (@while?.Invoke(current) ?? current != null)) {
                if (beforeWhileSkips > 0 || afterWhileSkips > 0) {
                    if (beforeWhileSkips > 0)
                        beforeWhileSkips--;
                    else
                        afterWhileSkips--;

                    next();
                    continue;
                }

                var retVal = current;
                next();
                yield return retVal;

                void next() => current = current.TryGetSubLayer();
            }

            if (throwError)
                throw new Exception("no result.");
        }
    }
}
