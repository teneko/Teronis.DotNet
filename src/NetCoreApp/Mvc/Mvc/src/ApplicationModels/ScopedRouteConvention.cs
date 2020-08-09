using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teronis.Mvc.ApplicationModels.Filters;

namespace Teronis.Mvc.ApplicationModels
{
    public class ScopedRouteConvention : IControllerModelConvention, IActionModelConvention
    {
        public string? DefaultRouteTemplate { get; }
        public bool ForceDefaultRoute { get; }
        public string? PrefixRouteTemplate { get; }
        private readonly IApplicationModelFilter? applicationFilter;
        private readonly IControllerModelFilter? controllerFilter;
        private readonly IActionModelFilter? actionFilter;

        /// <summary>
        /// Defines a route convention for controllers or actions.
        /// </summary>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="defaultRouteTemplate">Used for empty routes.</param>
        /// <param name="prefixRouteTemplate">The prefix is prepend to each route.</param>
        public ScopedRouteConvention(string? defaultRouteTemplate, bool forceDefaultRoute, string? prefixRouteTemplate,
             IApplicationModelFilter? applicationFilter = null, IControllerModelFilter? controllerFilter = null,
             IActionModelFilter? actionFilter = null)
        {
            DefaultRouteTemplate = defaultRouteTemplate;
            ForceDefaultRoute = forceDefaultRoute;
            PrefixRouteTemplate = prefixRouteTemplate;
            this.applicationFilter = applicationFilter;
            this.controllerFilter = controllerFilter;
            this.actionFilter = actionFilter;
        }

        public ScopedRouteConvention(string? defaultRouteTemplate, bool forceDefaultRoute,
            IApplicationModelFilter? applicationFilter = null, IControllerModelFilter? controllerFilter = null,
            IActionModelFilter? actionFilter = null)
            : this(defaultRouteTemplate, forceDefaultRoute, null, applicationFilter, controllerFilter, actionFilter) { }

        public ScopedRouteConvention(string? defaultRouteTemplate, IApplicationModelFilter? applicationFilter = null,
            IControllerModelFilter? controllerFilter = null, IActionModelFilter? actionFilter = null)
            : this(defaultRouteTemplate, false, null, applicationFilter, controllerFilter, actionFilter) { }

        public ScopedRouteConvention(string? defaultRouteTemplate, string? prefixRouteTemplate,
            IApplicationModelFilter? applicationFilter = null, IControllerModelFilter? controllerFilter = null,
            IActionModelFilter? actionFilter = null)
            : this(defaultRouteTemplate, false, prefixRouteTemplate, applicationFilter, controllerFilter, actionFilter) { }

        internal bool IsApplicationAllowed(ApplicationModel application) =>
            applicationFilter?.IsAllowed(application) ?? true;

        internal bool IsControllerAllowed(ControllerModel controller) =>
            controllerFilter?.IsAllowed(controller) ?? true;

        internal bool IsActionAllowed(ActionModel action) =>
            actionFilter?.IsAllowed(action) ?? true;

        internal void ChangeSelectorRoutes(IEnumerable<SelectorModel> selectors)
        {
            var lazyDefaultRouteModel = new Lazy<AttributeRouteModel>(() => new AttributeRouteModel(new RouteAttribute(DefaultRouteTemplate)));
            var lazyPrefixRouteModel = new Lazy<AttributeRouteModel>(() => new AttributeRouteModel(new RouteAttribute(PrefixRouteTemplate)));

            AttributeRouteModel createRouteModelWithPrefix(AttributeRouteModel attributeRouteModel) =>
                PrefixRouteTemplate == null
                ? attributeRouteModel
                : AttributeRouteModel.CombineAttributeRouteModel(lazyPrefixRouteModel.Value, attributeRouteModel);

            AttributeRouteModel createDefaultRouteModel(AttributeRouteModel emptyRouteModel) =>
                DefaultRouteTemplate == null
                ? emptyRouteModel
                : AttributeRouteModel.CombineAttributeRouteModel(lazyDefaultRouteModel.Value, emptyRouteModel);

            foreach (var selector in selectors) {
                if (selector.AttributeRouteModel is null || ForceDefaultRoute) {
                    var attributeRouteModel = createDefaultRouteModel(new AttributeRouteModel());
                    selector.AttributeRouteModel = createRouteModelWithPrefix(attributeRouteModel);
                } else if (string.IsNullOrEmpty(selector.AttributeRouteModel.Template) || ForceDefaultRoute) {
                    var attributeRouteModel = createDefaultRouteModel(selector.AttributeRouteModel);
                    selector.AttributeRouteModel = createRouteModelWithPrefix(attributeRouteModel);
                } else {
                    selector.AttributeRouteModel = createRouteModelWithPrefix(selector.AttributeRouteModel);
                }
            }
        }

        public void Apply(ActionModel action)
        {
            if (!IsControllerAllowed(action.Controller) && !IsActionAllowed(action)) {
                return;
            }

            ChangeSelectorRoutes(action.Selectors);
        }

        public void Apply(ControllerModel controller)
        {
            if (!IsControllerAllowed(controller)) {
                return;
            }

            ChangeSelectorRoutes(controller.Selectors);
        }
    }
}
