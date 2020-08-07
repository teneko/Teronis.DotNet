using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels
{
    public class ControllerRouteConvention : IControllerModelConvention
    {
        public TypeInfo ControllerType { get; }
        public string? DefaultRouteTemplate { get; }
        public bool ForceDefaultRoute { get; }
        public string? PrefixRouteTemplate { get; }

        /// <summary>
        /// Defines the route convention for one controller.
        /// </summary>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="defaultRouteTemplate">Used for empty routes.</param>
        /// <param name="prefixRouteTemplate">The prefix is prepend to each route.</param>
        public ControllerRouteConvention(TypeInfo controllerType, string? defaultRouteTemplate, bool forceDefaultRoute, string? prefixRouteTemplate)
        {
            ControllerType = controllerType;
            DefaultRouteTemplate = defaultRouteTemplate;
            PrefixRouteTemplate = prefixRouteTemplate;
            ForceDefaultRoute = forceDefaultRoute;
        }

        public ControllerRouteConvention(TypeInfo controllerType, string? defaultRouteTemplate, bool forceDefaultRoute)
            : this(controllerType, defaultRouteTemplate, forceDefaultRoute, null) { }

        public ControllerRouteConvention(TypeInfo controllerType, string? defaultRouteTemplate)
            : this(controllerType, defaultRouteTemplate, false, null) { }

        public ControllerRouteConvention(TypeInfo controllerType, string? defaultRouteTemplate, string? prefixRouteTemplate)
            : this(controllerType, defaultRouteTemplate, false, prefixRouteTemplate) { }

        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType != ControllerType) {
                return;
            }

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

            foreach (var selector in controller.Selectors) {
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
    }
}
