using System;
using System.Collections;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations
{
    /// <summary>
    /// Can be used to annotated the last parameter of type <see cref="IEnumerable"/> 
    /// (e.g. `params object[]`) to announce this parameter being spreaded when the
    /// JavaScript function is called. The final JavaScript argument list will then
    /// consist of previous arguments and the spreaded arguments of the
    /// accommodatable annotated argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class AccommodatableAttribute : Attribute
    { }
}
