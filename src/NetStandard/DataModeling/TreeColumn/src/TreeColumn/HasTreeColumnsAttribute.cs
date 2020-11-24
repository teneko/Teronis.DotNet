using System;

namespace Teronis.DataModeling.TreeColumn
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HasTreeColumnsAttribute : Attribute { }
}
