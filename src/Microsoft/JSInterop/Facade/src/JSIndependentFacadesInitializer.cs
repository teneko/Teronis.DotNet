using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSIndependentFacadesInitializer : IJSIndependentFacadesInitializer
    {
        private static bool TryGetModulePathRelativeToWwwRoot(PropertyInfo propertyInfo, [MaybeNullWhen(false)] out string pathRelativeToWwwRoot)
        {
            if (!(Attribute.GetCustomAttribute(propertyInfo, propertyModuleAttributeType) is Annotiations.JSModuleFacadeAttribute propertyModuleAttribute)) {
                goto exit;
            }

            if (propertyModuleAttribute.PathRelativeToWwwRoot == null) {
                if (!(Attribute.GetCustomAttribute(propertyInfo.PropertyType, classModuleAttributeType) is Annotiations.Design.JSModuleFacadeAttribute moduleWrapperModuleAttribute)
                    || moduleWrapperModuleAttribute.PathRelativeToWwwRoot == null) {
                    throw new InvalidOperationException("Neither the module attribute from property nor a module attribute from class has a path to script.");
                }

                pathRelativeToWwwRoot = moduleWrapperModuleAttribute.PathRelativeToWwwRoot;
            } else {
                pathRelativeToWwwRoot = propertyModuleAttribute.PathRelativeToWwwRoot;
            }

            return true;

            exit:
            pathRelativeToWwwRoot = null;
            return false;
        }

        private static readonly Type propertyModuleAttributeType;
        private static readonly Type classModuleAttributeType;

        static JSIndependentFacadesInitializer()
        {
            propertyModuleAttributeType = typeof(Annotiations.JSModuleFacadeAttribute);
            classModuleAttributeType = typeof(Annotiations.Design.JSModuleFacadeAttribute);
        }

        public async Task<JSFacades> InitializeFacadesAsync(object component, IJSFacadeResolver jsFacadeResolver)
        {
            if (component is null) {
                throw new ArgumentNullException(nameof(component));
            }

            var jsFacades = new List<IAsyncDisposable>();

            foreach (var propertyInfo in JSFacadeUtils.GetComponentProperties(component.GetType())) {
                if (!TryGetModulePathRelativeToWwwRoot(propertyInfo, out var pathRelativeToWwwRoot)) {
                    continue;
                }

                var jsModule = await jsFacadeResolver.ResolveModuleAsync(pathRelativeToWwwRoot, propertyInfo.PropertyType);
                propertyInfo.SetValue(component, jsModule);
            }

            return new JSFacades(jsFacades, jsFacadeResolver);
        }

        public Task<JSFacades> InitializeFacadesAsync(IJSFacadeResolver jsFacadeResolver) =>
            Task.FromResult(new JSFacades(new List<IAsyncDisposable>(), jsFacadeResolver));
    }
}
