using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Teronis.Extensions;

namespace Teronis.Mvc
{
    /// <summary>
    /// Enables you to inject custom controllers to application that are not regarded.
    /// </summary>
    public class TypesProvidingApplicationPart : ApplicationPart, IApplicationPartTypeProvider
    {
        public static TypesProvidingApplicationPart Create(params TypeInfo[] types) =>
            new TypesProvidingApplicationPart(types);

        public static TypesProvidingApplicationPart Create(string? name, params TypeInfo[] types) =>
            new TypesProvidingApplicationPart(types, name);

        public IEnumerable<TypeInfo> Types { get; }

        public override string? Name => name ?? nameof(TypesProvidingApplicationPart);

        private string? name;

        public TypesProvidingApplicationPart(IEnumerable<TypeInfo> types, string? name)
        {
            Types = types ?? throw new ArgumentNullException(nameof(types));
            this.name = name;
        }

        public TypesProvidingApplicationPart(IEnumerable<TypeInfo> types)
            : this(types, null) { }
    }
}
