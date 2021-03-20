using System;

namespace Teronis.Microsoft.JSInterop.Dynamic.Annotations
{
    /// <summary>
    /// Can be used to annotate a method to specify an alternative
    /// JavaScript identifier. This will overwrite the default behaviour
    /// that takes the name from method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
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
