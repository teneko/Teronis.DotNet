using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Teronis.Libraries.NetStandard;

namespace Teronis.Reflection
{
    public sealed class VariableInfoSettings
    {
        public static readonly VariableInfoSettings Default = new VariableInfoSettings();

        public BindingFlags Flags;
        public bool ExcludeIfReadable;
        public bool ExcludeIfWritable;
        public bool IncludeIfReadable;
        public bool IncludeIfWritable;
        public IEnumerable<Type> ExcludeByAttributeTypes;
        public bool ExcludeByAttributeTypesInherit;
        public IEnumerable<Type> IncludeByAttributeTypes;
        public bool IncludeByAttributeTypesInherit;

        public VariableInfoSettings()
        {
            Flags = BindingFlags.Instance | BindingFlags.Public;
            ExcludeByAttributeTypesInherit = Library.DefaultCustomAttributesInherit;
            IncludeByAttributeTypesInherit = Library.DefaultCustomAttributesInherit;
        }
    }
}
