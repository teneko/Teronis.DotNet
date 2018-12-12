using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Teronis.NetStandard
{
    public sealed class VariableInfoSettings
    {
        public static readonly VariableInfoSettings Default = new VariableInfoSettings();

        public VariableInfoSettings() {
            Flags = BindingFlags.Instance | BindingFlags.Public;
            ExcludeIfReadable = false;
            ExcludeIfWritable = false;
            RequireReadability = false;
            RequireWritablity = false;
        }

        public BindingFlags Flags;
        public bool ExcludeIfReadable;
        public bool ExcludeIfWritable;
        public bool RequireReadability;
        public bool RequireWritablity;
    }
}
