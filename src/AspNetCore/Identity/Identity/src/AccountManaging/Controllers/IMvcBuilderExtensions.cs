using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects;
using Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters;
using Teronis.AspNetCore.Identity.Entities;
using Teronis.Mvc;
using Teronis.Mvc.ApplicationModels;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers
{
    public static partial class IMvcBuilderExtensions
    {
        public static AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddAccountControllers<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(
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

            controllerModelConfiguration.AddScopedRouteConvention("api/account",
                (configuration, convention) => configuration.AddControllerConvention(convention));

            configureControllerModel?.Invoke(controllerModelConfiguration);
            mvcBuilder.Services.ApplyControllerModelConfiguration(controllerModelConfiguration);
            return new AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(mvcBuilder);
        }

        public static AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddAccountControllersWithConverters<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(
            this IMvcBuilder mvcBuilder,
            IConvertUserDescriptor<UserDescriptorType, UserType> userDescriptorUserConverter, IConvertUser<UserType, UserCreationType> userUserCreationConverter,
            IConvertRoleDescriptor<RoleDescriptorType, RoleType> roleDescriptorRoleConverter, IConvertRole<RoleType, RoleCreationType> roleRoleCreationConverter)
            where UserDescriptorType : IUserDescriptor
            where UserType : IAccountUserEntity
            where RoleDescriptorType : IRoleDescriptor
        {
            var services = mvcBuilder.Services;
            services.AddSingleton(userDescriptorUserConverter);
            services.AddSingleton(userUserCreationConverter);
            services.AddSingleton(roleDescriptorRoleConverter);
            services.AddSingleton(roleRoleCreationConverter);
            var builder = mvcBuilder.AddAccountControllers<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>();
            return builder;
        }

        public static IMvcBuilder AddAccountControllers(this IMvcBuilder mvcBuilder, Action<ISelectedControllerModelConfiguration>? configureControllerModel = null)
        {
            var services = mvcBuilder.Services;
            services.AddSingleton<IConvertUserDescriptor<UserDescriptorDatatransject, UserEntity>, UserDescriptorUserConverter>();
            services.AddSingleton<IConvertUser<UserEntity, UserCreationDatatransject>, UserUserCreationConverter>();
            services.AddSingleton<IConvertRoleDescriptor<RoleDescriptorDatatransject, RoleEntity>, RoleDescriptorRoleConverter>();
            services.AddSingleton<IConvertRole<RoleEntity, RoleCreationDatatransject>, RoleRoleCreationConverter>();
            mvcBuilder.AddAccountControllers<UserDescriptorDatatransject, UserEntity, UserCreationDatatransject, RoleDescriptorDatatransject, RoleEntity, RoleCreationDatatransject>(configureControllerModel: configureControllerModel);
            return mvcBuilder;
        }
    }
}
