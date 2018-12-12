using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Teronis.NetStandard.Tools;

namespace Teronis.NetStandard.Extensions
{
   public static class PropertyInfoExtensions
    {
        public static bool TryToVariableInfo(this PropertyInfo property, out VariableInfo varInfo)
            => TypeTools.TryToVariableInfo(property, () => property.Name, () => property.PropertyType, out varInfo);
    }
}
