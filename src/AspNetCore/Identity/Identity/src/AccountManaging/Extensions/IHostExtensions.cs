// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Teronis.AspNetCore.Identity.AccountManaging;
using Teronis.AspNetCore.Identity.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IHostExtensions
    {
        /// <summary>
        /// Creates a scope, resolves <see cref="IAccountManager{UserType, RoleType}"/> and invokes <paramref name="handler"/>.
        /// </summary>
        /// <typeparam name="UserType">The typ of user entity.</typeparam>
        /// <typeparam name="RoleType">The type of role entity.</typeparam>
        /// <param name="host">The host.</param>
        /// <param name="handler">The handler that gets called back.</param>
        /// <returns>The host.</returns>
        public static async Task<IHost> SeedIdentityAsync<UserType, RoleType>(this IHost host, Func<IServiceProvider, IAccountManager<UserType, RoleType>, Task> handler)
        {
            await host.CreateScopeAsync(serviceProvider => {
                var accountManager = serviceProvider.GetRequiredService<IAccountManager<UserType, RoleType>>();
                return handler(serviceProvider, accountManager);
            });

            return host;
        }

        /// <summary>
        /// Creates a scope, resolves default <see cref="IAccountManager{UserType, RoleType}"/> and invokes <paramref name="handler"/>.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static Task<IHost> SeedIdentityAsync(this IHost host, Func<IServiceProvider, IAccountManager<UserEntity, RoleEntity>, Task> handler) =>
            SeedIdentityAsync<UserEntity, RoleEntity>(host, handler);
    }
}
