using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class ComponentPropertyUtils
    {
        public static bool TryGetModulePathRelativeToWwwRoot(PropertyInfo propertyInfo, [MaybeNullWhen(false)] out string pathRelativeToWwwRoot)
        {
            if (!(Attribute.GetCustomAttribute(propertyInfo, propertyModuleAttributeType) is Annotiations.JSModuleFacadeAttribute propertyModuleAttribute)) {
                goto exit;
            }

            if (propertyModuleAttribute.ModuleNameOrPath == null) {
                if (!(Attribute.GetCustomAttribute(propertyInfo.PropertyType, classModuleAttributeType) is Annotiations.Design.JSModuleFacadeAttribute moduleWrapperModuleAttribute)
                    || moduleWrapperModuleAttribute.ModuleNameOrPath == null) {
                    throw new InvalidOperationException("Neither the module attribute from property nor a module attribute from class has a path to script.");
                }

                pathRelativeToWwwRoot = moduleWrapperModuleAttribute.ModuleNameOrPath;
            } else {
                pathRelativeToWwwRoot = propertyModuleAttribute.ModuleNameOrPath;
            }

            return true;

            exit:
            pathRelativeToWwwRoot = null;
            return false;
        }

        private static readonly Type propertyModuleAttributeType;
        private static readonly Type classModuleAttributeType;

        static ComponentPropertyUtils()
        {
            propertyModuleAttributeType = typeof(Annotiations.JSModuleFacadeAttribute);
            classModuleAttributeType = typeof(Annotiations.Design.JSModuleFacadeAttribute);
        }
    }
}
