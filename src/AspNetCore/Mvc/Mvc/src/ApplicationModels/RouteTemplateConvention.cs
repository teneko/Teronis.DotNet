using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teronis.Mvc.ApplicationModels.Filters;

namespace Teronis.Mvc.ApplicationModels
{
    public class RouteTemplateConvention : IApplicationModelConvention, IControllerModelConvention
    {
        private readonly ScopedRouteTemplateConvention scopedConvention;

        /// <summary>
        /// Creates an action route template convention that formats route names.
        /// </summary>
        /// <param name="urlComponentTemplateFormatter">Formats route template.</param>
        /// <param name="controllerFilter">Filters on controller type.</param>
        public RouteTemplateConvention(IStringFormatter urlComponentTemplateFormatter, IApplicationModelFilter? applicationFilter = null,
            IControllerModelFilter? controllerFilter = null, IActionModelFilter? actionFilter = null) =>
            scopedConvention = new ScopedRouteTemplateConvention(urlComponentTemplateFormatter, applicationFilter, controllerFilter, actionFilter);

        private void formatControllerActionsRouteTemplate(IEnumerable<ActionModel> actions)
        {
            if (actions is null) {
                return;
            }

            foreach (var action in actions) {
                if (!scopedConvention.IsActionAllowed(action)) {
                    continue;
                }

                scopedConvention.FormatRouteTemplates(action.Selectors);
            }
        }

        public void Apply(ControllerModel controller)
        {
            if (!scopedConvention.IsControllerAllowed(controller)) {
                return;
            }

            scopedConvention.FormatRouteTemplates(controller.Selectors);
            formatControllerActionsRouteTemplate(controller.Actions);
        }

        public void Apply(ApplicationModel application)
        {
            if (!scopedConvention.IsApplicationAllowed(application) || application.Controllers is null) {
                return;
            }

            foreach (var controller in application.Controllers) {
                if (!scopedConvention.IsControllerAllowed(controller)) {
                    continue;
                }

                formatControllerActionsRouteTemplate(controller.Actions);
            }
        }
    }
}
