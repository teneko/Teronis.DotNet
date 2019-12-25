using System;
using System.Reflection;

namespace Teronis.Tools
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
    }
}
