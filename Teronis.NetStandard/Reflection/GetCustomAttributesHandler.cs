using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Reflection
{
    /// <summary>
    /// When overridden in a derived class, returns an array of all custom attributes applied to this member.
    /// </summary>
    /// <param name="inherit">true to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events.</param>
    /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
    public delegate object[] GetCustomAttributesHandler(bool inherit);
}
