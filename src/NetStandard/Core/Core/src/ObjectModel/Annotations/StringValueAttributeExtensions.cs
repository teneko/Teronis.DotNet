using System;
using System.Reflection;

namespace Teronis.ObjectModel.Annotations
{
    public static class StringValueAttributeExtensions
    {
        /// <summary>
        /// Will get the string value for a given enums value, this 
        /// will only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        public static string GetStringValue(this Enum value, int index = 0)
        {
            // Get the type.
            Type type = value.GetType() ?? throw new ArgumentException("Enum value type is null.");
            // Get fieldinfo for this type.
            FieldInfo fieldInfo = type.GetField(value.ToString()) ?? throw new ArgumentException("Enum value does not exist.");

            // Get the stringvalue attributes.
            if (!(fieldInfo?.GetCustomAttributes(typeof(StringValueAttribute), false) is StringValueAttribute[] attributes) || !(attributes.Length > index)) {
                throw new ArgumentException("Enum value has no string value attribute");
            }

            return attributes[index].StringValue;
        }
    }
}
