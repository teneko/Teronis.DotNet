using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Controllers
{
    public class AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>
        where UserDescriptorType : IUserDescriptor
        where UserType : IAccountUserEntity
        where RoleDescriptorType : IRoleDescriptor
    {
        public IServiceCollection Services => MvcBuilder.Services;
        public IMvcBuilder MvcBuilder { get; }

        public AccountManagerBuilder(IMvcBuilder mvcBuilder)
        {
            MvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));
        }

        public AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddUserDescriptorConverter<ImplementationType>()
            where ImplementationType : class, IConvertUserDescriptor<UserDescriptorType, UserType>
        {
            Services.AddSingleton<IConvertUserDescriptor<UserDescriptorType, UserType>, ImplementationType>();
            return this;
        }

        public AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddUserDescriptorConverter<ImplementationType>(Func<IServiceProvider, ImplementationType> implementationFactory)
            where ImplementationType : class, IConvertUserDescriptor<UserDescriptorType, UserType>
        {
            Services.AddSingleton(implementationFactory);
            return this;
        }

        public AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddUserConverter<ImplementationType>()
            where ImplementationType : class, IConvertUser<UserType, UserCreationType>
        {
            Services.AddSingleton<IConvertUser<UserType, UserCreationType>, ImplementationType>();
            return this;
        }

        public AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddUserConverter<ImplementationType>(Func<IServiceProvider, ImplementationType> implementationFactory)
            where ImplementationType : class, IConvertUser<UserType, UserCreationType>
        {
            Services.AddSingleton(implementationFactory);
            return this;
        }

        public AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddRoleDescriptorConverter<ImplementationType>()
            where ImplementationType : class, IConvertRoleDescriptor<RoleDescriptorType, RoleType>
        {
            Services.AddSingleton<IConvertRoleDescriptor<RoleDescriptorType, RoleType>, ImplementationType>();
            return this;
        }

        public AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddRoleDescriptorConverter<ImplementationType>(Func<IServiceProvider, ImplementationType> implementationFactory)
            where ImplementationType : class, IConvertRoleDescriptor<RoleDescriptorType, RoleType>
        {
            Services.AddSingleton(implementationFactory);
            return this;
        }

        public AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddRoleConverter<ImplementationType>()
            where ImplementationType : class, IConvertRole<RoleType, RoleCreationType>
        {
            Services.AddSingleton<IConvertRole<RoleType, RoleCreationType>, ImplementationType>();
            return this;
        }

        public AccountManagerBuilder<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddRoleConverter<ImplementationType>(Func<IServiceProvider, ImplementationType> implementationFactory)
            where ImplementationType : class, IConvertRole<RoleType, RoleCreationType>
        {
            Services.AddSingleton(implementationFactory);
            return this;
        }
    }
}
