using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Reflection
{
    /// <summary>
    /// Returns the property value of a specified object.
    /// </summary>
    /// <param name="obj">The object whose property value will be returned.</param>
    /// <returns>The property value of the specified object.</returns>
    public delegate object GetValueHandler(object obj);
}
