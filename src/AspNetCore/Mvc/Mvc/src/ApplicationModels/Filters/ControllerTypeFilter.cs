using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teronis.Reflection;

namespace Teronis.Mvc.ApplicationModels.Filters
{
    public class ControllerTypeFilter : IControllerModelFilter, ITypeInfoFilter
    {
        public TypeInfoFilter TypeInfoFilter { get; }

        public ControllerTypeFilter(TypeInfoFilter controllerTypeFilter) =>
            TypeInfoFilter = controllerTypeFilter;

        public ControllerTypeFilter(TypeInfo[]? typeInfoAllowList, params TypeInfo[]? typeInfoBlockList) =>
            TypeInfoFilter = new TypeInfoFilter(typeInfoAllowList, typeInfoBlockList);

        public ControllerTypeFilter(params TypeInfo[] typeInfoAllowList)
            : this(typeInfoAllowList, default) { }

        public bool IsAllowed(TypeInfo? type) =>
            TypeInfoFilter.IsAllowed(type);

        public bool IsAllowed(ControllerModel? controller) =>
            IsAllowed(controller?.ControllerType);
    }
}
