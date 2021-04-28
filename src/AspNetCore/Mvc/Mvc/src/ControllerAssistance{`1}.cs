// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Teronis.AspNetCore.Mvc.ApplicationModels;
using Teronis.AspNetCore.Mvc.ApplicationParts;

namespace Teronis.AspNetCore.Mvc
{
    public class ControllerAssistance<T>
    {
        public IMvcBuilder MvcBuilder { get; }
        public TypeInfo ControllerTypeInfo { get; }

        public ControllerAssistance(IMvcBuilder mvcBuilder, TypeInfo controllerTypeInfo)
        {
            MvcBuilder = mvcBuilder;
            ControllerTypeInfo = controllerTypeInfo;
        }

        public ControllerAssistance(IMvcBuilder mvcBuilder, Type controllerType)
        {
            MvcBuilder = mvcBuilder;
            ControllerTypeInfo = controllerType.GetTypeInfo();
        }

        /// <summary>
        /// Makes the controller discoverable.
        /// </summary>
        /// <returns></returns>
        public ControllerAssistance<T> Discoverable()
        {
            MvcBuilder.ConfigureApplicationPartManager(partManager => {
                var constrainedTypesProvider = TypesProvidingApplicationPart.Create(ControllerTypeInfo);
                partManager.ApplicationParts.Add(constrainedTypesProvider);
            });

            return this;
        }

        /// <summary>
        /// Adds a default controller route to controller model but provides callback to customize controller model.
        /// </summary>
        /// <returns></returns>
        public ControllerAssistance<T> HasDefaultRoute(
            string defaultRoute,
            Action<ISelectedControllerModelConfiguration>? configureControllerModel = null)
        {
            var controllerModelConfiguration = new ControllerModelConfiguration(ControllerTypeInfo);

            controllerModelConfiguration.AddSelectorRouteConventionToSelectedController(defaultRoute,
                (configuration, convention) => configuration.AddControllerConvention(convention));

            configureControllerModel?.Invoke(controllerModelConfiguration);
            MvcBuilder.Services.ApplyControllerModelConfiguration(controllerModelConfiguration);
            return this;
        }
    }
}
