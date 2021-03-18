using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facades.Annotiations;

namespace Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssignments
{
    public class JSModuleComponentAssignment : IComponentPropertyAssignment
    {
        public async ValueTask<YetNullable<IAsyncDisposable>> TryAssignComponentProperty(
            IComponentPropertyInfo componentProperty,
            IJSFacadeResolver jsFacadeResolver)
        {
            if (!componentProperty.TryGetAttribtue<JSModuleFacadePropertyAttribute>(out var propertyModuleAttribute)) {
                goto exit;
            }

            string? moduleNameOrPath;

            if (propertyModuleAttribute.ModuleNameOrPath == null) {
                if (!componentProperty.ComponentPropertyTypeInfo.TryGetAttribtue<JSModuleFacadeAttribute>(out var classModuleAttribute)
                    || classModuleAttribute.ModuleNameOrPath == null) {
                    throw new InvalidOperationException("Neither the module attribute from property nor a module attribute from class has a path to script.");
                }

                moduleNameOrPath = classModuleAttribute.ModuleNameOrPath;
            } else {
                moduleNameOrPath = propertyModuleAttribute.ModuleNameOrPath;
            }

            var jsFacade = await jsFacadeResolver.CreateModuleFacadeAsync(moduleNameOrPath, componentProperty.PropertyType);
            return new YetNullable<IAsyncDisposable>(jsFacade);

            exit:
            return default;
        }
    }
}
