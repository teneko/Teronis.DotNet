using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Teronis.Identity.Bearer.Controllers;
using Teronis.Mvc.Case;
using Teronis.Reflection;

namespace Teronis.Mvc.ApplicationModels
{
    public static class IControllerModelConfigurationExtensions
    {
        /// <summary>
        /// Adds <see cref="IControllerModelConfiguration.ControllerType"/> scoped
        /// <see cref="ControllerRouteConvention"/> instance to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="forceDefaultRoute"></param>
        /// <param name="prefixRouteTemplate"></param>
        /// <returns></returns>
        public static IControllerModelConfiguration AddControllerRouteConvention(this IControllerModelConfiguration configuration,
            TypeInfo controllerType, string? defaultRouteTemplate, bool forceDefaultRoute, string? prefixRouteTemplate)
        {
            var controllerRouteConvention = new ControllerRouteConvention(controllerType, defaultRouteTemplate, forceDefaultRoute, prefixRouteTemplate);
            return configuration.AddControllerConvention(controllerRouteConvention);
        }

        /// <summary>
        /// Adds <see cref="IControllerModelConfiguration.ControllerType"/> scoped
        /// <see cref="ControllerRouteConvention"/> instance to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="forceDefaultRoute"></param>
        /// <returns></returns>
        public static IControllerModelConfiguration AddControllerRouteConvention(this IControllerModelConfiguration configuration,
            TypeInfo controllerType, string? defaultRouteTemplate, bool forceDefaultRoute)
        {
            var controllerRouteConvention = new ControllerRouteConvention(controllerType, defaultRouteTemplate, forceDefaultRoute);
            return configuration.AddControllerConvention(controllerRouteConvention);
        }

        /// <summary>
        /// Adds <see cref="IControllerModelConfiguration.ControllerType"/> scoped
        /// <see cref="ControllerRouteConvention"/> instance to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <returns></returns>
        public static IControllerModelConfiguration AddControllerRouteConvention(this IControllerModelConfiguration configuration,
            TypeInfo controllerType, string? defaultRouteTemplate)
        {
            var controllerRouteConvention = new ControllerRouteConvention(controllerType, defaultRouteTemplate);
            return configuration.AddControllerConvention(controllerRouteConvention);
        }

        /// <summary>
        /// Adds <see cref="IControllerModelConfiguration.ControllerType"/> scoped
        /// <see cref="ControllerRouteConvention"/> instance to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="prefixRouteTemplate"></param>
        /// <returns></returns>
        public static IControllerModelConfiguration AddControllerRouteConvention(this IControllerModelConfiguration configuration,
            TypeInfo controllerType, string? defaultRouteTemplate, string? prefixRouteTemplate)
        {
            var controllerRouteConvention = new ControllerRouteConvention(controllerType, defaultRouteTemplate, prefixRouteTemplate);
            return configuration.AddControllerConvention(controllerRouteConvention);
        }

        /// <summary>
        /// Adds <see cref="IControllerModelConfiguration.ControllerType"/> scoped 
        /// <see cref="ControllerActionRouteTemplateConvention"/> instance to <see cref="MvcOptions"/>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="actionNameFormatter"></param>
        /// <returns></returns>
        public static IControllerModelConfiguration AddControllerActionRouteTemplateConvention(this IControllerModelConfiguration configuration, 
            IStringFormatter actionNameFormatter)
        {
            var typeInfoFilter = new TypeInfoFilter(configuration.ControllerType);
            var actionNameConvention = new ControllerActionRouteTemplateConvention(actionNameFormatter, typeInfoFilter);
            configuration.AddActionConvention(actionNameConvention);
            return configuration;
        }

        /// <summary>
        /// Adds <see cref="IControllerModelConfiguration.ControllerType"/> scoped 
        /// <see cref="ControllerActionRouteTemplateConvention"/> instance to <see cref="MvcOptions"/>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="caseType"></param>
        /// <returns></returns>
        public static IControllerModelConfiguration AddControllerActionRouteTemplateConvention(this IControllerModelConfiguration configuration, CaseType caseType)
        {
            var actionNameFormatter = new CaseFormatter(caseType);
            return AddControllerActionRouteTemplateConvention(configuration, actionNameFormatter);
        }
    }
}
