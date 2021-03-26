// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Microsoft.JSInterop.Facades.Annotations
{
    public static class JSModuleAttributeUtils
    {
        /// <summary>
        /// Tries to get the module name or path. If property
        /// attribute does exist the module name or path has
        /// to be provided either from property attribute or
        /// module attribute.
        /// </summary>
        /// <param name="componentProperty"></param>
        /// <param name="moduleNameOrPath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when JavaScrit module facade attribute is 
        /// given but module name or path could not been found.
        /// </exception>
        public static bool TryGetModuleNameOrPath<TPropertyAttribute, TClassAttribute>(IComponentProperty componentProperty, [MaybeNullWhen(false)] out string moduleNameOrPath)
            where TPropertyAttribute : JSModulePropertyAttribute
            where TClassAttribute : JSModuleClassAttribute
        {
            if (!componentProperty.TryGetAttribtue<TPropertyAttribute>(out var propertyModuleAttribute)) {
                moduleNameOrPath = null;
                return false;
            }

            if (propertyModuleAttribute.ModuleNameOrPath == null) {
                if (!componentProperty.ComponentPropertyType.TryGetAttribtue<TClassAttribute>(out var classModuleAttribute)
                    || classModuleAttribute.ModuleNameOrPath == null) {
                    throw new InvalidOperationException("Neither the module attribute from property nor a module attribute from class has module name or a path to script.");
                }

                moduleNameOrPath = classModuleAttribute.ModuleNameOrPath;
            } else {
                moduleNameOrPath = propertyModuleAttribute.ModuleNameOrPath;
            }

            return true;
        }
    }
}
