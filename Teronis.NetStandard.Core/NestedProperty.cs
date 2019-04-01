using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Teronis.NetStandard
{
    public class NestedProperty
    {
        public PropertyInfo PropertyInfo { get; set; }
        public object OriginObject { get; set; }
    }
}
