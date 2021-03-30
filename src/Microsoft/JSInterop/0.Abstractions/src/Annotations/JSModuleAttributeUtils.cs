// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    public static class JSModuleAttributeUtils
    {
        public static string GetModuleNameOrPath<TClassAttribute>(JSModulePropertyAttribute propertyAttribute, ICustomAttributes propertyTypeAttributes)
            where TClassAttribute : JSModuleClassAttribute
        {
            if (propertyAttribute.ModuleNameOrPath == null) {
                if (!propertyTypeAttributes.TryGetAttribute<TClassAttribute>(out var classModuleAttribute)
                    || classModuleAttribute.ModuleNameOrPath == null) {
                    throw new InvalidOperationException("Neither the module attribute from property nor a module attribute from class has module name or a path to script.");
                }

                return classModuleAttribute.ModuleNameOrPath;
            }

            return propertyAttribute.ModuleNameOrPath;
        }

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
            if (!componentProperty.TryGetAttribute<TPropertyAttribute>(out var propertyModuleAttribute)) {
                moduleNameOrPath = null;
                return false;
            }

            moduleNameOrPath = GetModuleNameOrPath<TClassAttribute>(propertyModuleAttribute, componentProperty.PropertyType);
            return true;
        }
    }
}
