using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Teronis.Extensions;

namespace Teronis.Identity.Controllers
{
    public class TypesProviderApplicationPart : ApplicationPart, IApplicationPartTypeProvider
    {
        public IEnumerable<TypeInfo> Types { get; }
        public override string Name => nameof(TypesProviderApplicationPart).TrimEnd(nameof(ApplicationPart));

        public TypesProviderApplicationPart(IEnumerable<TypeInfo> typeInfos) {
            typeInfos = typeInfos ?? throw new ArgumentNullException(nameof(typeInfos));
            Types = typeInfos;
        }
    }
}
