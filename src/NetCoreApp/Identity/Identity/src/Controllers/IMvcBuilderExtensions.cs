using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Entities;
using Teronis.Mvc;
using Teronis.Mvc.ApplicationModels;

namespace Teronis.Identity.Controllers
{
    public static partial class IMvcBuilderExtensions
    {
        public static AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddAccountControllers<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(
            this IMvcBuilder mvcBuilder)
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

            mvcBuilder.Services.ConfigureControllerModel(controllerModelConfiguration);

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
            var builder = AddAccountControllers<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(mvcBuilder);
            return builder;
        }

        public static IMvcBuilder AddAccountControllers(this IMvcBuilder mvcBuilder)
        {
            var services = mvcBuilder.Services;
            services.AddSingleton<IConvertUserDescriptor<UserDescriptorDatatransject, UserEntity>, UserDescriptorUserConverter>();
            services.AddSingleton<IConvertUser<UserEntity, UserCreationDatatransject>, UserUserCreationConverter>();
            services.AddSingleton<IConvertRoleDescriptor<RoleDescriptorDatatransject, RoleEntity>, RoleDescriptorRoleConverter>();
            services.AddSingleton<IConvertRole<RoleEntity, RoleCreationDatatransject>, RoleRoleCreationConverter>();
            AddAccountControllers<UserDescriptorDatatransject, UserEntity, UserCreationDatatransject, RoleDescriptorDatatransject, RoleEntity, RoleCreationDatatransject>(mvcBuilder);
            return mvcBuilder;
        }
    }
}
