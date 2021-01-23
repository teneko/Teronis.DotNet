using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Teronis.Mvc.ApplicationModels.Filters;
using Teronis.Mvc.Case;

namespace Teronis.Mvc.ApplicationModels
{
    public static class IControllerModelConfigurationExtensions
    {
        /// <summary>
        /// Adds an <see cref="ScopedRouteConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="forceDefaultRoute"></param>
        /// <param name="prefixRouteTemplate"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedRouteConvention(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, bool forceDefaultRoute, string? prefixRouteTemplate,
            Action<DelegatedControllerModelConfiguration, ScopedRouteConvention> addConvention,
            TypeInfo? controllerType = null)
        {
            var controllerFilter = new ControllerTypeFilter(controllerType ?? configuration.SelectedControllerType);
            var convention = new ScopedRouteConvention(defaultRouteTemplate, forceDefaultRoute, prefixRouteTemplate, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedRouteConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="forceDefaultRoute"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedRouteConvention(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, bool forceDefaultRoute, Action<DelegatedControllerModelConfiguration, ScopedRouteConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new ScopedRouteConvention(defaultRouteTemplate, forceDefaultRoute, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedRouteConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedRouteConvention(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, Action<DelegatedControllerModelConfiguration, ScopedRouteConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new ScopedRouteConvention(defaultRouteTemplate, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedRouteConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="prefixRouteTemplate"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedRouteConvention(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, string? prefixRouteTemplate, Action<DelegatedControllerModelConfiguration, ScopedRouteConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new ScopedRouteConvention(defaultRouteTemplate, prefixRouteTemplate, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedRouteTemplateConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="routeTemplateFormatter"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedRouteTemplateConvention(this ISelectedControllerModelConfiguration configuration,
            IStringFormatter routeTemplateFormatter, Action<DelegatedControllerModelConfiguration, ScopedRouteTemplateConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new ScopedRouteTemplateConvention(routeTemplateFormatter, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedRouteTemplateConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="caseType"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedRouteTemplateConvention(this ISelectedControllerModelConfiguration configuration, CaseType caseType,
            Action<DelegatedControllerModelConfiguration, ScopedRouteTemplateConvention> addConvention)
        {
            var formatter = new RouteTemplateCaseFormatter(caseType);
            return AddScopedRouteTemplateConvention(configuration, formatter, addConvention);
        }

        /// <summary>
        /// Adds an <see cref="RouteTemplateConvention"/> instance to
        /// <see cref="MvcOptions"/> which operates recursively.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="routeTemplateFormatter"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddRouteTemplateConvention(this ISelectedControllerModelConfiguration configuration,
            IStringFormatter routeTemplateFormatter, IActionModelFilter? actionFilter = null)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new RouteTemplateConvention(routeTemplateFormatter, controllerFilter: controllerFilter, actionFilter: actionFilter);
            configuration.AddControllerConvention(convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="RouteTemplateConvention"/> instance to
        /// <see cref="MvcOptions"/> which operates recursively.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="caseType"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddRouteTemplateConvention(this ISelectedControllerModelConfiguration configuration,
            CaseType caseType, IActionModelFilter? actionFilter = null)
        {
            var formatter = new RouteTemplateCaseFormatter(caseType);
            return AddRouteTemplateConvention(configuration, formatter, actionFilter: actionFilter);
        }
    }
}
