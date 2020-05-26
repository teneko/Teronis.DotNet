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
        public static string GetStringValue(this Enum value)
        {
            // Get the type.
            Type type = value.GetType();
            // Get fieldinfo for this type.
            FieldInfo fieldInfo = type.GetField(value.ToString());
            // Get the stringvalue attributes.
            StringValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            // Return the first if there was a match.
            return attributes.Length > 0 ? attributes[0].StringValue : null;
        }
    }
}
