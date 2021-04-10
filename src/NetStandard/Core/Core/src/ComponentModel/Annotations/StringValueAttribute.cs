// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.ComponentModel.Annotations
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
