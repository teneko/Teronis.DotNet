using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teronis.Reflection;

namespace Teronis.Mvc.ApplicationModels
{
    public class ControllerActionRouteTemplateConvention : IActionModelConvention
    {
        private readonly IStringFormatter actionRouteTemplateFormatter;
        private readonly ITypeInfoFilter? controllerTypeFilter;

        /// <summary>
        /// Creates an action name convetion that manipulates action names.
        /// </summary>
        /// <param name="actionRouteTemplateFormatter">Transformer on which the action name will be applied.</param>
        /// <param name="controllerTypeFilter">Filter on which the controller type if action will be applied.</param>
        public ControllerActionRouteTemplateConvention(IStringFormatter actionRouteTemplateFormatter, ITypeInfoFilter? controllerTypeFilter)
        {
            this.actionRouteTemplateFormatter = actionRouteTemplateFormatter ?? throw new ArgumentNullException(nameof(actionRouteTemplateFormatter));
            this.controllerTypeFilter = controllerTypeFilter;
        }

        public ControllerActionRouteTemplateConvention(IStringFormatter actionRouteTemplateFormatter)
            : this(actionRouteTemplateFormatter, null) { }

        public void Apply(ActionModel action)
        {
            if (controllerTypeFilter?.IsBlocked(action.Controller.ControllerType) ?? false) {
                return;
            }

            foreach (var selector in action.Selectors) {
                var attributeRouteModel = selector.AttributeRouteModel;

                if (attributeRouteModel is null || string.IsNullOrEmpty(attributeRouteModel.Template)) {
                    continue;
                }

                attributeRouteModel.Template = actionRouteTemplateFormatter.Format(attributeRouteModel.Template);
            }
        }
    }
}
