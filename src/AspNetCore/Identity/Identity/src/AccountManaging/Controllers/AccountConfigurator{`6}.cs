// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects;
using Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers
{
    /// <summary>
    /// Configures the required dependencies for <see cref="AccountManager{DbContextType, UserType, RoleType}"/> and
    /// <see cref="AccountController{UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType}"/>.
    /// </summary>
    /// <typeparam name="UserDescriptorType">Type of user descriptor.</typeparam>
    /// <typeparam name="UserType">Type of user entity.</typeparam>
    /// <typeparam name="UserCreationType">Type of user view.</typeparam>
    /// <typeparam name="RoleDescriptorType">Type of role descriptor.</typeparam>
    /// <typeparam name="RoleType">Type of role entity.</typeparam>
    /// <typeparam name="RoleCreationType">Type of role view.</typeparam>
    public class AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType>
        where UserDescriptorType : IUserDescriptor
        where UserType : IAccountUserEntity
        where RoleDescriptorType : IRoleDescriptor
    {
        public IServiceCollection Services => MvcBuilder.Services;
        public IMvcBuilder MvcBuilder { get; }

        public AccountConfigurator(IMvcBuilder mvcBuilder)
        {
            MvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));
        }

        /// <summary>
        /// Adds <typeparamref name="ImplementationType"/> as <see cref="IConvertUserDescriptor{UserDescriptorType, UserType}"/> to <see cref="Services"/>.
        /// </summary>
        /// <typeparam name="ImplementationType">The impementation type.</typeparam>
        /// <returns></returns>
        public AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddUserDescriptorConverter<ImplementationType>()
            where ImplementationType : class, IConvertUserDescriptor<UserDescriptorType, UserType>
        {
            Services.AddSingleton<IConvertUserDescriptor<UserDescriptorType, UserType>, ImplementationType>();
            return this;
        }

        /// <summary>
        /// Adds <paramref name="implementationFactory"/> as <see cref="IConvertUserDescriptor{UserDescriptorType, UserType}"/> to <see cref="Services"/>.
        /// </summary>
        /// <typeparam name="ImplementationType">The implementation type.</typeparam>
        /// <param name="implementationFactory">The factory that creates <typeparamref name="ImplementationType"/>.</param>
        /// <returns></returns>
        public AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddUserDescriptorConverter<ImplementationType>(
            Func<IServiceProvider, ImplementationType> implementationFactory)
            where ImplementationType : class, IConvertUserDescriptor<UserDescriptorType, UserType>
        {
            Services.AddSingleton(implementationFactory);
            return this;
        }

        /// <summary>
        /// Adds <typeparamref name="ImplementationType"/> as <see cref="IConvertUser{UserType, UserCreationType}"/> to <see cref="Services"/>.
        /// </summary>
        /// <typeparam name="ImplementationType">The impementation type.</typeparam>
        /// <returns></returns>
        public AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddUserConverter<ImplementationType>()
            where ImplementationType : class, IConvertUser<UserType, UserCreationType>
        {
            Services.AddSingleton<IConvertUser<UserType, UserCreationType>, ImplementationType>();
            return this;
        }

        /// <summary>
        /// Adds <paramref name="implementationFactory"/> as <see cref="IConvertUser{UserType, UserCreationType}"/> to <see cref="Services"/>.
        /// </summary>
        /// <typeparam name="ImplementationType">The implementation type.</typeparam>
        /// <param name="implementationFactory">The factory that creates <typeparamref name="ImplementationType"/>.</param>
        /// <returns></returns>
        public AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddUserConverter<ImplementationType>(
            Func<IServiceProvider, ImplementationType> implementationFactory)
            where ImplementationType : class, IConvertUser<UserType, UserCreationType>
        {
            Services.AddSingleton(implementationFactory);
            return this;
        }

        /// <summary>
        /// Adds <typeparamref name="ImplementationType"/> as <see cref="IConvertRoleDescriptor{RoleDescriptorType, RoleType}"/> to <see cref="Services"/>.
        /// </summary>
        /// <typeparam name="ImplementationType">The impementation type.</typeparam>
        /// <returns></returns>
        public AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddRoleDescriptorConverter<ImplementationType>()
            where ImplementationType : class, IConvertRoleDescriptor<RoleDescriptorType, RoleType>
        {
            Services.AddSingleton<IConvertRoleDescriptor<RoleDescriptorType, RoleType>, ImplementationType>();
            return this;
        }

        /// <summary>
        /// Adds <paramref name="implementationFactory"/> as <see cref="IConvertRoleDescriptor{RoleDescriptorType, RoleType}"/> to <see cref="Services"/>.
        /// </summary>
        /// <typeparam name="ImplementationType">The implementation type.</typeparam>
        /// <param name="implementationFactory">The factory that creates <typeparamref name="ImplementationType"/>.</param>
        /// <returns></returns>
        public AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddRoleDescriptorConverter<ImplementationType>(Func<IServiceProvider, ImplementationType> implementationFactory)
            where ImplementationType : class, IConvertRoleDescriptor<RoleDescriptorType, RoleType>
        {
            Services.AddSingleton(implementationFactory);
            return this;
        }

        /// <summary>
        /// Adds <typeparamref name="ImplementationType"/> as <see cref="IConvertRole{RoleType, RoleCreationType}"/> to <see cref="Services"/>.
        /// </summary>
        /// <typeparam name="ImplementationType">The impementation type.</typeparam>
        /// <returns></returns>
        public AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddRoleConverter<ImplementationType>()
            where ImplementationType : class, IConvertRole<RoleType, RoleCreationType>
        {
            Services.AddSingleton<IConvertRole<RoleType, RoleCreationType>, ImplementationType>();
            return this;
        }

        /// <summary>
        /// Adds <paramref name="implementationFactory"/> as <see cref="IConvertRole{RoleType, RoleCreationType}"/> to <see cref="Services"/>.
        /// </summary>
        /// <typeparam name="ImplementationType">The implementation type.</typeparam>
        /// <param name="implementationFactory">The factory that creates <typeparamref name="ImplementationType"/>.</param>
        /// <returns></returns>
        public AccountConfigurator<UserDescriptorType, UserType, UserCreationType, RoleDescriptorType, RoleType, RoleCreationType> AddRoleConverter<ImplementationType>(Func<IServiceProvider, ImplementationType> implementationFactory)
            where ImplementationType : class, IConvertRole<RoleType, RoleCreationType>
        {
            Services.AddSingleton(implementationFactory);
            return this;
        }
    }
}
