using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Reflection
{
    /// <summary>
    /// When overridden in a derived class, indicates whether one or more attributes of the specified type or of its derived types is applied to this member.
    /// </summary>
    /// <param name="attributeType">The type of custom attribute to search for. The search includes derived types.</param>
    /// <param name="inherit">true to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events.</param>
    /// <returns>true if one or more instances of attributeType or any of its derived types is applied to this member; otherwise, false.</returns>
    public delegate bool IsDefinedHandler(Type attributeType, bool inherit);
}
