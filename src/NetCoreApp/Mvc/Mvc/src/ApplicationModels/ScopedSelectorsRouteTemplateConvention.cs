using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teronis.Mvc.ApplicationModels.Filters;

namespace Teronis.Mvc.ApplicationModels
{
    public class ScopedSelectorsRouteTemplateConvention : IControllerModelConvention, IActionModelConvention
    {
        public IStringFormatter SelectorRouteTemplateFormatter { get; }
        public IControllerModelFilter? ControllerFilter { get; }
        public IApplicationModelFilter? ApplicationFilter { get; }
        public IActionModelFilter? ActionFilter { get; }

        /// <summary>
        /// Creates an action route template convention that formats route names.
        /// </summary>
        /// <param name="selectorRouteTemplateFormatter">Formats route template.</param>
        /// <param name="controllerFilter">Filters on controller type.</param>
        public ScopedSelectorsRouteTemplateConvention(IStringFormatter selectorRouteTemplateFormatter,
            IApplicationModelFilter? applicationFilter = null, IControllerModelFilter? controllerFilter = null,
            IActionModelFilter? actionFilter = null)
        {
            SelectorRouteTemplateFormatter = selectorRouteTemplateFormatter ?? throw new ArgumentNullException(nameof(selectorRouteTemplateFormatter));
            ControllerFilter = controllerFilter;
            ApplicationFilter = applicationFilter;
            ActionFilter = actionFilter;
        }

        internal bool IsApplicationAllowed(ApplicationModel application) =>
            ApplicationFilter?.IsAllowed(application) ?? true;

        internal bool IsControllerAllowed(ControllerModel controller) =>
            ControllerFilter?.IsAllowed(controller) ?? true;

        internal bool IsActionAllowed(ActionModel action) =>
            IsControllerAllowed(action.Controller) && (ActionFilter?.IsAllowed(action) ?? true);

        internal void FormatRouteTemplates(IEnumerable<SelectorModel> selectors)
        {
            if (selectors is null) {
                return;
            }

            foreach (var selector in selectors) {
                var attributeRouteModel = selector.AttributeRouteModel;

                if (attributeRouteModel is null || string.IsNullOrEmpty(attributeRouteModel.Template)) {
                    continue;
                }

                attributeRouteModel.Template = SelectorRouteTemplateFormatter.Format(attributeRouteModel.Template);
            }
        }

        public void Apply(ActionModel action)
        {
            if (!IsActionAllowed(action) || IsApplicationAllowed(action.Controller.Application)
                || IsApplicationAllowed(action.Controller.Application)) {
                return;
            }

            FormatRouteTemplates(action.Selectors);
        }

        public void Apply(ControllerModel controller)
        {
            if (!IsControllerAllowed(controller) || IsApplicationAllowed(controller.Application)) {
                return;
            }

            FormatRouteTemplates(controller.Selectors);
        }
    }
}
