// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teronis.AspNetCore.Mvc.ApplicationModels.Filters;

namespace Teronis.AspNetCore.Mvc.ApplicationModels
{
    public class RouteTemplateConvention : IApplicationModelConvention, IControllerModelConvention
    {
        private readonly ScopedRouteTemplateConvention scopedConvention;

        /// <summary>
        /// Creates a route template convention for appliaction or controller that formats route template of <see cref="ActionModel"/>s.
        /// </summary>
        /// <param name="urlComponentTemplateFormatter">A route template formatter.</param>
        /// <param name="applicationFilter"></param>
        /// <param name="controllerFilter">Applies formatter on specific controller models.</param>
        /// <param name="actionFilter">Applies formatter on specific action models.</param>
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
