using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public class IndependentJSFacadePropertiesInitializer : IIndependentJSFacadePropertiesInitializer
    {
        private const BindingFlags PROPERTY_BINDING_FLAGS = BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic;

        private static bool GetPathRelativeToWwwRoot(PropertyInfo propertyInfo, [MaybeNullWhen(false)] out string pathRelativeToWwwRoot)
        {
            if (!(Attribute.GetCustomAttribute(propertyInfo, propertyModuleAttributeType) is Annotiations.JSModuleFacadeAttribute propertyModuleAttribute)) {
                goto exit;
            }

            if (propertyModuleAttribute.PathRelativeToWwwRoot == null) {
                if (!(Attribute.GetCustomAttribute(propertyInfo, classModuleAttributeType) is Annotiations.Design.JSModuleFacadeAttribute moduleWrapperModuleAttribute)
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

        static IndependentJSFacadePropertiesInitializer()
        {
            propertyModuleAttributeType = typeof(Annotiations.JSModuleFacadeAttribute);
            classModuleAttributeType = typeof(Annotiations.Design.JSModuleFacadeAttribute);
        }

        public async Task<JSFacadePropertiesInitialization> InitializeFacadePropertiesAsync(object component, IJSFacadeResolver jsFacadeResolver)
        {
            var propertyInfos = component.GetType().GetProperties(PROPERTY_BINDING_FLAGS);
            var moduleWrappers = new List<IAsyncDisposable>();

            foreach (var propertyInfo in propertyInfos) {
                if (!propertyInfo.CanWrite) {
                    continue;
                }

                if (!GetPathRelativeToWwwRoot(propertyInfo, out var pathRelativeToWwwRoot)) {
                    continue;
                }

                var jsModule = await jsFacadeResolver.ResolveModuleWrapper(pathRelativeToWwwRoot, propertyInfo.PropertyType);
                propertyInfo.SetValue(component, jsModule);
            }

            return new JSFacadePropertiesInitialization(moduleWrappers);
        }
    }
}
