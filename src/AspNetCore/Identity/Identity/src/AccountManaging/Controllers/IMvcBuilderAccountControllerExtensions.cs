// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects;
using Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters;
using Teronis.AspNetCore.Identity.Entities;
using Teronis.AspNetCore.Mvc.ApplicationModels;
using Teronis.AspNetCore.Mvc.ApplicationParts;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers
{
    public static partial class IMvcBuilderAccountControllerExtensions
    {
        /// <summary>
        /// <para>
        /// Registers the non-auto-discovered <see cref="AccountController{UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType}"/> 
        /// to <see cref="IMvcBuilder"/>.
        /// </para>
        /// <para>
        /// Adds default route "api/account" by adding a <see cref="SelectorRouteApplicableConvention"/> to <see cref="MvcOptions.Conventions"/>.
        /// <br/>You can override it by providing a <see cref="SelectorRouteApplicableConvention"/> yourself and enable forced default route.
        /// </para>
        /// </summary>
        /// <typeparam name="UserDescriptorType">Type of user descriptor.</typeparam>
        /// <typeparam name="UserType">Type of user entity.</typeparam>
        /// <typeparam name="UserCreationType">Type of user view.</typeparam>
        /// <typeparam name="RoleDescriptorType">Type of role descriptor.</typeparam>
        /// <typeparam name="RoleType">Type of role entity.</typeparam>
        /// <typeparam name="RoleCreationType">Type of role view.</typeparam>
        /// <param name="mvcBuilder">The MVC builder.</param>
        /// <param name="configureControllerModel">Configures the controller.</param>
        /// <returns></returns>
        public static AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddAccountControllers<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(
            this IMvcBuilder mvcBuilder, Action<ISelectedControllerModelConfiguration>? configureControllerModel = null)
            where UserDescriptorType : IUserDescriptor
            where UserType : IAccountUserEntity
            where RoleDescriptorType : IRoleDescriptor
        {
            var accountControllerTypeInfo = typeof(AccountController<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>).GetTypeInfo();

            mvcBuilder.ConfigureApplicationPartManager(setup => {
                var applicationTypes = new[] {
                    accountControllerTypeInfo
                };

                var constrainedTypesProvider = new TypesProvidingApplicationPart(applicationTypes);
                setup.ApplicationParts.Add(constrainedTypesProvider);
            });

            var controllerModelConfiguration = new ControllerModelConfiguration(accountControllerTypeInfo);

            controllerModelConfiguration.AddSelectorRouteConventionToSelectedController("api/account",
                (configuration, convention) => configuration.AddControllerConvention(convention));

            configureControllerModel?.Invoke(controllerModelConfiguration);
            mvcBuilder.Services.ApplyControllerModelConfiguration(controllerModelConfiguration);
            return new AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(mvcBuilder);
        }

        /// <summary>
        /// <para>
        /// Adds the user/role converters as singletons to the services.
        /// </para>
        /// <para>
        /// Registers the non-auto-discovered <see cref="AccountController{UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType}"/> 
        /// to <see cref="IMvcBuilder"/>.
        /// </para>
        /// <para>
        /// Adds default route "api/account" by adding a <see cref="SelectorRouteApplicableConvention"/> to <see cref="MvcOptions.Conventions"/>.
        /// <br/>You can override it by providing a <see cref="SelectorRouteApplicableConvention"/> yourself and enable forced default route.
        /// </para>
        /// </summary>
        /// <typeparam name="UserDescriptorType">Type of user descriptor.</typeparam>
        /// <typeparam name="UserType">Type of user entity.</typeparam>
        /// <typeparam name="UserCreationType">Type of user view.</typeparam>
        /// <typeparam name="RoleDescriptorType">Type of role descriptor.</typeparam>
        /// <typeparam name="RoleType">Type of role entity.</typeparam>
        /// <typeparam name="RoleCreationType">Type of role view.</typeparam>
        /// <param name="mvcBuilder"></param>
        /// <param name="userDescriptorUserConverter"></param>
        /// <param name="userUserCreationConverter"></param>
        /// <param name="roleDescriptorRoleConverter"></param>
        /// <param name="roleRoleCreationConverter"></param>
        /// <param name="configureControllerModel"></param>
        /// <returns></returns>
        public static AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddAccountControllersWithConverters<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(
            this IMvcBuilder mvcBuilder,
            IConvertUserDescriptor<UserDescriptorType, UserType> userDescriptorUserConverter, IConvertUser<UserType, UserCreationType> userUserCreationConverter,
            IConvertRoleDescriptor<RoleDescriptorType, RoleType> roleDescriptorRoleConverter, IConvertRole<RoleType, RoleCreationType> roleRoleCreationConverter,
            Action<ISelectedControllerModelConfiguration>? configureControllerModel = null)
            where UserDescriptorType : IUserDescriptor
            where UserType : IAccountUserEntity
            where RoleDescriptorType : IRoleDescriptor
        {
            var services = mvcBuilder.Services;
            services.AddSingleton(userDescriptorUserConverter);
            services.AddSingleton(userUserCreationConverter);
            services.AddSingleton(roleDescriptorRoleConverter);
            services.AddSingleton(roleRoleCreationConverter);

            var builder = mvcBuilder.AddAccountControllers<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(
                configureControllerModel: configureControllerModel);

            return builder;
        }

        /// <summary>
        /// <para>
        /// Adds default user/role converters as singletons to the services.
        /// </para>
        /// <para>
        /// Registers the non-auto-discovered and default <see cref="AccountController{UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType}"/> 
        /// to <see cref="IMvcBuilder"/>.
        /// </para>
        /// <para>
        /// Adds default route "api/account" by adding a <see cref="SelectorRouteApplicableConvention"/> to <see cref="MvcOptions.Conventions"/>.
        /// <br/>You can override it by providing a <see cref="SelectorRouteApplicableConvention"/> yourself and enable forced default route.
        /// </para>
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="configureControllerModel"></param>
        /// <returns></returns>
        public static IMvcBuilder AddAccountControllers(this IMvcBuilder mvcBuilder, Action<ISelectedControllerModelConfiguration>? configureControllerModel = null)
        {
            var services = mvcBuilder.Services;
            services.AddSingleton<IConvertUserDescriptor<UserDescriptorDatatransject, UserEntity>, UserDescriptorUserConverter>();
            services.AddSingleton<IConvertUser<UserEntity, UserCreationDatatransject>, UserUserCreationConverter>();
            services.AddSingleton<IConvertRoleDescriptor<RoleDescriptorDatatransject, RoleEntity>, RoleDescriptorRoleConverter>();
            services.AddSingleton<IConvertRole<RoleEntity, RoleCreationDatatransject>, RoleRoleCreationConverter>();

            mvcBuilder.AddAccountControllers<UserDescriptorDatatransject, UserEntity, UserCreationDatatransject, RoleDescriptorDatatransject, RoleEntity, RoleCreationDatatransject>(
                configureControllerModel: configureControllerModel);

            return mvcBuilder;
        }
    }
}
