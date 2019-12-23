using System;

namespace Teronis.Text.UriBuilder
{
    public class LinkKeyValueAttribute : Attribute
    {
        public object DefaultValue {
            get { return @default; }
            set {
                @default = value;
                IsDefaultSet = true;
            }
        }
        public bool IsDefaultSet { get; private set; }

        public string Name;

        object @default;

        public bool ProofValueEquality(object val)
        {
            if (@default == null && val == null)
                return true;
            else if (@default == null || val == null)
                return false;
            else
                return DefaultValue.Equals(val);
        }

        public LinkKeyValueAttribute() { }

        public LinkKeyValueAttribute(bool nullable)
        {
            IsDefaultSet = nullable;
        }
    }
}
