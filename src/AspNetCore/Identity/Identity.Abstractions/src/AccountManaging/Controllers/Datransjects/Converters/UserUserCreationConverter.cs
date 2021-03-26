// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects.Converters
{
    public class UserUserCreationConverter : IConvertUser<UserEntity, UserCreationDatatransject>
    {
        public UserCreationDatatransject Convert(UserEntity source, string[]? roles) =>
            source.ToUserCreation(roles);
    }
}
