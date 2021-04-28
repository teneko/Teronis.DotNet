// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc;
using Teronis.AspNetCore.Mvc.ApplicationModels.Filters;
using Teronis.AspNetCore.Mvc.Case;

namespace Teronis.AspNetCore.Mvc.ApplicationModels
{
    public static class IControllerModelConfigurationExtensions
    {
        /// <summary>
        /// Adds an <see cref="SelectorRouteApplicableConvention"/> instance to <see cref="MvcOptions"/> that only operates on controller model with controller
        /// on <see cref="ISelectedControllerModelConfiguration.SelectedControllerType"/> of <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="forceDefaultRoute"></param>
        /// <param name="addConvention"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorRouteConventionToSelectedController(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, bool forceDefaultRoute, Action<ISelectedControllerModelConfiguration, SelectorRouteApplicableConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new SelectorRouteApplicableConvention(defaultRouteTemplate, forceDefaultRoute, controllerFilter: controllerFilter);
            addConvention(configuration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds a pre-defined <see cref="SelectorRouteApplicableConvention"/> instance to <see cref="MvcOptions"/> that only operates on controller model with controller
        /// on <see cref="ISelectedControllerModelConfiguration.SelectedControllerType"/> of <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="addConvention"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorRouteConventionToSelectedController(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, Action<ISelectedControllerModelConfiguration, SelectorRouteApplicableConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new SelectorRouteApplicableConvention(defaultRouteTemplate, controllerFilter: controllerFilter);
            addConvention(configuration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="SelectorRouteApplicableConvention"/> instance to <see cref="MvcOptions"/> that only operates on controller model with controller
        /// on <see cref="ISelectedControllerModelConfiguration.SelectedControllerType"/> of <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="defaultRouteTemplate"></param>
        /// <param name="prefixRouteTemplate"></param>
        /// <param name="addConvention"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorRouteConventionToSelectedController(this ISelectedControllerModelConfiguration configuration,
            string? defaultRouteTemplate, string? prefixRouteTemplate, Action<ISelectedControllerModelConfiguration, SelectorRouteApplicableConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new SelectorRouteApplicableConvention(defaultRouteTemplate, prefixRouteTemplate, controllerFilter: controllerFilter);
            addConvention(configuration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="SelectorRouteTemplateApplicableConvention"/> instance to <see cref="MvcOptions"/> that only operates on controller model with controller
        /// on <see cref="ISelectedControllerModelConfiguration.SelectedControllerType"/> of <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="routeTemplateFormatter"></param>
        /// <param name="addConvention"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorRouteTemplateConventionToSelectedController(this ISelectedControllerModelConfiguration configuration,
            IStringFormatter routeTemplateFormatter, Action<ISelectedControllerModelConfiguration, SelectorRouteTemplateApplicableConvention> addConvention)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new SelectorRouteTemplateApplicableConvention(routeTemplateFormatter, controllerFilter: controllerFilter);
            addConvention(configuration, convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="SelectorRouteTemplateApplicableConvention"/> instance to <see cref="MvcOptions"/> that only operates on controller model with controller
        /// on <see cref="ISelectedControllerModelConfiguration.SelectedControllerType"/> of <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="caseType"></param>
        /// <param name="addConvention"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorRouteTemplateConventionToSelectedController(this ISelectedControllerModelConfiguration configuration, CaseType caseType,
            Action<ISelectedControllerModelConfiguration, SelectorRouteTemplateApplicableConvention> addConvention)
        {
            var formatter = new RouteTemplateCaseFormatter(caseType);
            return AddSelectorRouteTemplateConventionToSelectedController(configuration, formatter, addConvention);
        }

        /// <summary>
        /// Adds an <see cref="RouteTemplateConvention"/> instance to <see cref="MvcOptions"/> that operates on controller of type
        /// <see cref="ISelectedControllerModelConfiguration.SelectedControllerType"/> of <paramref name="configuration"/> and action models.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="routeTemplateFormatter"></param>
        /// <param name="actionFilter"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorRouteTemplateConventionToSelectedController(this ISelectedControllerModelConfiguration configuration,
            IStringFormatter routeTemplateFormatter, IActionModelFilter? actionFilter = null)
        {
            var controllerFilter = new ControllerTypeFilter(configuration.SelectedControllerType);
            var convention = new RouteTemplateConvention(routeTemplateFormatter, controllerFilter: controllerFilter, actionFilter: actionFilter);
            configuration.AddControllerConvention(convention);
            return configuration;
        }

        /// <summary>
        /// Adds an <see cref="RouteTemplateConvention"/> instance to <see cref="MvcOptions"/> that operates on all action models independently
        /// of application model and controller model.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="caseType"></param>
        /// <param name="actionFilter"></param>
        /// <returns></returns>
        public static ISelectedControllerModelConfiguration AddSelectorRouteTemplateConventionToSelectedController(this ISelectedControllerModelConfiguration configuration,
            CaseType caseType, IActionModelFilter? actionFilter = null)
        {
            var formatter = new RouteTemplateCaseFormatter(caseType);
            return AddSelectorRouteTemplateConventionToSelectedController(configuration, formatter, actionFilter: actionFilter);
        }
    }
}
