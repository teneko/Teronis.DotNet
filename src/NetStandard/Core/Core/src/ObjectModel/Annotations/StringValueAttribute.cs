using System;

namespace Teronis.ObjectModel.Annotations
{
    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        /// <summary>
        /// Holds the string value for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        public StringValueAttribute(string stringValue) =>
            StringValue = stringValue;
    }
}
