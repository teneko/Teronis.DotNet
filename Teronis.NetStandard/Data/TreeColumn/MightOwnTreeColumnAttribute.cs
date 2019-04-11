using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data.TreeColumn
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MightOwnTreeColumnsAttribute : Attribute { }
}
