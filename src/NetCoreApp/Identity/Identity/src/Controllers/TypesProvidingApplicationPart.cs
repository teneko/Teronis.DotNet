using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Teronis.Extensions;

namespace Teronis.Identity.Controllers
{
    /// <summary>
    /// Enables you to inject custom controllers to application that won't be regarded.
    /// </summary>
    public class TypesProvidingApplicationPart : ApplicationPart, IApplicationPartTypeProvider
    {
        public static TypesProvidingApplicationPart Create(params TypeInfo[] typeInfos) =>
            new TypesProvidingApplicationPart(typeInfos);

        public IEnumerable<TypeInfo> Types { get; }
        public override string Name => nameof(TypesProvidingApplicationPart).TrimEnd(nameof(ApplicationPart));

        public TypesProvidingApplicationPart(IEnumerable<TypeInfo> typeInfos)
        {
            typeInfos = typeInfos ?? throw new ArgumentNullException(nameof(typeInfos));
            Types = typeInfos;
        }
    }
}
