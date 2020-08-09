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
        /// Adds an <see cref="ScopedSelectorsRouteConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="forceDefaultRoute"></param>
        /// <param name="prefixRouteTemplate"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedSelectorsRouteConvention(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, bool forceDefaultRoute, string? prefixRouteTemplate,
            Action<DelegatedControllerModelConfiguration, ScopedSelectorsRouteConvention> addConvention,
            TypeInfo? controllerType = null)
        {
            var controllerFilter = new ControllerTypeFilter(controllerType ?? configuration.SelectedControllerType);
            var convention = new ScopedSelectorsRouteConvention(defaultRouteTemplate, forceDefaultRoute, prefixRouteTemplate, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedSelectorsRouteConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="forceDefaultRoute"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedSelectorsRouteConvention(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, bool forceDefaultRoute, Action<DelegatedControllerModelConfiguration, ScopedSelectorsRouteConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new ScopedSelectorsRouteConvention(defaultRouteTemplate, forceDefaultRoute, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedSelectorsRouteConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedSelectorsRouteConvention(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, Action<DelegatedControllerModelConfiguration, ScopedSelectorsRouteConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new ScopedSelectorsRouteConvention(defaultRouteTemplate, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedSelectorsRouteConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="controllerType"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="prefixRouteTemplate"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedSelectorsRouteConvention(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, string? prefixRouteTemplate, Action<DelegatedControllerModelConfiguration, ScopedSelectorsRouteConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new ScopedSelectorsRouteConvention(defaultRouteTemplate, prefixRouteTemplate, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedSelectorsRouteTemplateConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="selectorRouteTemplateFormatter"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedSelectorsRouteTemplateConvention(this ISelectedControllerModelConfiguration configuration,
            IStringFormatter selectorRouteTemplateFormatter, Action<DelegatedControllerModelConfiguration, ScopedSelectorsRouteTemplateConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new ScopedSelectorsRouteTemplateConvention(selectorRouteTemplateFormatter, controllerFilter: controllerFilter);
            var delegatedConfiguration = new DelegatedControllerModelConfiguration(configuration);
            addConvention(delegatedConfiguration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="ScopedSelectorsRouteTemplateConvention"/> instance to
        /// <see cref="MvcOptions"/> which only operates one level deep.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="caseType"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddScopedSelectorsRouteTemplateConvention(this ISelectedControllerModelConfiguration configuration, CaseType caseType,
            Action<DelegatedControllerModelConfiguration, ScopedSelectorsRouteTemplateConvention> addConvention)
        {
            var formatter = new CaseFormatter(caseType);
            return AddScopedSelectorsRouteTemplateConvention(configuration, formatter, addConvention);
        }

        /// <summary>
        /// Adds an <see cref="SelectorsRouteTemplateConvention"/> instance to
        /// <see cref="MvcOptions"/> which operates recursively.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="selectorRouteTemplateFormatter"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorsRouteTemplateConvention(this ISelectedControllerModelConfiguration configuration,
            IStringFormatter selectorRouteTemplateFormatter, IActionModelFilter? actionFilter = null)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new SelectorsRouteTemplateConvention(selectorRouteTemplateFormatter, controllerFilter: controllerFilter, actionFilter: actionFilter);
            configuration.AddControllerConvention(convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="SelectorsRouteTemplateConvention"/> instance to
        /// <see cref="MvcOptions"/> which operates recursively.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="caseType"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorsRouteTemplateConvention(this ISelectedControllerModelConfiguration configuration,
            CaseType caseType, IActionModelFilter? actionFilter = null)
        {
            var formatter = new CaseFormatter(caseType);
            return AddSelectorsRouteTemplateConvention(configuration, formatter, actionFilter: actionFilter);
        }
    }
}
