using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Reflection
{
    /// <summary>
    /// Sets the property value for a specified object.
    /// </summary>
    /// <param name="obj">The object whose property value will be set.</param>
    /// <param name="value">The new property value.</param>
    public delegate void SetValueHandler(object obj, object value);
}
