using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Teronis.Collections.Algorithms;

namespace Teronis.Collections.Synchronization.Utils
{
    public static class IEnumerableICollectionModificationUtils
    {
        /// <summary>
        /// This method traces moving indexes and determines replace modifications and yield returns this modification with its respective intial old index.
        /// </summary>
        /// <typeparam name="OldItemType"></typeparam>
        /// <typeparam name="NewItemType"></typeparam>
        /// <param name="modifications"></param>
        /// <returns></returns>
        internal static IEnumerable<(ICollectionModification<OldItemType, NewItemType> Modification, int InitialOldIndex)> YieldTuplesButOnlyReplaceModificationWithInitialOldIndex<OldItemType, NewItemType>(IEnumerable<ICollectionModification<OldItemType, NewItemType>> modifications) {
            yield return default;

            var tempModification = new Dictionary<int, (ICollectionModification<OldItemType, NewItemType> Modification, int InitialOldIndex)>();

            foreach (var modification in modifications) {
                if (modification.Action == NotifyCollectionChangedAction.Move) {
                    Debug.Assert(tempModification.Count == 0, "Two move modifications in a row were not expected.");
                    tempModification.Add(modification.NewIndex, (Modification: modification, InitialOldIndex: modification.OldIndex));
                } else if (modification.Action == NotifyCollectionChangedAction.Replace) {
                    if (tempModification.TryGetValue(modification.OldIndex, out var gotValue)) {
                        yield return (Modification: modification, gotValue.InitialOldIndex);
                        tempModification.Remove(modification.OldIndex);
                    } else { 
                        yield return (Modification: modification, modification.OldIndex);
                    }
                }
            }
        }
    }
}
