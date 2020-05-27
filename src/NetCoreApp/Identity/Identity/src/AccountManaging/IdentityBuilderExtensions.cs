using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Entities;

namespace Teronis.Identity
{
    public static partial class IdentityBuilderExtensions
    {
        private static IdentityBuilder addAccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(this IdentityBuilder identityBuilder,
            Func<IServiceProvider, AccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>>? getRequiredService)
            where UserDescriptorType : IUserDescriptor
            where UserType : class
            where RoleDescriptorType : IRoleDescriptor
            where RoleType : class
        {
            var services = identityBuilder.Services;

            if (getRequiredService is null) {
                services.AddScoped<AccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>>();
            } else {
                services.AddScoped(getRequiredService);
            }

            services.AddScoped<IAccountManager<UserDescriptorType, UserCreationType, RoleDescriptorType, RoleCreationType>>(serviceProvider =>
                serviceProvider.GetRequiredService<AccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>>());

            return identityBuilder;
        }

        public static IdentityBuilder AddAccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(this IdentityBuilder identityBuilder)
            where UserDescriptorType : IUserDescriptor
            where UserType : class
            where RoleDescriptorType : IRoleDescriptor
            where RoleType : class
        {
            addAccountManager<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>(identityBuilder, null);
            return identityBuilder;
        }

        public static IdentityBuilder AddAccountManager(this IdentityBuilder identityBuilder)
        {
            var services = identityBuilder.Services;
            services.AddScoped<AccountManager>();

            AccountManager getRequiredService(IServiceProvider serviceProvider) =>
                serviceProvider.GetRequiredService<AccountManager>();

            services.AddScoped<IAccountManager>(getRequiredService);
            addAccountManager(identityBuilder, getRequiredService);
            return identityBuilder;
        }
    }
}
