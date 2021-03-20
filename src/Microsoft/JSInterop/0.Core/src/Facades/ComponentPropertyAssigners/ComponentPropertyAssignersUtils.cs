using System;

namespace Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners
{
    internal static class ComponentPropertyAssignersUtils
    {
        public static void ForEachDefaultComponentPropertyAssigner(Action<Type> defaultComponentPropertyAssignmentCallback)
        {
            if (defaultComponentPropertyAssignmentCallback is null) {
                throw new ArgumentNullException(nameof(defaultComponentPropertyAssignmentCallback));
            }

            defaultComponentPropertyAssignmentCallback(typeof(JSModuleComponentPropertyAssigner));
        }
    }
}
