using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels
{
    public class ControllerRouteConvention : IControllerModelConvention
    {
        public TypeInfo ControllerType { get; }
        public string? DefaultRoute { get; }
        public string? RoutePrefix { get; }

        /// <summary>
        /// Defines the route convention for a controller.
        /// </summary>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="defaultRouteTemplate">Used for empty routes.</param>
        /// <param name="routeTemplatePrefix">The prefix is prepend to each route.</param>
        public ControllerRouteConvention(TypeInfo controllerType, string? defaultRouteTemplate, string? routeTemplatePrefix)
        {
            ControllerType = controllerType;
            DefaultRoute = defaultRouteTemplate;
            RoutePrefix = routeTemplatePrefix;
        }

        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType != ControllerType) {
                return;
            }
            
            var lazyDefaultRouteModel = new Lazy<AttributeRouteModel>(() => new AttributeRouteModel(new RouteAttribute(DefaultRoute)));
            var lazyPrefixRouteModel = new Lazy<AttributeRouteModel>(() => new AttributeRouteModel(new RouteAttribute(RoutePrefix)));

            AttributeRouteModel createRouteModelWithPrefix(AttributeRouteModel attributeRouteModel) =>
                RoutePrefix == null 
                ? attributeRouteModel 
                : AttributeRouteModel.CombineAttributeRouteModel(lazyPrefixRouteModel.Value, attributeRouteModel);

            AttributeRouteModel createDefaultRouteModel(AttributeRouteModel emptyRouteModel) =>
                DefaultRoute == null
                ? emptyRouteModel
                : AttributeRouteModel.CombineAttributeRouteModel(lazyDefaultRouteModel.Value, emptyRouteModel);

            foreach (var selector in controller.Selectors) {
                if (selector.AttributeRouteModel is null) {
                    var attributeRouteModel = createDefaultRouteModel(new AttributeRouteModel());
                    selector.AttributeRouteModel = createRouteModelWithPrefix(attributeRouteModel);
                } else if (string.IsNullOrEmpty(selector.AttributeRouteModel.Template)) {
                    var attributeRouteModel = createDefaultRouteModel(selector.AttributeRouteModel);
                    selector.AttributeRouteModel = createRouteModelWithPrefix(attributeRouteModel);
                } else {
                    selector.AttributeRouteModel = createRouteModelWithPrefix(selector.AttributeRouteModel);
                }
            }
        }
    }
}
