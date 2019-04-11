using System.Collections.Generic;

namespace Teronis.Extensions.NetStandard.tmp
{
    public static class IEnumerableIExternalObjectInitializationExtensions
    {
        public static void LetAttributesReceiveAttributeVariableInfo(this IEnumerable<IExternalObjectInitialization> ttbInitializedObjects)
        {
            var enumerator = ttbInitializedObjects.GetEnumerator();

            if (!enumerator.MoveNext())
                return;

            do {
                enumerator.Current.TriggerExternalObjectInitialization();
            } while (enumerator.MoveNext());
        }
    }
}
