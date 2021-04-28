// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teronis.Reflection;

namespace Teronis.AspNetCore.Mvc.ApplicationModels.Filters
{
    public class ControllerTypeFilter : IControllerModelFilter, ITypeInfoFilter
    {
        public TypeInfoFilter TypeInfoFilter { get; }

        public ControllerTypeFilter(TypeInfoFilter controllerTypeFilter) =>
            TypeInfoFilter = controllerTypeFilter;

        public ControllerTypeFilter(TypeInfo[]? typeInfoAllowlist, params TypeInfo[]? typeInfoBlocklist) =>
            TypeInfoFilter = new TypeInfoFilter(typeInfoAllowlist, typeInfoBlocklist);

        public ControllerTypeFilter(params TypeInfo[] typeInfoAllowList)
            : this(typeInfoAllowList, default) { }

        public bool IsAllowed(TypeInfo? type) =>
            TypeInfoFilter.IsAllowed(type);

        public bool IsAllowed(ControllerModel? controller) =>
            IsAllowed(controller?.ControllerType);
    }
}
