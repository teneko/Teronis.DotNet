// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Annotations
{
    public static class JSModuleAttributeUtils
    {
        public static string GetModuleNameOrPath<TClassAttribute>(AssignModuleAttribute propertyAttribute, ICustomAttributes propertyTypeAttributes)
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
        /// <param name="componentMember"></param>
        /// <param name="moduleNameOrPath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when JavaScrit module facade attribute is 
        /// given but module name or path could not been found.
        /// </exception>
        public static bool TryGetModuleNameOrPath<TPropertyAttribute, TClassAttribute>(IDefinition componentMember, [MaybeNullWhen(false)] out string moduleNameOrPath)
            where TPropertyAttribute : AssignModuleAttribute
            where TClassAttribute : JSModuleClassAttribute
        {
            if (!componentMember.TryGetAttribute<TPropertyAttribute>(out var propertyModuleAttribute)) {
                moduleNameOrPath = null;
                return false;
            }

            moduleNameOrPath = GetModuleNameOrPath<TClassAttribute>(propertyModuleAttribute, componentMember.DefinitionMemberType);
            return true;
        }
    }
}
