// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.Reflection
{
    public static class NestedPropertyUtils
    {
        public static NestedProperty GetNestedProperty(object deepObject, string dotSeparatedPathParts, int startAtPathPart = 0)
        {
            deepObject = deepObject ?? throw new ArgumentNullException(nameof(deepObject));

            if (startAtPathPart < 0) {
                throw new IndexOutOfRangeException("The start index cannot be negative.");
            }

            Type objectType = deepObject.GetType();

            PropertyInfo? getNestedProperty(string pathPart) =>
                objectType.GetProperty(pathPart);

            PropertyInfo? nestedPropertyInfo = null;
            object? propertyHolderObject = null;
            object? pathPartObject = deepObject;

            foreach (var partedPath in dotSeparatedPathParts.Split('.')) {
                if (startAtPathPart > 0) {
                    startAtPathPart--;
                } else {
                    nestedPropertyInfo = getNestedProperty(partedPath);
                }

                if (nestedPropertyInfo == null) {
                    propertyHolderObject = null;
                    break;
                }

                objectType = nestedPropertyInfo.PropertyType;
                propertyHolderObject = pathPartObject;
                pathPartObject = nestedPropertyInfo.GetValue(propertyHolderObject);
            }

            return new NestedProperty() {
                PropertyHolderObject = propertyHolderObject,
                PropertyInfo = nestedPropertyInfo,
            };
        }

        public static object? GetNestedValue(NestedProperty nestedProperty)
        {
            if (nestedProperty.PropertyHolderObject != null && nestedProperty.PropertyInfo != null) {
                return nestedProperty.PropertyInfo.GetValue(nestedProperty.PropertyHolderObject);
            }

            return null;
        }

        public static object? GetNestedValue(object obj, string path, int startIndex = 0)
            => GetNestedValue(GetNestedProperty(obj, path, startIndex));
    }
}
