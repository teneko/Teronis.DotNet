using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IMemberInfoAttributes
    {
        ILookup<Type, Attribute> Attributes { get; }

        bool IsAttributeDefined(Type attributeType);
        bool IsAttributeDefined<T>();

        bool TryGetAttribtue<T>([MaybeNullWhen(false)] out T attribute)
            where T : Attribute;
    }
}
