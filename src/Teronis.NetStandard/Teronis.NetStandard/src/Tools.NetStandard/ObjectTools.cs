using System;
using System.Reflection;
using Teronis.Text;

namespace Teronis.Tools.NetStandard
{
    public static class ObjectTools
    {
        /// <summary>
        /// Gets the value of a Property for a given object instance.
        /// </summary>
        /// <typeparam name="ValueType">The <see cref="Type"/> you want the value to be converted to when returned.</typeparam>
        /// <param name="instance">The Type instance to extract the Property's data from.</param>
        /// <param name="propertyName">The name of the Property to extract the data from.</param>
        /// <returns></returns>
        internal static ValueType GetPropertyValue<ValueType>(object instance, string propertyName, BindingFlags flags = 0)
        {
            var pi = instance.GetType().GetProperty(propertyName, flags);
            return (ValueType)pi.GetValue(instance, null);
        }

        /// <summary>
        /// Gets the value of a Property for a given object instance.
        /// </summary>
        /// <typeparam name="ValueType">The <see cref="Type"/> you want the value to be converted to when returned.</typeparam>
        /// <param name="instance">The Type instance to extract the Property's data from.</param>
        /// <param name="fieldName">The name of the Property to extract the data from.</param>
        /// <returns></returns>
        internal static ValueType GetFieldValue<ValueType>(object instance, string fieldName, BindingFlags flags = 0)
        {
            var pi = instance.GetType().GetField(fieldName, 0);
            return (ValueType)pi.GetValue(instance);
        }

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
