using System;
using Teronis.Tools.NetStandard;

namespace Teronis.Reflection
{
    /// <summary>
    /// It is a default exclusion for <see cref="VariableInfoSettings.ExcludeByAttributeTypes"/> used for <see cref="EntityTools.GetDefaultVariableInfoSettings"/>.
    /// It's purpose is to exclude properties from <see cref="EntityTools.UpdateEntityVariables{T}(T, T)"/>
    /// </summary>
    public class IgnoreEntityVariableAttribute : Attribute
    { }
}
