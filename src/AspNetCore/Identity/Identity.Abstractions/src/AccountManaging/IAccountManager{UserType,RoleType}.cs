// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.AspNetCore.Identity.AccountManaging
{
    public interface IAccountManager<UserType, RoleType>
    {
        Task<RoleType> CreateRoleAsync(RoleType roleEntity);
        Task<RoleType> GetRoleByNameAsync(string roleName);

        Task<UserType> CreateUserAsync(UserType userEntity, string password, params string[]? roles);
        Task<UserType> GetUserByNameAsync(string userName);
    }
}
