using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class JSFacadesActivatorOptionsExtensions
    {
        public static void AddDefaultComponentPropertyAssigners(this JSFacadesActivatorOptions options)
        {
            ComponentPropertyAssignersUtils.ForEachDefaultComponentPropertyAssigner(defaultComponentPropertyAssignerType =>
                options.ComponentPropertyAssignerFactories.Add(defaultComponentPropertyAssignerType, value: null));
        }
    }
}
