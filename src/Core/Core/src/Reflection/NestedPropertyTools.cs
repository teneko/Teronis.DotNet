using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Teronis.Reflection
{
    public static class NestedPropertyTools
    {


        public static NestedProperty GetNestedProperty(object obj, string path, int startIndex = 0)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (startIndex < 0)
                throw new IndexOutOfRangeException("The start index cannot be negative.");

            Type objectType = obj.GetType();

            PropertyInfo getNestedProperty(string pathPart) => objectType.GetProperty(pathPart);

            PropertyInfo nestedPropertyInfo = null;
            object originObject = null;
            object currentObject = obj;

            foreach (var partedPath in path.Split('.')) {
                if (startIndex > 0)
                    startIndex--;
                else {
                    nestedPropertyInfo = getNestedProperty(partedPath);
                }

                if (nestedPropertyInfo == null) {
                    originObject = null;
                    break;
                } else {
                    objectType = nestedPropertyInfo.PropertyType;
                    originObject = currentObject;
                    currentObject = nestedPropertyInfo.GetValue(originObject);
                }
            }

            return new NestedProperty() {
                OriginObject = originObject,
                PropertyInfo = nestedPropertyInfo,
            };
        }

        public static object GetNestedValue(NestedProperty nestedProperty)
        {
            if (nestedProperty.OriginObject != null && nestedProperty.PropertyInfo != null)
                return nestedProperty.PropertyInfo.GetValue(nestedProperty.OriginObject);
            else
                return null;
        }

        public static object GetNestedValue(object obj, string path, int startIndex = 0)
            => GetNestedValue(GetNestedProperty(obj, path, startIndex));
    }
}
