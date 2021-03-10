using System;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations
{
    public class IdentifierAttribute : Attribute
    {
        public string Identifier { get; }

        public IdentifierAttribute(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier)) {
                throw new ArgumentException($"The parameter { nameof(identifier) } can not be null or white-space.", nameof(identifier));
            }

            Identifier = identifier;
        }
    }
}
